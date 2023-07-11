using AutoMapper;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Sales_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto.Sales_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.Sales_Module;
using CRM_api.Services.IServices.Sales_Module;

namespace CRM_api.Services.Services.Sales_Module
{
    public class StatusService : IStatusService
    {
        private readonly IStatusRepository _statusRepository;
        private readonly IMapper _mapper;

        public StatusService(IStatusRepository statusRepository, IMapper mapper)
        {
            _statusRepository = statusRepository;
            _mapper = mapper;
        }

        #region Get Statues
        public async Task<ResponseDto<StatusDto>> GetStatuesAsync(string search, SortingParams sortingParams)
        {
            var statuss = await _statusRepository.GetStatues(search,sortingParams);
            var mapStatus = _mapper.Map<ResponseDto<StatusDto>>(statuss);
            return mapStatus;
        }
        #endregion

        #region Get Status By Id
        public async Task<StatusDto> GetStatusByIdAsync(int id)
        {
            var status = await _statusRepository.GetStatusById(id);
            var mappedStatus = _mapper.Map<StatusDto>(status);
            return mappedStatus;
        }
        #endregion

        #region Get Status By Name
        public async Task<StatusDto> GetStatusByNameAsync(string Name)
        {
            var status = await _statusRepository.GetStatusByName(Name);
            var mappedStatus = _mapper.Map<StatusDto>(status);
            return mappedStatus;
        }
        #endregion

        #region Add Status
        public async Task<int> AddStatusAsync(AddStatusDto statusDto)
        {
            var status = _mapper.Map<TblStatusMaster>(statusDto);
            return await _statusRepository.AddStatus(status);
        }
        #endregion

        #region Update Status
        public async Task<int> UpdateStatusAsync(UpdateStatusDto statusDto)
        {
            var status = _mapper.Map<TblStatusMaster>(statusDto);
            return await _statusRepository.UpdateStatus(status);
        }
        #endregion

        #region Deactivate Status
        public async Task<int> DeactivateStatusAsync(int id)
        {
            return await _statusRepository.DeactivateStatus(id);
        }
        #endregion
    }
}
