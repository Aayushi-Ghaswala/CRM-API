using AutoMapper;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.HR_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto.HR_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.HR_Module;
using CRM_api.Services.IServices.HR_Module;

namespace CRM_api.Services.Services.HR_Module
{
    public class UserLeaveService : IUserLeaveService
    {
        private readonly IUserLeaveRepository _userLeaveRepository;
        private readonly IMapper _mapper;

        public UserLeaveService(IUserLeaveRepository userLeaveRepository, IMapper mapper)
        {
            _userLeaveRepository = userLeaveRepository;
            _mapper = mapper;
        }

        #region Get UserLeave
        public async Task<ResponseDto<UserLeaveDto>> GetUserLeaveAsync(string search, SortingParams sortingParams)
        {
            var userLeaves = await _userLeaveRepository.GetUserLeaves(search, sortingParams);
            var mapUserLeave = _mapper.Map<ResponseDto<UserLeaveDto>>(userLeaves);
            return mapUserLeave;
        }
        #endregion

        #region Get UserLeave By Id
        public async Task<UserLeaveDto> GetUserLeaveByIdAsync(int id)
        {
            var userLeave = await _userLeaveRepository.GetUserLeaveById(id);
            var mappedUserLeave = _mapper.Map<UserLeaveDto>(userLeave);
            return mappedUserLeave;
        }
        #endregion

        #region Get Leave By User
        public async Task<UserLeaveDto> GetLeaveByUserAsync(int userId)
        {
            var userLeave = await _userLeaveRepository.GetLeavesByUser(userId);
            var mappedUserLeave = _mapper.Map<UserLeaveDto>(userLeave);
            return mappedUserLeave;
        }
        #endregion

        #region Add userLeave
        public async Task<int> AddUserLeaveAsync(AddUserLeaveDto userLeaveDto)
        {
            var userLeave = _mapper.Map<TblUserLeave>(userLeaveDto);
            return await _userLeaveRepository.AddUserLeave(userLeave);
        }
        #endregion

        #region Update userLeave
        public async Task<int> UpdateUserLeaveAsync(UpdateUserLeaveDto userLeaveDto)
        {
            var userLeave = _mapper.Map<TblUserLeave>(userLeaveDto);
            return await _userLeaveRepository.UpdateUserLeave(userLeave);
        }
        #endregion

        #region Deactivate userLeave
        public async Task<int> DeactivateUserLeaveAsync(int id)
        {
            return await _userLeaveRepository.DeactivateUserLeave(id);
        }
        #endregion
    }
}
