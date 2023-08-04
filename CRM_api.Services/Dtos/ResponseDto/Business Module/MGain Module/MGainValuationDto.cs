using CRM_api.DataAccess.Models;

namespace CRM_api.Services.Dtos.ResponseDto.Business_Module.MGain_Module
{
    public class MGainValuationDto
    {
        public int MGainId { get; set; }
        public DateTime? Date { get; set; }
        public decimal? MgainInvamt { get; set; }
        public string? MgainType { get; set; }
        public List<MGainPaymentDto> TblMgainPaymentMethods { get; set; }
        public double? Tenure { get; set; }
        public string? Schemename { get; set; }
        public decimal? InterestRate { get; set; }
        public decimal InterestPayout { get; set; } = 0;
        public decimal InterestAccrued { get; set; } = 0;
        public string? MgainBankName { get; set; }
        public double? RemainingLockinPeriod { get; set; }
        public DateTime? AmountUnlockDate { get; set; }
    }
}
