using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamPage.Web.BLL.Users
{
    public interface IUserService
    {
        Task<UserSummaryVM> GetCurrentUserAsync();
    }
}
