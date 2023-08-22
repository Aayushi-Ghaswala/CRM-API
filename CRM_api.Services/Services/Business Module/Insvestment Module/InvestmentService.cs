using AutoMapper;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Business_Module.Investment_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.Investment_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.Investment_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.IServices.Business_Module.Insvestment_Module;

namespace CRM_api.Services.Services.Business_Module.Insvestment_Module
{
    public class InvestmentService : IInvestmentService
    {
        private readonly IInvestmentRepository _investmentRepository;
        private readonly IMapper _mapper;

        public InvestmentService(IInvestmentRepository investmentRepository, IMapper mapper)
        {
            _investmentRepository = investmentRepository;
            _mapper = mapper;
        }

        #region Get InvestmentType
        public async Task<ResponseDto<InvestmentTypeDto>> GetInvestmentTypeAsync(string? search, SortingParams sortingParams, bool? isActive)
        {
            var result = await _investmentRepository.GetInvestmentType(search, sortingParams, isActive);
            var mappedResult = _mapper.Map<ResponseDto<InvestmentTypeDto>>(result);
            return mappedResult;
        }
        #endregion

        #region Get InvestmentType by id
        public async Task<InvestmentTypeDto> GetInvestmentTypeByIdAsync(int id)
        {
            var result = await _investmentRepository.GetInvestmentTypeById(id);
            var mappedResult = _mapper.Map<InvestmentTypeDto>(result);
            return mappedResult;
        }
        #endregion

        #region Get Sub InvestmentType
        public async Task<ResponseDto<SubInvestmentTypeDto>> GetSubInvestmentTypeAsync(string? search, SortingParams sortingParams, bool? isActive)
        {
            var result = await _investmentRepository.GetSubInvestmentType(search, sortingParams, isActive);
            var mappedResult = _mapper.Map<ResponseDto<SubInvestmentTypeDto>>(result);
            return mappedResult;
        }
        #endregion

        #region Get Sub InvestmentType by InvestmentType id
        public async Task<ResponseDto<SubInvestmentTypeDto>> GetSubInvestmentTypeByInvIdAsync(int invId, string? search, SortingParams sortingParams)
        {
            var result = await _investmentRepository.GetSubInvestmentTypeByInvId(invId, search, sortingParams);
            var mappedResult = _mapper.Map<ResponseDto<SubInvestmentTypeDto>>(result);
            return mappedResult;
        }
        #endregion

        #region Get SubsubInvestmentType
        public async Task<ResponseDto<SubsubInvTypeDto>> GetSubsubInvestmentTypeAsync(string? search, SortingParams sortingParams, bool? isActive)
        {
            var result = await _investmentRepository.GetSubsubInvestmentType(search, sortingParams, isActive);
            var mappedResult = _mapper.Map<ResponseDto<SubsubInvTypeDto>>(result);
            return mappedResult;
        }
        #endregion

        #region Get SubsubInvestmentType by SubInvestment id
        public async Task<ResponseDto<SubsubInvTypeDto>> GetSubsubInvestmentTypeBySubInvIdAsync(int subInvId, string? search, SortingParams sortingParams)
        {
            var result = await _investmentRepository.GetSubsubInvestmentTypeBySubInvId(subInvId, search, sortingParams);
            var mappedResult = _mapper.Map<ResponseDto<SubsubInvTypeDto>>(result);
            return mappedResult;
        }
        #endregion

        #region Add InvestmentType
        public async Task<int> AddInvestmentTypeAsync(AddInvestmentTypeDto addInvestmentTypeDto)
        {
            var mappedModel = _mapper.Map<TblInvesmentType>(addInvestmentTypeDto);
            return await _investmentRepository.AddInvestmentType(mappedModel);
        }
        #endregion

        #region Add SubInvestmentType
        public async Task<int> AddSubInvestmentTypeAsync(AddSubInvestmentTypeDto addSubInvestmentTypeDto)
        {
            var mappedModel = _mapper.Map<TblSubInvesmentType>(addSubInvestmentTypeDto);
            return await _investmentRepository.AddSubInvestmentType(mappedModel);
        }
        #endregion

        #region Add SubsubInvestmentType
        public async Task<int> AddSubsubInvestmentTypeAsync(AddSubsubInvTypeDto addSubsubInvTypeDto)
        {
            var mappedModel = _mapper.Map<TblSubsubInvType>(addSubsubInvTypeDto);
            return await _investmentRepository.AddSubsubInvestmentType(mappedModel);
        }
        #endregion

        #region Update InvestmentType
        public async Task<int> UpdateInvestmentTypeAsync(UpdateInvestmentTypeDto updateInvestmentTypeDto)
        {
            var mappedModel = _mapper.Map<TblInvesmentType>(updateInvestmentTypeDto);
            return await _investmentRepository.UpdateInvestmentType(mappedModel);
        }
        #endregion

        #region Update SubInvestmentType
        public async Task<int> UpdateSubInvestmentTypeAsync(UpdateSubInvestmentTypeDto updateSubInvestmentTypeDto)
        {
            var mappedModel = _mapper.Map<TblSubInvesmentType>(updateSubInvestmentTypeDto);
            return await _investmentRepository.UpdateSubInvestmentType(mappedModel);
        }
        #endregion

        #region Update SubsubInvestmentType
        public async Task<int> UpdateSubsubInvestmentTypeAsync(UpdateSubsubInvTypeDto updateSubsubInvTypeDto)
        {
            var mappedModel = _mapper.Map<TblSubsubInvType>(updateSubsubInvTypeDto);
            return await _investmentRepository.UpdateSubsubInvestmentType(mappedModel);
        }
        #endregion

        #region Deactive InvestmentType
        public async Task<int> DeactiveInvestmentTypeAsync(int id)
        {
            return await _investmentRepository.DeactiveInvestmentType(id);
        }
        #endregion

        #region Deactive SubInvestmentType
        public async Task<int> DeactiveSubInvestmentTypeAsync(int id)
        {
            return await _investmentRepository.DeactiveSubInvestmentType(id);
        }
        #endregion

        #region Deactive SubsubInvestmentType
        public async Task<int> DeactiveSubsubInvestmentTypeAsync(int id)
        {
            return await _investmentRepository.DeactiveSubsubInvestmentType(id);
        }
        #endregion
    }
}
