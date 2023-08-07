using AutoMapper;
using CRM_api.DataAccess.IRepositories.User_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto.User_Module;
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
