using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_api.Services.Dtos.AddDataDto.Business_Module.MGain_Module
{
    public class UpdateMgainRedemptionRequestsDto
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
        public IFormFile? RedemptionDocFile { get; set; }
    }
}
