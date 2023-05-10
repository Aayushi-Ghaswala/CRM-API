using System;
using System.Collections.Generic;

namespace CRM_api.DataAccess.Models
{
    public partial class TblNotexistuserMftransaction
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Userpan { get; set; }
        public string? Foliono { get; set; }
        public string? Schemename { get; set; }
        public string? Transactiontype { get; set; }
        public DateTime? Date { get; set; }
        public decimal? Noofunit { get; set; }
        public double? Nav { get; set; }
        public decimal? Invamount { get; set; }
        public int? Scripid { get; set; }
        public int? Userid { get; set; }
    }
}
