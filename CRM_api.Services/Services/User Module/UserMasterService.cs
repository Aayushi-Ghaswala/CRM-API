using AutoMapper;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.User_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto;
using CRM_api.Services.Dtos.ResponseDto;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.IServices.User_Module;
using Microsoft.EntityFrameworkCore;

namespace CRM_api.Services.Services.User_Module
{
    public class UserMasterService : IUserMasterService
    {
        private readonly IUserMasterRepository _userMasterRepository;
        private readonly IMapper _mapper;
        public UserMasterService(IUserMasterRepository userMasterRepository, IMapper mapper)
        {
            _userMasterRepository = userMasterRepository;
            _mapper = mapper;
        }

        #region Get All Users
        public async Task<ResponseDto<UserMasterDto>> GetUsersAsync(Dictionary<string, object> searchingParams, SortingParams sortingParams)
        {
            var users = await _userMasterRepository.GetUsers(searchingParams, sortingParams);
            var mapUsers = _mapper.Map<ResponseDto<UserMasterDto>>(users);

            return mapUsers;
        }
        #endregion
        
        #region Get User By Id
        public async Task<UserMasterDto> GetUserMasterByIdAsync(int id)
        {
            var user = await _userMasterRepository.GetUserMasterbyId(id);
            var mapUser = _mapper.Map<UserMasterDto>(user);

            return mapUser;
        }
        #endregion

        #region Get All User Category
        public async Task<ResponseDto<UserCategoryDto>> GetUserCategoriesAsync(Dictionary<string, object> searchingParams, SortingParams sortingParams)
        {
            var catagories = await _userMasterRepository.GetUserCategories(searchingParams, sortingParams);
            var mapCatagories = _mapper.Map<ResponseDto<UserCategoryDto>>(catagories);

            return mapCatagories;
        }
        #endregion

        #region Get All User By Category Id
        public async Task<ResponseDto<UserMasterDto>> GetUsersByCategoryIdAsync(int categoryId, Dictionary<string, object> searchingParams, SortingParams sortingParams)
        {
            var users = await _userMasterRepository.GetUsersByCategoryId(categoryId, searchingParams, sortingParams);
            var mapUsers = _mapper.Map<ResponseDto<UserMasterDto>>(users);

            return mapUsers;
        }
        #endregion

        #region Get Category By Name
        public async Task<TblUserCategoryMaster> GetCategoryByNameAsync(string name)
        {
            var cat = await _userMasterRepository.GetCategoryByName(name);
            return cat;
        }
        #endregion

        #region Add User
        public async Task<int> AddUserAsync(AddUserMasterDto addUser)
        {
            var user = _mapper.Map<TblUserMaster>(addUser);

            return await _userMasterRepository.AddUser(user);
        }
        #endregion

        #region Update User
        public async Task<int> UpdateUserAsync(UpdateUserMasterDto updateUser)
        {
            var user = _mapper.Map<TblUserMaster>(updateUser);

            return await _userMasterRepository.UpdateUser(user);
        }
        #endregion        
        
        #region Deactivate User
        public async Task<int> DeactivateUserAsync(int id)
        {
            return await _userMasterRepository.DeactivateUser(id);
        }
        #endregion
    }
}
