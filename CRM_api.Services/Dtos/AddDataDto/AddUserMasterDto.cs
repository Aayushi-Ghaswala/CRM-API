namespace CRM_api.Services.Dtos.AddDataDto
{
    public class AddUserMasterDto
    {
        public Nullable<int> Cat_Id { get; set; }
        public Nullable<int> User_SponId { get; set; }
        public Nullable<int> User_ParentId { get; set; }
        public string? User_Name { get; set; }
        public string? User_Pan { get; set; }
        public DateTime User_Doj { get; set; }
        public string? User_Mobile { get; set; }
        public string? User_Email { get; set; }
        public string? User_Addr { get; set; }
        public string? User_Pin { get; set; }
        public Nullable<int> User_CountryId { get; set; }
        public Nullable<int> User_StateId { get; set; }
        public Nullable<int> User_CityId { get; set; }
        public string? User_Uname { get; set; }
        public string? User_Passwd { get; set; }
        public Nullable<bool> User_IsActive { get; set; }
        public string? User_ProfilePhoto { get; set; }
        public string? User_PromoCode { get; set; }
        public string? User_SubCategory { get; set; }
        public string? User_GstNo { get; set; }
        public Nullable<DateTime> User_Dob { get; set; }
        public string? User_Aadhar { get; set; }
        public Nullable<int> User_AccountType { get; set; }
        public Nullable<bool> User_fastTrack { get; set; }
        public Nullable<bool> User_WbcActive { get; set; }
        public string? User_Deviceid { get; set; }
        public Nullable<bool> User_TermAndCondition { get; set; }
        public Nullable<int> Family_Id { get; set; }
        public string? User_NjName { get; set; }
    }
}
