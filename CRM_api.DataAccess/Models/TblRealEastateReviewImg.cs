using System;
using System.Collections.Generic;

namespace CRM_api.DataAccess.Models
{
    public partial class TblRealEastateReviewImg
    {
        public int Id { get; set; }
        public string Img { get; set; } = null!;
        public int EastateId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool Isdeleted { get; set; }
    }
}
