using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto;

namespace CRM_api.Services.BuilderMethod
{
    public class UserMasterBuilder
    {
        public static TblUserMaster Build(AddUserMasterDto addUser, string fcmId)
        {
            return new TblUserMaster(addUser.Cat_Id, addUser.User_SponId, addUser.User_ParentId, addUser.User_Name, addUser.User_Pan
                                    , addUser.User_Doj, addUser.User_Mobile, addUser.User_Email, addUser.User_Addr, addUser.User_Pin
                                    , addUser.User_CountryId, addUser.User_StateId, addUser.User_CityId, addUser.User_Uname
                                    , addUser.User_Passwd, addUser.User_IsActive, addUser.User_ProfilePhoto
                                    , addUser.User_PromoCode, addUser.User_SubCategory, addUser.User_GstNo, fcmId, addUser.User_Dob
                                    , addUser.User_Aadhar, addUser.User_AccountType, addUser.User_fastTrack, addUser.User_WbcActive
                                    , addUser.User_Deviceid, addUser.User_TermAndCondition, addUser.Family_Id, addUser.User_NjName);
        }
    }
}
