using System;
using System.Collections.Generic;

namespace CRM_api.DataAccess.Models
{
    public partial class TblUserCategoryMaster
    {
        public int CatId { get; set; }
        public string? CatName { get; set; }
        public bool? CatIsactive { get; set; }
    }
}
