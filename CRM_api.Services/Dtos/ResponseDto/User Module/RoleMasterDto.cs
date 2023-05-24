namespace CRM_api.Services.Dtos.ResponseDto
{
    public class RoleMasterDto
    {
        public int RoleId { get; set; } 
        public string RoleName { get; set; } = null!;
        public bool IsDeleted { get; set; }
    }
}
