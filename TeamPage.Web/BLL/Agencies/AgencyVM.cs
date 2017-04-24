namespace TeamPage.Web.BLL.Agencies
{
    public class AgencyVM
    {
        public int Id { get; internal set; }
        public string Name { get; internal set; }
        public string Url { get; internal set; }
        public string UniqueCode { get; set; }
        public AgencyUserVM[] Users { get; set; }
        public AgencyClientVM[] Clients { get; internal set; }
    }
}