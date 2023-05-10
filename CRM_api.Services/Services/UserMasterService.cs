using AutoMapper;
using CRM_api.DataAccess.IRepositories;
using CRM_api.Services.BuilderMethod;
using CRM_api.Services.Dtos.AddDataDto;
using CRM_api.Services.Dtos.ResponseDto;
using CRM_api.Services.IServices;

namespace CRM_api.Services.Services
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

        #region AddUser Detail
        public async Task<int> AddUserAsync(AddUserMasterDto addUser)
        {
            var fcmId = Guid.NewGuid().ToString();
            var user = UserMasterBuilder.Build(addUser, fcmId);

            return await _userMasterRepository.AddUser(user);
        }
        #endregion

        #region UpdateUser Detail
        public async Task<int> UpdateUserAsync(AddUserMasterDto updateUser, int id)
        {
            var user = await _userMasterRepository.GetUserMasterbyId(id);

            user.UpdateUserMaster(updateUser.Cat_Id, updateUser.User_SponId, updateUser.User_ParentId, updateUser.User_Name
                                    , updateUser.User_Pan, updateUser.User_Doj, updateUser.User_Mobile, updateUser.User_Email
                                    , updateUser.User_Addr, updateUser.User_Pin, updateUser.User_CountryId, updateUser.User_StateId
                                    , updateUser.User_CityId, updateUser.User_Uname, updateUser.User_Passwd, updateUser.User_IsActive
                                    , updateUser.User_PurposeId, updateUser.User_ProfilePhoto, updateUser.User_PromoCode
                                    , updateUser.User_SubCategory, updateUser.User_GstNo, updateUser.User_Dob, updateUser.User_Aadhar
                                    , updateUser.User_AccountType, updateUser.User_fastTrack, updateUser.User_WbcActive
                                    , updateUser.User_Deviceid, updateUser.User_TermAndCondition, updateUser.Family_Id
                                    , updateUser.User_NjName);

            return await _userMasterRepository.UpdateUser(user);
        }
        #endregion

        #region Get All UserMaster Details
        public async Task<IEnumerable<DisplayUserMasterDto>> GetUsersAsync()
        {
            var users = await _userMasterRepository.GetUsers();

            var mapUsers = _mapper.Map<IEnumerable<DisplayUserMasterDto>>(users);

            return mapUsers;
        }
        #endregion

        #region Get UserMaster By Id
        public async Task<GetUserMasterForUpdateDto> GetUserMasterById(int id)
        {
            var user = await _userMasterRepository.GetUserMasterbyId(id);
            var mapUser = _mapper.Map<GetUserMasterForUpdateDto>(user);

            return mapUser;
        }
        #endregion

        #region Get All User Category
        public async Task<List<UserCategoryDto>> GetUserCategoriesAsync()
        {
            var catagories = await _userMasterRepository.GetUserCategories();
            var mapCatagories = _mapper.Map<List<UserCategoryDto>>(catagories);

            return mapCatagories;
        }
        #endregion
    }
}
