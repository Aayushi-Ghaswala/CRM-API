using AutoMapper;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Business_Module.Fasttrack_Module;
using CRM_api.DataAccess.IRepositories.User_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto;
using CRM_api.Services.Dtos.ResponseDto;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.IServices.User_Module;
using Microsoft.Extensions.Logging;

namespace CRM_api.Services.Services.User_Module
{
    public class UserMasterService : IUserMasterService
    {
        private readonly IUserMasterRepository _userMasterRepository;
        private readonly IFasttrackRepository _fasttrackRepository;
        private readonly IMapper _mapper;
        public UserMasterService(IUserMasterRepository userMasterRepository, IFasttrackRepository fasttrackRepository, IMapper mapper)
        {
            _userMasterRepository = userMasterRepository;
            _fasttrackRepository = fasttrackRepository;
            _mapper = mapper;
        }

        #region Get All Users
        public async Task<ResponseDto<UserMasterDto>> GetUsersAsync(string filterString, string search, SortingParams sortingParams)
        {
            var users = await _userMasterRepository.GetUsers(filterString, search, sortingParams);
            var mapUsers = _mapper.Map<ResponseDto<UserMasterDto>>(users);
            foreach (var user in mapUsers.Values)
            {
                if (user.UserFasttrack == true)
                {
                    var fasttrackCategory = await _fasttrackRepository.GetUserFasttrackCategory(user.UserId);
                    user.UserFasttrackCategory = fasttrackCategory;
                }
                user.UserName = user.UserName.ToLower();
            }
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

        #region Get User Count
        public async Task<Dictionary<string, int>> GetUserCountAsync()
        {
            var users = await _userMasterRepository.GetUserCount();

            return users;
        }
        #endregion

        #region Get All User Category
        public async Task<ResponseDto<UserCategoryDto>> GetUserCategoriesAsync(string search, SortingParams sortingParams)
        {
            var catagories = await _userMasterRepository.GetUserCategories(search, sortingParams);
            var mapCatagories = _mapper.Map<ResponseDto<UserCategoryDto>>(catagories);

            foreach(var catagory in catagories.Values)
            {
                catagory.CatName = catagory.CatName.ToLower();
            }

            return mapCatagories;
        }
        #endregion

        #region Get All User By Category Id
        public async Task<ResponseDto<UserMasterDto>> GetUsersByCategoryIdAsync(int categoryId, string search, SortingParams sortingParams)
        {
            var users = await _userMasterRepository.GetUsersByCategoryId(categoryId, search, sortingParams);
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

        #region Check Pan Or Addhar Exist
        public int PanOrAadharExistAsync(int? id, string? pan, string? aadhar)
        {
            return _userMasterRepository.PanOrAadharExist(id, pan, aadhar);
        }
        #endregion

        #region Add User
        public async Task<int> AddUserAsync(AddUserMasterDto addUser)
        {
            var user = _mapper.Map<TblUserMaster>(addUser);

            var addedUser = await _userMasterRepository.AddUser(user);
            if (addedUser is not null)
            {
                var uname = addUser.UserName.Split(' ');
                switch (uname.Length)
                {
                    case 1:
                        addedUser.UserUname = string.Concat(uname[0], "_", addedUser.UserId);
                        break;
                    case 2:
                        addedUser.UserUname = string.Concat(uname[0], "_", uname[1], "_", addedUser.UserId);
                        break;
                    default:
                        addedUser.UserUname = string.Concat(uname[0], "_", uname[2], "_", addedUser.UserId);
                        break;
                }
                return await _userMasterRepository.UpdateUser(addedUser);
            }
            else
            {
                return 0;
            }
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
