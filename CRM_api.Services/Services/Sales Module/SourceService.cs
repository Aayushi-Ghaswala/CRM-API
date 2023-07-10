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
    public class SourceService : ISourceService
    {
        private readonly ISourceRepository _sourceRepository;
        private readonly IMapper _mapper;

        public SourceService(ISourceRepository sourceRepository, IMapper mapper)
        {
            _sourceRepository = sourceRepository;
            _mapper = mapper;
        }

        #region Get Sources
        public async Task<ResponseDto<SourceDto>> GetSourcesAsync(string search, SortingParams sortingParams)
        {
            var sources = await _sourceRepository.GetSources(search,sortingParams);
            var mapSource = _mapper.Map<ResponseDto<SourceDto>>(sources);
            return mapSource;
        }
        #endregion

        #region Get Source By Id
        public async Task<SourceDto> GetSourceByIdAsync(int id)
        {
            var source = await _sourceRepository.GetSourceById(id);
            var mappedSource = _mapper.Map<SourceDto>(source);
            return mappedSource;
        }
        #endregion

        #region Get Source By Name
        public async Task<SourceDto> GetSourceByNameAsync(string Name)
        {
            var source = await _sourceRepository.GetSourceByName(Name);
            var mappedSource = _mapper.Map<SourceDto>(source);
            return mappedSource;
        }
        #endregion

        #region Add Source
        public async Task<int> AddSourceAsync(AddSourceDto sourceDto)
        {
            var source = _mapper.Map<TblSourceMaster>(sourceDto);
            return await _sourceRepository.AddSource(source);
        }
        #endregion

        #region Update Source
        public async Task<int> UpdateSourceAsync(UpdateSourceDto sourceDto)
        {
            var source = _mapper.Map<TblSourceMaster>(sourceDto);
            return await _sourceRepository.UpdateSource(source);
        }
        #endregion

        #region Deactivate Source
        public async Task<int> DeactivateSourceAsync(int id)
        {
            return await _sourceRepository.DeactivateSource(id);
        }
        #endregion
    }
}
