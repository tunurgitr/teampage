using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamPage.Web.BLL.Agencies
{
    public class AgencyClientVM
    {
        public int ClientId { get; set; }
        public string Name { get; set; }

        public AgencyClientUserVM[] Users { get; set; }
        public string Url { get; internal set; }
    }
}
