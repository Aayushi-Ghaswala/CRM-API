namespace CRM_api.Services.Dtos.ResponseDto.User_Module
{
    public class UserNameDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string? UserEmail { get; set; }
        public string? UserMobile { get; set; }
    }
}
