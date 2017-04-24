using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamPage.Web.Data;
using TeamPage.Web.Models;

namespace TeamPage.Web.BLL.Clients.Impl
{
    public class ClientService : IClientService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private IHttpContextAccessor _contextAccessor;

        public ClientService(ApplicationDbContext dbContext, UserManager<User> userManager, IHttpContextAccessor contextAccessor)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
        }


        public async Task<int> RegisterClientAsync(RegisterClient cmd)
        {
            // #TODO: integrate FluentValidation

            var id = _userManager.GetUserId(_contextAccessor.HttpContext.User);

            var agency = await _dbContext.Agencies.FirstOrDefaultAsync(a => a.UniqueCode.Equals(cmd.AgencyUniqueCode, StringComparison.OrdinalIgnoreCase));
            if (agency == null)
            {
                throw new ArgumentException("Agency not found", "AgencyUniqueCode");
            }
            var user = await _dbContext.Users
                .Include(u => u.ClientAssignments)
                    .ThenInclude(x => x.Client)
                .SingleOrDefaultAsync(u => u.Id == id);
            
            var client = new Client
            {
                Name = cmd.Name,
                Url = cmd.Url,
            };
            _dbContext.Clients.Add(client);
            user.ClientAssignments.Add(new UserClientAssignment
            {
                ClientId= client.Id,
                UserId = id
            });
            _dbContext.AgencyClientAssignments.Add(new AgencyClientAssignment
            {
                AgencyId = agency.Id,
                ClientId = client.Id
            });
            await _dbContext.SaveChangesAsync();
            return agency.Id;
        }

        public async Task<ClientVM[]> GetClientsAsync()
        {
            var id = _userManager.GetUserId(_contextAccessor.HttpContext.User);
            var user = await _dbContext.Users
                    .Include(u => u.ClientAssignments)
                        .ThenInclude(x => x.Client)
                            .ThenInclude(a => a.AgencyAssignments)
                                .ThenInclude(x => x.Agency)
                                    .ThenInclude(c => c.UserAssignments)
                                        .ThenInclude(ua => ua.User)
                    .Include(u => u.ClientAssignments)
                        .ThenInclude(x => x.Client)
                            .ThenInclude(a => a.UserAssignments)
                                .ThenInclude(x => x.User)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(u => u.Id == id);
            return map(user.ClientAssignments).ToArray();
        }


        #region | Private Mapping methods |

        private IEnumerable<ClientVM> map(IEnumerable<UserClientAssignment> userClientAssignments)
        {
            if (userClientAssignments != null)
            {
                foreach (var a in userClientAssignments)
                    yield return map(a);
            }
        }

        private ClientVM map(UserClientAssignment a)
        {
            return new ClientVM
            {
                Id = a.ClientId,
                Name = a.Client.Name,
                Url = a.Client.Url,
                Users = mapUsers(a.Client.UserAssignments).ToArray(),
                Agencies = mapAgencies(a.Client.AgencyAssignments).ToArray()
            };
        }

        private IEnumerable<ClientUserVM> mapUsers(IEnumerable<UserClientAssignment> userAssignments)
        {
                if (userAssignments != null)
                {
                    foreach (var u in userAssignments)
                        yield return mapUser(u);
                }
            }

        private ClientUserVM mapUser(UserClientAssignment u)
        {
            return new ClientUserVM
            {
                UserId = u.UserId,
                Email = u.User.Email,
                FirstName = u.User.FirstName,
                LastName = u.User.LastName
            };
        }
    

        private IEnumerable<ClientAgencyVM> mapAgencies(IEnumerable<AgencyClientAssignment> agencyAssignments)
        {
            if (agencyAssignments != null)
            {
                foreach (var a in agencyAssignments)
                    yield return mapAgency(a);
            }
        }

        private ClientAgencyVM mapAgency(AgencyClientAssignment a)
        {
            return new ClientAgencyVM
            {
                AgencyId = a.AgencyId,
                Name = a.Agency.Name,
                Url = a.Agency.Url
            };
        }

        #endregion

    }
}
