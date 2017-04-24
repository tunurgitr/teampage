

using TeamPage.Web.BLL.Agencies;
using TeamPage.Web.BLL.Agencies.Impl;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class DIHelpers
    {
        public static IServiceCollection ConfigureIocForAgencies(this IServiceCollection services)
        {
            return services.AddScoped<IAgencyService, AgencyService>();
        }

    }
}
