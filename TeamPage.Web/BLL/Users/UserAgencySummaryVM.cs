namespace TeamPage.Web.BLL.Users
{
    public class UserAgencySummaryVM
    {
        public int AgencyId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string UniqueCode { get; set; }
        
        // #Future: add info on this user's role at the agency
    }
}