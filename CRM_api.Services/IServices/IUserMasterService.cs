using CRM_api.Services.Dtos.AddDataDto;
using CRM_api.Services.Dtos.ResponseDto;

namespace CRM_api.Services.IServices
{
    public interface IUserMasterService
    {
        Task<int> AddUserAsync(AddUserMasterDto addUser);
        Task<int> UpdateUserAsync(AddUserMasterDto updateUser, int id);
        Task<DisplayUserMasterDto> GetUsersAsync(int page);
        Task<GetUserMasterForUpdateDto> GetUserMasterById(int id);
        Task<List<UserCategoryDto>> GetUserCategoriesAsync();
    }
}
