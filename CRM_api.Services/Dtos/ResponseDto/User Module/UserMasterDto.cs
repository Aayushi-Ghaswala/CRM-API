namespace CRM_api.Services.Dtos.ResponseDto
{
    public class UserMasterDto
    {
        public int UserId { get; set; }
        public DateTime UserDoj { get; set; }
        public string UserUName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string UserMobile { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
    }
}
