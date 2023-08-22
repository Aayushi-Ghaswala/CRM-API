namespace CRM_api.DataAccess.ResponseModel.Bussiness_Module.WBC_Module
{
    public class ReferenceTrackingResponseModel
    {
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? MobileNo { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsFasttrack { get; set; }

        public ReferenceTrackingResponseModel()
        {
            
        }
        public ReferenceTrackingResponseModel(int userId, string? username, string? mobileNo, bool? isActive, bool? isFasttrack)
        {
            UserId = userId;
            Username = username;
            MobileNo = mobileNo;
            IsActive = isActive;
            IsFasttrack = isFasttrack;
        }
    }
}
