using CRM_api.Services.IServices.User_Module;
using Microsoft.AspNetCore.Mvc;

namespace CRM_api.Controllers.User_Module
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        #region GenerateOTP
        [HttpGet("GenerateOTP")]
        public async Task<ActionResult> GenerateOTP(string email)
        {
            try
            {
                var flag = await _loginService.GenerateOTPAsync(email);
                return flag != 0 ? Ok(new { Message = "Otp send successfully." }) : BadRequest(new { Message = "Invalid email address." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region VerifyOTP
        [HttpGet("VerifyOTP")]
        public async Task<ActionResult> VerifyOTP(string email, string otp)
        {
            try
            {
                var flag = await _loginService.VerifyOTPAsync(email, otp);
                return flag.Item1 == 0 ? BadRequest(new { Message = "OTP has expired." }) : flag.Item1 == 1 ? BadRequest(new { Message = "Invalid OTP." }) : Ok(new { Message = "Login successfully.", User = flag.Item2 });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
