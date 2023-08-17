using AutoMapper;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.WBC_Mall_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto.WBC_Mall_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.WBC_Mall_Module;
using CRM_api.Services.IServices.WBC_Mall_Module;
using Microsoft.AspNetCore.Http;

namespace CRM_api.Services.Services.WBC_Mall_Module
{
    public class MallProductService : IMallProductService
    {
        private readonly IMallProductRepository _mallProductRepository;
        private readonly IMapper _mapper;

        public MallProductService(IMallProductRepository mallProductRepository, IMapper mapper)
        {
            _mallProductRepository = mallProductRepository;
            _mapper = mapper;
        }

        #region Get Mall Products
        public async Task<ResponseDto<MallProductDto>> GetMallProductsAsync(int? catId, string? filterString, string? search, SortingParams sortingParams)
        {
            var products = await _mallProductRepository.GetMallProducts(catId, filterString, search, sortingParams);
            var mappedProducts = _mapper.Map<ResponseDto<MallProductDto>>(products);

            return mappedProducts;
        }
        #endregion

        #region Add Mall Product
        public async Task<int> AddMallProductAsync(AddMallProductDto addMallProductDto)
        {
            var product = _mapper.Map<TblWbcMallProduct>(addMallProductDto);
            var addedProduct = await _mallProductRepository.AddMallProduct(product);

            var directoryPath = Directory.GetCurrentDirectory() + $"\\wwwroot\\Mall-Product-Image\\{addedProduct.ProdId}";
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            addedProduct.ProdImage = SaveFileAsync(directoryPath, addMallProductDto.FormFile);

            foreach (var file in addMallProductDto.FormFiles)
            {
                var filePath = SaveFileAsync(directoryPath, file);
                var prodImages = new TblProductImg(filePath);
                addedProduct.TblProductImgs.Add(prodImages);
            }
            var addedProductList = new List<TblWbcMallProduct>();
            addedProductList.Add(addedProduct);
            return await _mallProductRepository.UpdateMallProduct(addedProductList);
        }
        #endregion

        #region Update Mall Product
        public async Task<int> UpdateMallProductAsync(UpdateMallProductDto updateMallProductDto)
        {
            var product = await _mallProductRepository.GetMallProductById(updateMallProductDto.ProdId);
            if (product is not null)
            {
                var updatedProduct = _mapper.Map<TblWbcMallProduct>(updateMallProductDto);
                updatedProduct.TblProductImgs = product.TblProductImgs;

                var directoryPath = Directory.GetCurrentDirectory() + $"\\wwwroot\\Mall-Product-Image\\{product.ProdId}";
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                if (updateMallProductDto.ProdImage is null && updateMallProductDto.FormFile is not null)
                    updatedProduct.ProdImage = SaveFileAsync(directoryPath, updateMallProductDto.FormFile);

                if (updateMallProductDto.FormFiles is not null && updateMallProductDto.FormFiles.Count() > 0)
                {
                    foreach (var file in updateMallProductDto.FormFiles)
                    {
                        var filePath = SaveFileAsync(directoryPath, file);
                        var savedFile = new TblProductImg(filePath);
                        updatedProduct.TblProductImgs.Add(savedFile);
                    }
                }

                var updateProductList = new List<TblWbcMallProduct>();
                updateProductList.Add(updatedProduct);
                return await _mallProductRepository.UpdateMallProduct(updateProductList);
            }

            return 0;
        }
        #endregion

        #region Delete Product Image
        public async Task<int> DeleteProductImageAsync(int id)
        {
            return await _mallProductRepository.DeleteProductImage(id);
        }
        #endregion

        #region Save File
        private static string SaveFileAsync(string? directoryPath, IFormFile file)
        {
            var filePath = Path.Combine(directoryPath, file.FileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            using (var fileStream = System.IO.File.Create(filePath))
            {
                var stream = file.OpenReadStream();
                fileStream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(fileStream);
            }

            return filePath;
        }
        #endregion
    }
}
