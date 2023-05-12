using CRM_api.Services.Dtos.ResponseDto.User_Module;

namespace CRM_api.Services.Dtos.ResponseDto.Business_Module.Loan_Module
{
    public class LoanMasterDto
    {
        public int Id { get; set; }
        public UserNameDto TblUserMaster { get; set; }
        public UserCategoryDto TblUserCategoryMaster { get; set; }
        public int? BankId { get; set; }
        public decimal? LoanAmount { get; set; }
        public decimal? Emi { get; set; }
        public DateTime? StartDate { get; set; }
        public int? Term { get; set; }
        public string? Frequency { get; set; }
        public DateTime? MaturityDate { get; set; }
        public decimal? RateOfInterest { get; set; }
        public Int64? LoanAccountNo { get; set; }
        public DateTime? Date { get; set; }
        public Boolean IsNotification { get; set; }
        public Boolean IsCompleted { get; set; }
    }
}
