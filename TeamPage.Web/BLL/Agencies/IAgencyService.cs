using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamPage.Web.BLL.Agencies
{
    public interface IAgencyService
    {
        Task<int> SetupAgencyAsync(SetupAgency cmd);
        Task<AgencyVM[]> GetAgenciesAsync();
        Task<AgencyVM> GetAgencyAsync(string id);
    }
}
