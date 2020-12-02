namespace Globomantics.IdentityServer.Data
{
    public class CompanyMember
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string MemberEmail { get; set; }
        public Company Company { get; set; }
    }
}
