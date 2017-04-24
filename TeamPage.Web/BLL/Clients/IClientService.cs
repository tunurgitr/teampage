using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamPage.Web.BLL.Clients
{
    public interface IClientService
    {
        Task<int> RegisterClientAsync(RegisterClient cmd);
        Task<ClientVM[]> GetClientsAsync();
    }
}
