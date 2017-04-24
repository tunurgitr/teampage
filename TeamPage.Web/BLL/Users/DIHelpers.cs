
using TeamPage.Web.BLL.Users;
using TeamPage.Web.BLL.Users.Impl;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class DIHelpers
    {
        public static IServiceCollection ConfigureIocForUsers(this IServiceCollection services)
        {
            return services.AddScoped<IUserService, UserService>();
        }

    }
}
