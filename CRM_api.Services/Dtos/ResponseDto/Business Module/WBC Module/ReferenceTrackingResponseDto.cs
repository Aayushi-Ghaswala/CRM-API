namespace CRM_api.Services.Dtos.ResponseDto.Business_Module.WBC_Module
{
    public class ReferenceTrackingResponseDto
    {
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? MobileNo { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsFasttrack { get; set; }
    }
}
