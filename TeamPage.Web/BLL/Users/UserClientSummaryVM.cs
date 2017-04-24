using System.Collections.Generic;

namespace TeamPage.Web.BLL.Users
{
    public class UserClientSummaryVM
    {
        public int ClientId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        // #FUTURE: Add user's role at this client

    }
}