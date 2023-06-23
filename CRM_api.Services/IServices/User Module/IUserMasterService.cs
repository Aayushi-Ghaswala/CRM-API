using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using CRM_api.Services.Dtos.AddDataDto;
using CRM_api.Services.Dtos.ResponseDto;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;

namespace CRM_api.Services.IServices.User_Module
{
    public interface IUserMasterService
    {
        Task<ResponseDto<UserMasterDto>> GetUsersAsync(string filterString, string search, SortingParams sortingParams);
        Task<UserMasterDto> GetUserMasterByIdAsync(int id);
        Task<Dictionary<string, int>> GetUserCountAsync();
        Task<ResponseDto<UserCategoryDto>> GetUserCategoriesAsync(string search, SortingParams sortingParams);
        Task<ResponseDto<UserMasterDto>> GetUsersByCategoryIdAsync(int categoryId, string search, SortingParams sortingParams);
        Task<TblUserCategoryMaster> GetCategoryByNameAsync(string name);
        int PanOrAadharExistAsync(string? pan, string? aadhar);
        Task<int> AddUserAsync(AddUserMasterDto addUser);
        Task<int> UpdateUserAsync(UpdateUserMasterDto updateUser);
        Task<int> DeactivateUserAsync(int id);
    }
}
