namespace CRM_api.Services.Dtos.ResponseDto.User_Module
{
    public class FamilyMemberDto
    {
        public int Memberid { get; set; }
        public string? Name { get; set; }
        public string Mobileno { get; set; } = null!;
        public string? Relation { get; set; }
        public UserMasterDto TblUserMaster { get; set; }
        public int? Familyid { get; set; }
        public UserMasterDto RelativeUser { get; set; }
        public bool IsDisable { get; set; }
    }
}
