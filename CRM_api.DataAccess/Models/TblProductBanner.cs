using System;
using System.Collections.Generic;

namespace CRM_api.DataAccess.Models
{
    public partial class TblProductBanner
    {
        public int BannerId { get; set; }
        public string BannerImg { get; set; } = null!;
        public bool BannerIsactive { get; set; }
    }
}
