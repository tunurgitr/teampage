using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TeamPage.Web.Models
{
    public class UserClientAssignment
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public int ClientId { get; set; }

        
        public virtual User User { get; set; }
        public virtual Client Client { get; set; }
    }
}
