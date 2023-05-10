using System.ComponentModel.DataAnnotations;

namespace CRM_api.DataAccess.Model
{
    public class StateMaster
    {
        public StateMaster()
        {
            CityMasters = new HashSet<CityMaster>();
        }

        public int Id { get; set; }
        public int Country_Id { get; set; }
        public string? State_Name { get; set; }
        public virtual ICollection<CityMaster> CityMasters { get; set; }
        public virtual CountryMaster? CountryMaster { get; set; }
    }
}
