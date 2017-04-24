using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamPage.Web.Data;
using TeamPage.Web.Models;

namespace TeamPage.Web.BLL.Agencies.Impl
{
    public class AgencyService : IAgencyService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private IHttpContextAccessor _contextAccessor;

        public AgencyService(ApplicationDbContext dbContext, UserManager<User> userManager, IHttpContextAccessor contextAccessor)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
        }

        public async Task<int> SetupAgencyAsync(SetupAgency cmd)
        {
            var id = _userManager.GetUserId(_contextAccessor.HttpContext.User);
            var user = await _dbContext.Users
                    .Include(u => u.AgencyAssignments)
                        .ThenInclude(x => x.Agency)
                    .SingleOrDefaultAsync(u => u.Id == id);
            var agency = new Agency
            {
               Name = cmd.Name,
               Url = cmd.Url,
               UniqueCode = cmd.UniqueCode
            };
            _dbContext.Agencies.Add(agency);
            user.AgencyAssignments.Add(new UserAgencyAssignment
            {
                AgencyId = agency.Id,
                UserId = id
            });
            await _dbContext.SaveChangesAsync();
            return agency.Id;
        }

        public async Task<AgencyVM[]> GetAgenciesAsync()
        {
            var id = _userManager.GetUserId(_contextAccessor.HttpContext.User);
            var user = await _dbContext.Users
                    .Include(u => u.AgencyAssignments)
                        .ThenInclude(x => x.Agency)
                            .ThenInclude(a => a.ClientAssignments)
                                .ThenInclude(x => x.Client)
                                    .ThenInclude(c => c.UserAssignments)
                                        .ThenInclude(ua => ua.User)
                    .Include(u => u.AgencyAssignments)
                        .ThenInclude(x => x.Agency)
                            .ThenInclude(a => a.UserAssignments)
                                .ThenInclude(x => x.User)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(u => u.Id == id);
            return map(user.AgencyAssignments);
        }


        public async Task<AgencyVM> GetAgencyAsync(string id)
        {
            Int32 intId;
            if (Int32.TryParse(id, out intId))
            {
                return (await GetAgenciesAsync()).FirstOrDefault(a => a.Id == intId);
            }
            else
            {
                return (await GetAgenciesAsync()).FirstOrDefault(a => a.UniqueCode.Equals(id, StringComparison.OrdinalIgnoreCase));
            }
        }

        #region | Private mapping methods |

        private AgencyVM[] map(ICollection<UserAgencyAssignment> agencyAssignments)
        {
            if (agencyAssignments == null)
            {
                return new AgencyVM[0];
            }

            var results = new List<AgencyVM>();
            foreach (var assignment in agencyAssignments)
            {
                results.Add(map(assignment));
            }
            return results.Where(x => x != null).ToArray();
        }
        private AgencyVM map(UserAgencyAssignment a)
        {
            if (a == null)
                return null;

            return new AgencyVM
            {
                Id = a.Agency.Id,
                Name = a.Agency.Name,
                Url = a.Agency.Url,
                UniqueCode = a.Agency.UniqueCode,
                Users = mapUsers(a.Agency.UserAssignments).ToArray(),
                Clients = map(a.Agency.ClientAssignments).ToArray()
            };
        }

        private IEnumerable<AgencyUserVM> mapUsers(IEnumerable<UserAgencyAssignment> assignments)
        {
            if (assignments != null)
            {
                foreach (var a in assignments)
                {
                    yield return mapOtherUser(a);
                }
            }
        }


        private AgencyUserVM mapOtherUser(UserAgencyAssignment a)
        {
            return new AgencyUserVM
            {
                Email = a.User.Email,
                FirstName = a.User.FirstName,
                LastName = a.User.LastName,
                UserId = a.User.Id
            };
        }

        private IEnumerable<AgencyClientVM> map(IEnumerable<AgencyClientAssignment> assignments)
        {
            if (assignments != null)
            {
                foreach (var a in assignments)
                {
                    yield return mapClient(a);
                }
            }
        }

        private AgencyClientVM mapClient(AgencyClientAssignment a)
        {
            return new AgencyClientVM
            {
                ClientId = a.ClientId,
                Name = a.Client.Name,
                Url = a.Client.Url,
                Users = mapClientUsers(a.Client.UserAssignments).ToArray()
            };
        }

        private IEnumerable<AgencyClientUserVM> mapClientUsers(IEnumerable<UserClientAssignment> uca)
        {
            if (uca != null)
            {
                foreach (var x in uca)
                {
                    yield return mapClientUser(x);
                }
            }
        }

        private AgencyClientUserVM mapClientUser(UserClientAssignment uca)
        {
            return new AgencyClientUserVM
            {
                FirstName = uca.User.FirstName,
                LastName = uca.User.LastName,
                UserId = uca.User.Id,
                Email = uca.User.Email
            };
        }

        #endregion 
    }
}
