using System;
using System.Collections.Generic;

namespace CRM_api.DataAccess.Models
{
    public partial class TblFaq
    {
        public int Id { get; set; }
        public string Question { get; set; } = null!;
        public string? Answer { get; set; }
        public bool Isactive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
