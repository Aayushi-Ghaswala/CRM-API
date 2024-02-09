using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.ResponseDto.HR_Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_api.Services.Dtos.ResponseDto.Business_Module.MGain_Module
{
    public class MgainRedemptionRequestDto
    {
        public int Id { get; set; }
        public int MgainId { get; set; }
        public int EmployeeId { get; set; }
        public int UserId { get; set; }
        public DateTime? RecordDate { get; set; }
        public decimal? RedemptionAmount { get; set; }
        public decimal? TotalInterest { get; set; }
        public decimal? MaturityAmount { get; set; }
        public decimal? AmountDeducted { get; set; }
        public decimal? First3MonthInterest { get; set; }
        public string? RedemptionDoc { get; set; }
        public int AccountId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsCompleted { get; set; }
        public string? Reason { get; set; }

        public int? NoOfDays { get; set; }
        public decimal? DaysInterest { get; set; }
        public DateTime? RedemptionDate { get; set; }

        public MGainDetailsDto TblMgaindetails { get; set; }
        public EmployeeMasterDto TblEmployeeMaster { get; set; }
        public UserMasterDto TblUserMaster { get; set; }
        public TblAccountMaster TblAccountMaster { get; set; }
    }
}
