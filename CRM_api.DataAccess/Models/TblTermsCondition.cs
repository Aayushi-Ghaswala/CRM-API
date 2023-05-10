using System;
using System.Collections.Generic;

namespace CRM_api.DataAccess.Models
{
    public partial class TblTermsCondition
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public bool Isactive { get; set; }
        public DateTime Entrydate { get; set; }
    }
}
