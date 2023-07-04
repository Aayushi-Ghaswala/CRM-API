using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace CRM_api.DataAccess.Models
{
    public class TblUserLeave
    {
        public int Id { get; set; }
        public int? LeaveTypeId { get; set; }
        public int? RequestedBy { get; set; }
        public int? PermittedBy { get; set; }
        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? RequestedDate { get; set; } = DateTime.Now;
        public string? Reason { get; set; }
        public bool IsPermitted { get; set; }
        public string? RejectedReason { get; set; }
        public bool IsDeleted { get; set; } = false;

        [ForeignKey(nameof(PermittedBy))]
        public virtual TblUserMaster PermitBy { get; set; } = null;

        [ForeignKey(nameof(RequestedBy))]
        public virtual TblUserMaster RequestBy { get; set; } = null;

        [ForeignKey(nameof(LeaveTypeId))]
        public virtual TblLeaveType LeaveType { get; set; } = null;
    }
}
