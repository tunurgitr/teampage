namespace TeamPage.Web.BLL.Clients
{
    public class ClientVM
    {
        public int Id { get;  set; }
        public string Name { get;  set; }
        public string Url { get;  set; }

        public ClientUserVM[] Users { get; set; }
        public ClientAgencyVM[] Agencies { get; set; }
    }
}