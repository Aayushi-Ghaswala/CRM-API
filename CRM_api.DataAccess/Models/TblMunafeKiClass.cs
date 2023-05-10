using System;
using System.Collections.Generic;

namespace CRM_api.DataAccess.Models
{
    public partial class TblMunafeKiClass
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string Videolink { get; set; } = null!;
        public bool Isactive { get; set; }
        public DateTime Entrydate { get; set; }
    }
}
