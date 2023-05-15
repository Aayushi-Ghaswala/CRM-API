using CRM_api.Services.Dtos.AddDataDto;
using CRM_api.Services.Dtos.ResponseDto;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.IServices.User_Module
{
    public interface IUserMasterService
    {
        Task<int> AddUserAsync(AddUserMasterDto addUser);
        Task<int> UpdateUserAsync(UpdateUserMasterDto updateUser);
        Task<ResponseDto<UserMasterDto>> GetUsersAsync(int page, string search, string sortOn);
        Task<UserMasterDto> GetUserMasterById(int id);

        Task<ResponseDto<UserMasterDto>> GetUsersAsync(int page);
        Task<GetUserMasterForUpdateDto> GetUserMasterById(int id);
        Task<ResponseDto<UserCategoryDto>> GetUserCategoriesAsync(int page);
        Task<ResponseDto<UserMasterDto>> GetUsersByCategoryIdAsync(int page, int catId);
        Task<int> AddUserAsync(AddUserMasterDto addUser);
        Task<int> UpdateUserAsync(UpdateUserMasterDto updateUser);
        Task<int> DeactivateUserAsync(int id);
    }
}
