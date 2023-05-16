using AutoMapper;
using CRM_api.DataAccess.IRepositories.User_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto;
using CRM_api.Services.Dtos.ResponseDto;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.IServices.User_Module;

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

        #region UpdateUser Detail
        public async Task<int> UpdateUserAsync(UpdateUserMasterDto updateUser)
        {
            var user = _mapper.Map<TblUserMaster>(updateUser);

            return await _userMasterRepository.UpdateUser(user);
        }
        #endregion

        #region Get All UserMaster Details
        public async Task<ResponseDto<UserMasterDto>> GetUsersAsync(int page, string search, string sortOn)
        {
            var users = await _userMasterRepository.GetUsers(page, search, sortOn);
            var mapUsers = _mapper.Map<ResponseDto<UserMasterDto>>(users);

            return mapUsers;
        }
        #endregion

        #region Get UserMaster By Id
        public async Task<UserMasterDto> GetUserMasterById(int id)
        {
            var user = await _userMasterRepository.GetUserMasterbyId(id);
            var mapUser = _mapper.Map<UserMasterDto>(user);

            return mapUser;
        }
        #endregion

        #region Get All User Category
        public async Task<ResponseDto<UserCategoryDto>> GetUserCategoriesAsync(int page)
        {
            var catagories = await _userMasterRepository.GetUserCategories(page);
            var mapCatagories = _mapper.Map<ResponseDto<UserCategoryDto>>(catagories);

            return mapCatagories;
        }
        #endregion

        #region Get All User Master Detail By Category Id
        public async Task<ResponseDto<UserMasterDto>> GetUsersByCategoryIdAsync(int page, int catId, string search, string sortOn)
        {
            var users = await _userMasterRepository.GetUsersByCategoryId(page, catId, search, sortOn);
            var mapUsers = _mapper.Map<ResponseDto<UserMasterDto>>(users);

            return mapUsers;
        }
        #endregion

        #region AddUser Detail
        public async Task<int> AddUserAsync(AddUserMasterDto addUser)
        {
            var user = _mapper.Map<TblUserMaster>(addUser);

            return await _userMasterRepository.AddUser(user);
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
