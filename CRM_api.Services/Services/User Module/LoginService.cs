using AutoMapper;
using CRM_api.DataAccess.IRepositories.User_Module;
using CRM_api.Services.Dtos.ResponseDto;
using CRM_api.Services.Helper.Reminder_Helper;
using CRM_api.Services.IServices.User_Module;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace CRM_api.Services.Services.User_Module
{
    public class LoginService : ILoginService
    {
        private readonly IMemoryCache _cache;
        private readonly IUserMasterRepository _userMasterRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public LoginService(IMemoryCache cache, IUserMasterRepository userMasterRepository, IConfiguration configuration, IMapper mapper)
        {
            _cache = cache;
            _userMasterRepository = userMasterRepository;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<int> GenerateOTPAsync(string email)
        {
            var user = await _userMasterRepository.GetUserByEmail(email);
            if (user is null)
                return 0;

            else
            {
                string[] allowedRandom = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
                var random = new Random();
                var otp = "";
                for (int i = 0; i < 6; i++)
                {
                    var temp = allowedRandom[random.Next(0, allowedRandom.Length)];
                    otp += temp;
                }

                var subject = "Your One-Time Password (OTP)";
                var body = new BodyBuilder();
                body.TextBody = $"Dear {user.UserName},\n\n" +
                           $"Thank you for using our service. As requested, we have generated a One-Time Password (OTP) for you. Please find the OTP details below:\n\n" +
                           $"OTP : {otp}\n\n" +
                           $"Please note that this OTP is valid for a Three minutes and should be used for authentication or verification purposes only. Do not share this OTP with anyone.\n\n" +
                           $"If you did not request this OTP or have any concerns, please disregard this email.\n\n" +
                           $"Thank You\n" +
                           $"KA-Group";
                EmailHelper.SendMailAsync(_configuration, email, subject, body);
                var cacheOptions = new MemoryCacheEntryOptions()
                                        .SetSlidingExpiration(TimeSpan.FromMinutes(3));
                _cache.Set(user.UserEmail, otp, cacheOptions);
                return 1;
            }
        }

        public async Task<(int, UserMasterDto)> VerifyOTPAsync(string email, string otp)
        {
            var value = _cache.Get(email);
            if (value is null)
                return (0, null);
            else if (!value.Equals(otp))
                return (1, null);
            else
            {
                var user = await _userMasterRepository.GetUserByEmail(email);
                var mapUser = _mapper.Map<UserMasterDto>(user);
                return (2, mapUser);
            }
        }
    }
}
