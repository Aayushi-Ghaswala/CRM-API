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
    public class DesignationService : IDesignationService
    {
        private readonly IDesignationRepository _designationRepository;
        private readonly IMapper _mapper;

        public DesignationService(IDesignationRepository designationRepository, IMapper mapper)
        {
            _designationRepository = designationRepository;
            _mapper = mapper;
        }

        #region Get Designations
        public async Task<ResponseDto<DesignationDto>> GetDesignationAsync(string search, SortingParams sortingParams)
        {
            var designations = await _designationRepository.GetDesignation(search, sortingParams);
            var mappedDesignation = _mapper.Map<ResponseDto<DesignationDto>>(designations);
            return mappedDesignation;
        }
        #endregion

        #region Add designation
        public async Task<int> AddDesignationAsync(AddDesignationDto designationMaster)
        {
            var mappedDesignation = _mapper.Map<TblDesignationMaster>(designationMaster);
            mappedDesignation.Isdeleted = false;
            return await _designationRepository.AddDesignation(mappedDesignation);
        }
        #endregion

        #region Update designation
        public async Task<int> UpdateDesignationAsync(UpdateDesignationDto designationMaster)
        {
            var designation = _mapper.Map<TblDesignationMaster>(designationMaster);
            return await _designationRepository.UpdateDesignation(designation);
        }
        #endregion

        #region Deactivate designation
        public async Task<int> DeactivateDesignationAsync(int id)
        {
            return await _designationRepository.DeactivateDesignation(id);
        }
        #endregion
    }
}
