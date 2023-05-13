namespace CRM_api.Services.Dtos.ResponseDto
{
    public class UserMasterDto
    {
        public int UserId { get; set; }
        public DateTime? UserDoj { get; set; }
        public string? UserUName { get; set; } 
        public string? UserName { get; set; } 
        public string? UserMobile { get; set; }
        public string? UserEmail { get; set; }
    }
}
