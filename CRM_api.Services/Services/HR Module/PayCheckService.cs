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
    public class PayCheckService : IPayCheckService
    {
        private readonly IPayCheckRepository _payCheckRepository;
        private readonly IMapper _mapper;

        public PayCheckService(IPayCheckRepository payCheckRepository, IMapper mapper)
        {
            _payCheckRepository = payCheckRepository;
            _mapper = mapper;
        }

        #region Get PayCheck
        public async Task<ResponseDto<PayCheckDto>> GetPayCheckAsync(string search, SortingParams sortingParams)
        {
            var payChecks = await _payCheckRepository.GetPayChecks(search, sortingParams);
            var mapPayCheck = _mapper.Map<ResponseDto<PayCheckDto>>(payChecks);
            return mapPayCheck;
        }
        #endregion

        #region Get PayCheck By Id
        public async Task<PayCheckDto> GetPayCheckByIdAsync(int id)
        {
            var payCheck = await _payCheckRepository.GetPayCheckById(id);
            var mappedPayCheck = _mapper.Map<PayCheckDto>(payCheck);
            return mappedPayCheck;
        }
        #endregion

        #region Get PayCheck By Designation
        public async Task<PayCheckDto> GetPayCheckByDesignationAsync(int designationId)
        {
            var payCheck = await _payCheckRepository.GetPayChecksByDesignation(designationId);
            var mappedPayCheck = _mapper.Map<PayCheckDto>(payCheck);
            return mappedPayCheck;
        }
        #endregion

        #region Add payCheck
        public async Task<int> AddPayCheckAsync(AddPayCheckDto payCheckDto)
        {
            var payCheck = _mapper.Map<TblPayCheck>(payCheckDto);
            return await _payCheckRepository.AddPayCheck(payCheck);
        }
        #endregion

        #region Update payCheck
        public async Task<int> UpdatePayCheckAsync(UpdatePayCheckDto payCheckDto)
        {
            var payCheck = _mapper.Map<TblPayCheck>(payCheckDto);
            return await _payCheckRepository.UpdatePayCheck(payCheck);
        }
        #endregion

        #region Deactivate payCheck
        public async Task<int> DeactivatePayCheckAsync(int id)
        {
            return await _payCheckRepository.DeactivatePayCheck(id);
        }
        #endregion
    }
}
