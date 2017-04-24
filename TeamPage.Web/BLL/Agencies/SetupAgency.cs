using System.ComponentModel.DataAnnotations;

namespace TeamPage.Web.BLL.Agencies
{
    public class SetupAgency
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        
        [StringLength(200)]
        public string Url { get; set; }

        [Required]
        [StringLength(50)]

        public string UniqueCode { get; set; }
    }
}