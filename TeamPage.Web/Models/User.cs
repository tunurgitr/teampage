using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TeamPage.Web.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<UserAgencyAssignment> AgencyAssignments { get; set; }
        public ICollection<UserClientAssignment> ClientAssignments { get; set; }
    }
}
