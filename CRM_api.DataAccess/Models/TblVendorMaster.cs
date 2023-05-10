using System;
using System.Collections.Generic;

namespace CRM_api.DataAccess.Models
{
    public partial class TblVendorMaster
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? ImgPath { get; set; }
    }
}
