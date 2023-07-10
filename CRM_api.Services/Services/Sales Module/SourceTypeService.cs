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
    public class SourceTypeService : ISourceTypeService
    {
        private readonly ISourceTypeRepository _sourceTypeRepository;
        private readonly IMapper _mapper;

        public SourceTypeService(ISourceTypeRepository sourceTypeRepository, IMapper mapper)
        {
            _sourceTypeRepository = sourceTypeRepository;
            _mapper = mapper;
        }

        #region Get Source Types
        public async Task<ResponseDto<SourceTypeDto>> GetSourceTypesAsync(string search, SortingParams sortingParams)
        {
            var sourceTypes = await _sourceTypeRepository.GetSourceTypes(search,sortingParams);
            var mapSourceType = _mapper.Map<ResponseDto<SourceTypeDto>>(sourceTypes);
            return mapSourceType;
        }
        #endregion

        #region Get SourceType By Id
        public async Task<SourceTypeDto> GetSourceTypeByIdAsync(int id)
        {
            var sourceType = await _sourceTypeRepository.GetSourceTypeById(id);
            var mappedSourceType = _mapper.Map<SourceTypeDto>(sourceType);
            return mappedSourceType;
        }
        #endregion

        #region Get SourceType By Name
        public async Task<SourceTypeDto> GetSourceTypeByNameAsync(string Name)
        {
            var sourceType = await _sourceTypeRepository.GetSourceTypeByName(Name);
            var mappedSourceType = _mapper.Map<SourceTypeDto>(sourceType);
            return mappedSourceType;
        }
        #endregion

        #region Add SourceType
        public async Task<int> AddSourceTypeAsync(AddSourceTypeDto sourceTypeDto)
        {
            var sourceType = _mapper.Map<TblSourceTypeMaster>(sourceTypeDto);
            return await _sourceTypeRepository.AddSourceType(sourceType);
        }
        #endregion

        #region Update SourceType
        public async Task<int> UpdateSourceTypeAsync(UpdateSourceTypeDto sourceTypeDto)
        {
            var sourceType = _mapper.Map<TblSourceTypeMaster>(sourceTypeDto);
            return await _sourceTypeRepository.UpdateSourceType(sourceType);
        }
        #endregion

        #region Deactivate SourceType
        public async Task<int> DeactivateSourceTypeAsync(int id)
        {
            return await _sourceTypeRepository.DeactivateSourceType(id);
        }
        #endregion
    }
}
