namespace CRM_api.Services.Dtos.ResponseDto.Account_Module
{
    public class PaymentTypeDto
    {
        public int Id { get; set; }
        public string PaymentName { get; set; } = null!;
        public bool? IsActive { get; set; }
    }
}
