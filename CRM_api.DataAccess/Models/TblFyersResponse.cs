using System;
using System.Collections.Generic;

namespace CRM_api.DataAccess.Models
{
    public partial class TblFyersResponse
    {
        public int Id { get; set; }
        public int? Code { get; set; }
        public string? Message { get; set; }
        public string? Token { get; set; }
        public string? State { get; set; }
        public string? Sattribute { get; set; }
        public string? TokenType { get; set; }
        public DateTime? RecordDate { get; set; }
    }
}
