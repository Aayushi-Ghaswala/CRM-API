using AutoMapper;
using CRM_api.DataAccess.IRepositories.HR_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto.HR_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.HR_Module;
using CRM_api.Services.IServices.HR_Module;

namespace CRM_api.Services.Services.HR_Module
{
    public class LeaveTypeService : ILeaveTypeService
    {
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IMapper _mapper;

        public LeaveTypeService(ILeaveTypeRepository leaveTypeRepository, IMapper mapper)
        {
            _leaveTypeRepository = leaveTypeRepository;
            _mapper = mapper;
        }

        #region Get LeaveTypes
        public async Task<ResponseDto<LeaveTypeDto>> GetLeaveTypesAsync(int page)
        {
            var leaveTypes = await _leaveTypeRepository.GetLeaveTypes(page);
            var mapLeaveType = _mapper.Map<ResponseDto<LeaveTypeDto>>(leaveTypes);
            return mapLeaveType;
        }
        #endregion

        #region Get LeaveType By Id
        public async Task<LeaveTypeDto> GetLeaveTypeById(int id)
        {
            var leaveType = await _leaveTypeRepository.GetLeaveTypeById(id);
            var mappedLeaveType = _mapper.Map<LeaveTypeDto>(leaveType);
            return mappedLeaveType;
        }
        #endregion

        #region Get LeaveType By Name
        public async Task<LeaveTypeDto> GetLeaveTypeByName(string Name)
        {
            var leaveType = await _leaveTypeRepository.GetLeaveTypeByName(Name);
            var mappedLeaveType = _mapper.Map<LeaveTypeDto>(leaveType);
            return mappedLeaveType;
        }
        #endregion

        #region Add LeaveType
        public async Task<int> AddLeaveTypeAsync(AddLeaveTypeDto leaveTypeDto)
        {
            var leaveType = _mapper.Map<TblLeaveType>(leaveTypeDto);
            return await _leaveTypeRepository.AddLeaveType(leaveType);
        }
        #endregion

        #region Update LeaveType
        public async Task<int> UpdateLeaveTypeAsync(UpdateLeaveTypeDto leaveTypeDto)
        {
            var leaveType = _mapper.Map<TblLeaveType>(leaveTypeDto);
            return await _leaveTypeRepository.UpdateLeaveType(leaveType);
        }
        #endregion

        #region Deactivate LeaveType
        public async Task<int> DeactivateLeaveTypeAsync(int id)
        {
            return await _leaveTypeRepository.DeactivateLeaveType(id);
        }
        #endregion
    }
}
