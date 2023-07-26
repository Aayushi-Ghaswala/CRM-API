namespace CRM_api.Services.Dtos.AddDataDto.Account_Module
{
    public class UpdateAccountGroupDto
    {
        public int Id { get; set; }
        public int? RootGrpid { get; set; }
        public int? ParentGrpid { get; set; }
        public string? AccountGrpName { get; set; }
    }
}
