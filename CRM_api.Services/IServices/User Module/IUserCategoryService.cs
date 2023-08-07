using CRM_api.Services.Dtos.AddDataDto.User_Module;

namespace CRM_api.Services.IServices.User_Module
{
    public interface IUserCategoryService
    {
        Task<int> AddUserCategoryAsync(AddUserCategoryDto addUserCategory);
        Task<int> UpdateUserCategoryAsync(UpdateUserCategoryDto updateUserCategory);
        Task<int> DeActivateUserCategoryAsync(int id);
    }
}
