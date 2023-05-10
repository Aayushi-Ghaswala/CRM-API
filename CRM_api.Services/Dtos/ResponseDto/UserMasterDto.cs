namespace CRM_api.Services.Dtos.ResponseDto
{
    public class DisplayUserMasterDto
    {
        public int User_Id { get; set; }
        public DateTime User_Doj { get; set; } 
        public string User_UName { get; set; } = null!;
        public string User_Name { get; set;} = null!;
        public string User_Mobile { get; set; } = null!;
        public string User_Email { get; set; } = null!;
    }
}
