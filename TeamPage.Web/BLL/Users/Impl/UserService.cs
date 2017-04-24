using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamPage.Web.Data;
using TeamPage.Web.Models;

namespace TeamPage.Web.BLL.Users.Impl
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private IHttpContextAccessor _contextAccessor;

        public UserService(ApplicationDbContext dbContext, UserManager<User> userManager, SignInManager<User> signInManager, IHttpContextAccessor contextAccessor)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _contextAccessor = contextAccessor;
        }
        public async Task<UserSummaryVM> GetCurrentUserAsync()
        {
            var id = _userManager.GetUserId(_contextAccessor.HttpContext.User);
            var user = await _dbContext.Users
                    .Include(u => u.AgencyAssignments)
                        .ThenInclude(x => x.Agency)
                    .Include(u => u.ClientAssignments)
                        .ThenInclude(x => x.Client)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(u => u.Id == id);

            var result = map(user);
            if (result == null)
            {
                // artifact on dev environment if DB is cleared while user is still logged in
                await _signInManager.SignOutAsync();
            }
            return result;
        }


        #region | Private mapping methods |

        private UserSummaryVM map(User user)
        {
            if (user == null)
                return null;
            return new UserSummaryVM
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Agencies = map(user.AgencyAssignments)?.ToList(),
                Clients = map(user.ClientAssignments)?.ToList()
            };
        }
        private IEnumerable<UserAgencySummaryVM> map(IEnumerable<UserAgencyAssignment> assignments)
        {
            if (assignments != null)
            {
                foreach (var a in assignments)
                    yield return map(a);
            }
        }
        private IEnumerable<UserClientSummaryVM> map(IEnumerable<UserClientAssignment> assignments)
        {
            if (assignments != null)
            {
                foreach (var a in assignments)
                    yield return map(a);
            }
        }

        private UserAgencySummaryVM map(UserAgencyAssignment assignment)
        {
            if (assignment == null)
            {
                return null;
            }
            return new UserAgencySummaryVM
            {
                AgencyId = assignment.AgencyId,
                Name = assignment.Agency?.Name,
                Url = assignment.Agency?.Url,
                UniqueCode = assignment.Agency?.UniqueCode

            };
        }

        private UserClientSummaryVM map(UserClientAssignment assignment)
        {
            if (assignment == null)
            {
                return null;
            }
            return new UserClientSummaryVM
            {
                ClientId = assignment.ClientId,
                Name = assignment.Client?.Name,
                Url = assignment.Client?.Url
            };
        }

        #endregion

    }
}
