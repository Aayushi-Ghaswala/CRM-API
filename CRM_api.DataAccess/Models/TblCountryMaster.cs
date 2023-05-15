using System;
using System.Collections.Generic;

namespace CRM_api.DataAccess.Models
{
    public partial class TblCountryMaster
    {
        public TblCountryMaster()
        {
            TblStateMasters = new HashSet<TblStateMaster>();
        }

        public int CountryId { get; set; }
        public string? CountryName { get; set; }
        public string? Isdcode { get; set; }
        public string? Icon { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<TblStateMaster> TblStateMasters { get; set; }
    }
}
