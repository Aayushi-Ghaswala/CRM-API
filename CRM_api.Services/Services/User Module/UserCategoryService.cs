using AutoMapper;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.User_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto.User_Module;
using CRM_api.Services.Dtos.ResponseDto;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.IServices.User_Module;

namespace CRM_api.Services.Services.User_Module
{
    public class UserCategoryService : IUserCategoryService
    {
        private readonly IUserCategoryRepository _userCategoryRepository;
        private readonly IMapper _mapper;

        public UserCategoryService(IUserCategoryRepository userCategoryRepository, IMapper mapper)
        {
            _userCategoryRepository = userCategoryRepository;
            _mapper = mapper;
        }

        #region Get All User Category
        public async Task<ResponseDto<UserCategoryDto>> GetUserCategoriesAsync(string search, SortingParams sortingParams)
        {
            var catagories = await _userCategoryRepository.GetUserCategories(search, sortingParams);
            var mapCatagories = _mapper.Map<ResponseDto<UserCategoryDto>>(catagories);
            return mapCatagories;
        }
        #endregion

        #region Get Category By Name
        public async Task<UserCategoryDto> GetCategoryByNameAsync(string name)
        {
            var cat = await _userCategoryRepository.GetCategoryByName(name);
            var mappedUserCategory = _mapper.Map<UserCategoryDto>(cat);
            return mappedUserCategory;
        }
        #endregion

        #region Add User Category
        public async Task<int> AddUserCategoryAsync(AddUserCategoryDto addUserCategory)
        {
            var addCategory = _mapper.Map<TblUserCategoryMaster>(addUserCategory);
            return await _userCategoryRepository.AddUserCategory(addCategory);
        }
        #endregion

        #region Update User Category
        public async Task<int> UpdateUserCategoryAsync(UpdateUserCategoryDto updateUserCategory)
        {
            var updateCategory = _mapper.Map<TblUserCategoryMaster>(updateUserCategory);
            return await _userCategoryRepository.UpdateUserCategory(updateCategory);
        }
        #endregion

        #region De-Activate User Category
        public async Task<int> DeActivateUserCategoryAsync(int id)
        {
            return await _userCategoryRepository.DeActivateUserCategory(id);
        }
        #endregion
    }
}
