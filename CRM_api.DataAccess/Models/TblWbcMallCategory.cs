using System;
using System.Collections.Generic;

namespace CRM_api.DataAccess.Models
{
    public partial class TblWbcMallCategory
    {
        public int CatId { get; set; }
        public string CatName { get; set; } = null!;
        public bool CatActive { get; set; }
        public string? CatImage { get; set; }
    }
}
