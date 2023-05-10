using System;
using System.Collections.Generic;

namespace CRM_api.DataAccess.Models
{
    public partial class Tbl5paisaResponse
    {
        public int Id { get; set; }
        public string? Token { get; set; }
        public string? TokenType { get; set; }
        public string? State { get; set; }
        public DateTime? RecordDate { get; set; }
    }
}
