namespace CRM_api.Services.Dtos.ResponseDto
{
    public class GetUserMasterForUpdateDto
    {
        public int Id { get; set; }
        public string Category { get; set; } = null!;
        public int SponId { get; set; } 
        public int ParentId { get; set; }
        public string? Name { get; set; }
        public string? Pan { get; set; }
        public DateTime Doj { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public string? Addr { get; set; }
        public string? Pin { get; set; }
        public string Country { get; set; } = null!;
        public string State { get; set; } = null!;
        public string City { get; set; } = null!;
        public string? Uname { get; set; }
        public string? Passwd { get; set; }
        public bool IsActive { get; set; }
        public int PurposeId { get; set; }
        public string? ProfilePhoto { get; set; }
        public string? PromoCode { get; set; }
        public string? SubCategory { get; set; }
        public string? GstNo { get; set; }
        public string? FcmId { get; set; }
        public DateTime FcmLastUpdateDateTime { get; set; }
        public DateTime Dob { get; set; }
        public string? Aadhar { get; set; }
        public int AccountType { get; set; }
        public bool fastTrack { get; set; }
        public bool WbcActive { get; set; }
        public int TotalCountofAddContact { get; set; }
        public string? Deviceid { get; set; }
        public bool TermAndCondition { get; set; }
        public int Family_Id { get; set; }
        public string? NjName { get; set; }
        public DateTime FastTrackActivationDate { get; set; }
    }
}
