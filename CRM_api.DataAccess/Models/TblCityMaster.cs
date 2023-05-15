using System;
using System.Collections.Generic;

namespace CRM_api.DataAccess.Models
{
    public partial class TblCityMaster
    {
        public int CityId { get; set; }
        public int? StateId { get; set; }
        public string? CityName { get; set; }
        public bool IsDeleted { get; set; }

        public virtual TblStateMaster? State { get; set; }
    }
}
