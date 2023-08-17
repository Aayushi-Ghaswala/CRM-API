using AutoMapper;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.WBC_Mall_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto.WBC_Mall_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.WBC_Mall_Module;
using CRM_api.Services.IServices.WBC_Mall_Module;

namespace CRM_api.Services.Services.WBC_Mall_Module
{
    public class MallCategoryService : IMallCategoryService
    {
        private readonly IMallCategoryRepository _mallCategoryRepository;
        private readonly IMapper _mapper;

        public MallCategoryService(IMallCategoryRepository mallCategoryRepository, IMapper mapper)
        {
            _mallCategoryRepository = mallCategoryRepository;
            _mapper = mapper;
        }

        #region Get Mall Categories
        public async Task<ResponseDto<MallCategoryDto>> GetMallCategoriesAsync(string? search, SortingParams sortingParams)
        {
            var categories = await _mallCategoryRepository.GetMallCategories(search, sortingParams);
            var mappedCategories = _mapper.Map<ResponseDto<MallCategoryDto>>(categories);

            return mappedCategories;
        }
        #endregion

        #region Add Mall Category
        public async Task<int> AddMallCategoryAsync(AddMallCategoryDto addMallCategoryDto)
        {
            var addCategory = _mapper.Map<TblWbcMallCategory>(addMallCategoryDto);
            var addedCategory = await _mallCategoryRepository.AddMallCategory(addCategory);
            if (addedCategory is not null)
            {
                var directoryPath = Directory.GetCurrentDirectory() + $"\\wwwroot\\Mall-Category-Image\\{addedCategory.CatId}";
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                var filePath = Path.Combine(directoryPath, addMallCategoryDto.FormFile.FileName);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                using (var fileStream = System.IO.File.Create(filePath))
                {
                    var stream = addMallCategoryDto.FormFile.OpenReadStream();
                    fileStream.Seek(0, SeekOrigin.Begin);
                    stream.CopyTo(fileStream);
                }

                addedCategory.CatImage = filePath;

                return await _mallCategoryRepository.UpdateMallCategory(addedCategory);
            }
            return 0;
        }
        #endregion

        #region Update Mall Category
        public async Task<int> UpdateMallCategoryAsync(UpdateMallCategoryDto updateMallCategoryDto)
        {
            var mallCategory = await _mallCategoryRepository.GetMallCategoryById(updateMallCategoryDto.CatId);
            var updateMallCategory = _mapper.Map<TblWbcMallCategory>(updateMallCategoryDto);
            var directoryPath = Directory.GetCurrentDirectory() + $"\\wwwroot\\Mall-Category-Image\\{updateMallCategory.CatId}";

            if (updateMallCategoryDto.FormFile is not null)
            {
                var filePath = Path.Combine(directoryPath, updateMallCategoryDto.FormFile.FileName);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                using (var fileStream = System.IO.File.Create(filePath))
                {
                    var stream = updateMallCategoryDto.FormFile.OpenReadStream();
                    fileStream.Seek(0, SeekOrigin.Begin);
                    stream.CopyTo(fileStream);
                }
                updateMallCategory.CatImage = filePath;
            }

            return await _mallCategoryRepository.UpdateMallCategory(updateMallCategory);
        }
        #endregion

        #region De-Activate Mall Category
        public async Task<int> DeActivateMallCategoryAsync(int id)
        {
            return await _mallCategoryRepository.DeActivateMallCategory(id);
        }
        #endregion
    }
}
