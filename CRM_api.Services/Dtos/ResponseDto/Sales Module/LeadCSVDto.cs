using CRM_api.Services.Dtos.ResponseDto.User_Module;

namespace CRM_api.Services.Dtos.ResponseDto.Sales_Module
{
    public class LeadCSVDto
    {
        public string? AssignUser { get; set; }
        public string? ReferredUser { get; set; }
        public string? Campaign { get; set; }
        public string? Status { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string? Address { get; set; }
        public string? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string InterestedIn { get; set; }
        public string? Description { get; set; }
        public string CreatedAt { get; set; }
    }
}
