using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamPage.Web.Models
{
    public class AgencyClientAssignment
    {
        public int AgencyId { get; set; }
        public int ClientId { get; set; }

        public virtual Agency Agency { get; set; }
        public virtual Client Client { get; set; }
    }
}
