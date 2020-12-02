using System.Collections.Generic;

namespace Globomantics.Core.Data
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<CompanyMember> Members { get; set; }
    }
}
