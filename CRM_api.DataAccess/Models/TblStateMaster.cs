using System;
using System.Collections.Generic;

namespace CRM_api.DataAccess.Models
{
    public partial class TblStateMaster
    {
        public TblStateMaster()
        {
            TblCityMasters = new HashSet<TblCityMaster>();
        }

        public int StateId { get; set; }
        public int? CountryId { get; set; }
        public string? StateName { get; set; }
        public bool IsDeleted { get; set; }

        public virtual TblCountryMaster? Country { get; set; }
        public virtual ICollection<TblCityMaster> TblCityMasters { get; set; }
    }
}
