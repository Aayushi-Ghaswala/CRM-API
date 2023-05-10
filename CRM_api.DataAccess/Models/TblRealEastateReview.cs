using System;
using System.Collections.Generic;

namespace CRM_api.DataAccess.Models
{
    public partial class TblRealEastateReview
    {
        public int Id { get; set; }
        public string? PropertyType { get; set; }
        public string? CarpetArea { get; set; }
        public string? BuiltupArea { get; set; }
        public string? Location { get; set; }
        public string? ProjectName { get; set; }
        public string? CarParking { get; set; }
        public string? EnterFacing { get; set; }
        public decimal? Year { get; set; }
        public decimal? Price { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int UserId { get; set; }

        public virtual TblUserMaster User { get; set; } = null!;
    }
}
