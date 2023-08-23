using AutoMapper;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Business_Module.WBC_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.WBC_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.WBC_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.User_Module;
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

        #region Get gold point category
        public async Task<List<GoldPointCategoryDto>> GetPointCategoryAsync()
        {
            var result = await _wbcRepository.GetPointCategory();
            return _mapper.Map<List<GoldPointCategoryDto>>(result);
        }
        #endregion

        #region Get goldpoint user name
        public async Task<ResponseDto<UserNameDto>> GetGPUsernameAsync(string? type, string? searchingParams, SortingParams sortingParams)
        {
            var result = await _wbcRepository.GetGPUsername(type, searchingParams, sortingParams);
            var mappedResult = _mapper.Map<ResponseDto<UserNameDto>>(result);
            return mappedResult;
        }
        #endregion

        #region Get goldpoint type
        public async Task<ResponseDto<WbcTypeDto>> GetGPTypesAsync(int? userId, string? searchingParams, SortingParams sortingParams)
        {
            var result = await _wbcRepository.GetGPTypes(userId, searchingParams, sortingParams);
            var mappedResult = _mapper.Map<ResponseDto<WbcTypeDto>>(result);
            return mappedResult;
        }
        #endregion

        #region Get gold point ledger report
        public async Task<GoldPointResponseDto<GoldPointDto>> GetGPLedgerReportAsync(DateTime? date, int? userId, string? type, int? categoryId, string? searchingParams, SortingParams sortingParams)
        {
            var result = await _wbcRepository.GetGPLedgerReport(date, userId, type, categoryId, searchingParams, sortingParams);
            var mappedResult = _mapper.Map<GoldPointResponseDto<GoldPointDto>>(result);
            return mappedResult;
        }
        #endregion

        #region Get wbc GP of month
        public async Task<ResponseDto<WbcGPResponseDto>> GetGPAsync(string? search, DateTime date, SortingParams sortingParams)
        {
            var res = await _wbcRepository.GetGP(search, date, sortingParams);
            var mappedResponse = _mapper.Map<ResponseDto<WbcGPResponseDto>>(res);
            return mappedResponse;
        }
        #endregion

        #region Release Gold point
        public async Task<(int, string)> ReleaseGPAsync(DateTime date, bool isTruncate)
        {
            return await _wbcRepository.ReleaseGP(date, isTruncate);
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

        #region Get direct refferals list
        public async Task<ResponseDto<ReferenceTrackingResponseDto>> GetDirectReferralsAsync(int userId, string? search, SortingParams sortingParams)
        {
            var result = await _wbcRepository.GetDirectReferrals(userId, search, sortingParams);
            var mappedResult = _mapper.Map<ResponseDto<ReferenceTrackingResponseDto>>(result);
            return mappedResult;
        }
        #endregion

        #region Get reffered by list
        public async Task<List<ReferenceTrackingResponseDto>> GetReferredByListAsync(int? userId)
        {
            var reffUsers = await _wbcRepository.GetReferredByList(userId);
            var mappedReffUsers = _mapper.Map<List<ReferenceTrackingResponseDto>>(reffUsers);
            return mappedReffUsers;
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
