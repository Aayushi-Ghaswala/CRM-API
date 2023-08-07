using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto;
using CRM_api.Services.Dtos.ResponseDto;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.User_Module;

namespace CRM_api.Services.IServices.User_Module
{
    public interface IUserMasterService
    {
        Task<ResponseDto<UserMasterDto>> GetUsersAsync(string filterString, string search, SortingParams sortingParams);
        Task<UserMasterDto> GetUserMasterByIdAsync(int id);
        Task<Dictionary<string, int>> GetUserCountAsync();
        Task<List<UserNameDto>> GetUsersByCategoryIdAsync(int categoryId, string search, SortingParams sortingParams);
        int PanOrAadharExistAsync(int? id, string? pan, string? aadhar);
        Task<byte[]> GetUsersForCSVAsync(string filterString, string search, SortingParams sortingParams);
        Task<int> AddUserAsync(AddUserMasterDto addUser);
        Task<int> UpdateUserAsync(UpdateUserMasterDto updateUser);
        Task<int> DeactivateUserAsync(int id);
    }
}
