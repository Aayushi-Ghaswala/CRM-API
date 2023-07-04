using CRM_api.Services.Dtos.ResponseDto;

namespace CRM_api.Services.IServices.User_Module
{
    public interface ILoginService
    {
        Task<int> GenerateOTPAsync(string email);
        Task<(int, UserMasterDto)> VerifyOTPAsync(string email, string otp);
    }
}
