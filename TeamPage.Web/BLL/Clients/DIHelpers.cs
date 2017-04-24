using TeamPage.Web.BLL.Clients;
using TeamPage.Web.BLL.Clients.Impl;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class DIHelpers
    {
        public static IServiceCollection ConfigureIocForClients(this IServiceCollection services)
        {
            return services.AddScoped<IClientService, ClientService>();
        }

    }
}
