using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.User_Module;
using CRM_api.Services.Dtos.ResponseDto;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.IServices.User_Module
{
    public interface IUserCategoryService
    {
        Task<ResponseDto<UserCategoryDto>> GetUserCategoriesAsync(string search, SortingParams sortingParams);
        Task<UserCategoryDto> GetCategoryByNameAsync(string name);
        Task<int> AddUserCategoryAsync(AddUserCategoryDto addUserCategory);
        Task<int> UpdateUserCategoryAsync(UpdateUserCategoryDto updateUserCategory);
        Task<int> DeActivateUserCategoryAsync(int id);
    }
}
