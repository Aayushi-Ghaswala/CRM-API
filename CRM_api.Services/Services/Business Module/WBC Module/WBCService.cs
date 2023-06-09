﻿using AutoMapper;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Business_Module.WBC_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.WBC_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.WBC_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.IServices.Business_Module.WBC_Module;

namespace CRM_api.Services.Services.Business_Module.WBC_Module
{
    public class WBCService : IWBCService
    {
        private readonly IWBCRepository _wbcRepository;
        private readonly IMapper _mapper;

        public WBCService(IWBCRepository wbcRepository, IMapper mapper)
        {
            _wbcRepository = wbcRepository;
            _mapper = mapper;
        }

        #region Get wbc GP of month
        public async Task<ResponseDto<WbcGPResponseDto>> GetGPAsync(string? search, DateTime date, SortingParams sortingParams)
        {
            var res = await _wbcRepository.GetGP(search, date, sortingParams);
            var mappedResponse = _mapper.Map<ResponseDto<WbcGPResponseDto>>(res);
            return mappedResponse;
        }
        #endregion

        #region Release Gold point
        public async Task<int> ReleaseGPAsync(DateTime date)
        {
            return await _wbcRepository.ReleaseGP(date);
        }
        #endregion

        #region Get all Wbc scheme types
        public async Task<ResponseDto<WbcTypeDto>> GetAllWbcSchemeTypesAsync(string? searchingParams, SortingParams sortingParams)
        {
            var response = await _wbcRepository.GetAllWbcSchemeTypes(searchingParams, sortingParams);
            var mappedResponse = _mapper.Map<ResponseDto<WbcTypeDto>>(response);

            foreach(var item in mappedResponse.Values)
            {
                item.WbcType = item.WbcType.ToLower();
            }
            return mappedResponse;
        }
        #endregion

        #region Get all subInvestment types
        public async Task<ResponseDto<SubInvestmentTypeDto>> GetAllSubInvestmentTypesAsync(string? searchingParams, SortingParams sortingParams)
        {
            var response = await _wbcRepository.GetAllSubInvestmentTypes(searchingParams, sortingParams);
            var mappedResponse = _mapper.Map<ResponseDto<SubInvestmentTypeDto>>(response);

            foreach(var item in mappedResponse.Values)
            {
                item.InvestmentType = item.InvestmentType.ToLower();
            }
            return mappedResponse;
        }
        #endregion

        #region Get all subSubInvestment types
        public async Task<ResponseDto<SubSubInvestmentTypeDto>> GetAllSubSubInvestmentTypesAsync(string? searchingParams, SortingParams sortingParams, int? subInvestmentTypeId)
        {
            var response = await _wbcRepository.GetAllSubSubInvestmentTypes(searchingParams, sortingParams, subInvestmentTypeId);
            var mappedResponse = _mapper.Map<ResponseDto<SubSubInvestmentTypeDto>>(response);

            foreach(var item in mappedResponse.Values)
            {
                item.SubInvType = item.SubInvType.ToLower();
            }
            return mappedResponse;
        }
        #endregion

        #region Gel all wbc scheme
        public async Task<ResponseDto<WBCSchemeMasterDto>> GetAllWbcSchemesAsync(string? searchingParams, SortingParams sortingParams)
        {
            var response = await _wbcRepository.GetAllWbcSchemes(searchingParams, sortingParams);
            var mappedResponse = _mapper.Map<ResponseDto<WBCSchemeMasterDto>>(response);
            return mappedResponse;
        }
        #endregion

        #region Add wbc scheme
        public async Task<int> AddWbcSchemeAsync(AddWBCSchemeDto addWBCSchemeDto)
        {
            var schemeModel = _mapper.Map<TblWbcSchemeMaster>(addWBCSchemeDto);
            return await _wbcRepository.AddWbcScheme(schemeModel);
        }
        #endregion

        #region Update wbc scheme
        public async Task<int> UpdateWbcSchemeAsync(UpdateWBCSchemeDto updateWBCSchemeDto)
        {
            var schemeModel = _mapper.Map<TblWbcSchemeMaster>(updateWBCSchemeDto);
            return await _wbcRepository.UpdateWbcScheme(schemeModel);
        }
        #endregion

        #region Delete wbc scheme
        public async Task<int> DeleteWbcSchemeAsync(int id)
        {
            return await _wbcRepository.DeleteWbcScheme(id);
        }
        #endregion
    }
}
