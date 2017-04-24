using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TeamPage.Web.Models
{
    public class UserAgencyAssignment
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public int AgencyId { get; set; }

        public virtual User User { get; set; }
        public virtual Agency Agency { get; set; }
    }
}
