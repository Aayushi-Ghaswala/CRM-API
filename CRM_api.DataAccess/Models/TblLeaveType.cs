using System;
using System.Collections.Generic;

namespace CRM_api.DataAccess.Models
{
    public partial class TblLeaveType
    {
        public int LeaveId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? AllowedDay { get; set; }
        public bool? Isdeleted { get; set; }
    }
}
