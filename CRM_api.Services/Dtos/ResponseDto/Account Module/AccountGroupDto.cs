namespace CRM_api.Services.Dtos.ResponseDto.Account_Module
{
    public class AccountGroupDto
    {
        public int Id { get; set; }
        public AccountGroupDto? RootGroup { get; set; }
        public AccountGroupDto? ParentGroup { get; set; }
        public string? AccountGrpName { get; set; }
        public bool? Isdeleted { get; set; }
    }
}
