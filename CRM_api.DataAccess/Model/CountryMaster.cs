using System.ComponentModel.DataAnnotations;

namespace CRM_api.DataAccess.Model
{
    public class CountryMaster
    {
        public CountryMaster()
        {
            StateMasters = new HashSet<StateMaster>();
        }

        public int Id { get; set; }
        public string? Country_Name { get; set; }
        public string? Isdcode { get; set; }
        public string? Icon { get; set; }

        public virtual ICollection<StateMaster> StateMasters { get; set; }
    }
}
