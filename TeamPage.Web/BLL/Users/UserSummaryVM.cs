using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamPage.Web.BLL.Users
{
    public class UserSummaryVM
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<UserAgencySummaryVM> Agencies { get; set; }
        public List<UserClientSummaryVM> Clients { get; set; }

        public bool WorksForExactlyOneCompany()
        {
            var companyCount = (Agencies?.Count ?? 0) + (Clients?.Count ?? 0);
            return companyCount == 1;
        }

        public bool WorksForMultipleCompanies()
        {
            var companyCount = (Agencies?.Count ?? 0) + (Clients?.Count ?? 0);
            return companyCount > 1;
        }
    }
}
