using System;
using System.Collections.Generic;

namespace CRM_api.DataAccess.Models
{
    public partial class TblOfferMaster
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public int VendorId { get; set; }
        public string? Title { get; set; }
        public string? Img { get; set; }
        public string? Link { get; set; }
        public int? OfferPercentage { get; set; }
        public int? OfferValue { get; set; }
        public DateTime? OfferValidtill { get; set; }
        public bool? Isactive { get; set; }
    }
}
