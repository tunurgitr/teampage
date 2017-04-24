using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TeamPage.Web.Models
{
    public class Client
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Url { get; set; }

        public virtual ICollection<UserClientAssignment> UserAssignments { get; set; }

        public virtual ICollection<AgencyClientAssignment> AgencyAssignments { get; set; }

    }
}
