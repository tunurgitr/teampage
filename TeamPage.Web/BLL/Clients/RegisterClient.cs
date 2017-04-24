using System.ComponentModel.DataAnnotations;

namespace TeamPage.Web.BLL.Clients
{
    public class RegisterClient
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(200)]
        public string Url { get; set; }

        [Required]
        [StringLength(50)]
        public string AgencyUniqueCode { get; set; }
        
    }
}