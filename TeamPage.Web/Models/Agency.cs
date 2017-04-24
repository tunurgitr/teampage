using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TeamPage.Web.Models
{
    public class Agency
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Url { get; set; }

        [StringLength(50)]
        public string UniqueCode { get; set; }

        public virtual ICollection<AgencyClientAssignment> ClientAssignments { get; set; }

        public virtual ICollection<UserAgencyAssignment> UserAssignments { get; set; }
    }
}
