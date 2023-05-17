using AutoMapper;
using CRM_api.DataAccess.IRepositories.Business_Module.Stocks_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.Stocks_Module;
using CRM_api.Services.IServices.Business_Module.Stocks_Module;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace CRM_api.Services.Services.Business_Module.Stocks_Module
{
    public class SharekhanStockService : ISharekhanStockService
    {
        private readonly IStocksRepository _stocksRepository;
        private readonly IMapper _mapper;

        public SharekhanStockService(IStocksRepository stocksRepository, IMapper mapper)
        {
            _stocksRepository = stocksRepository;
            _mapper = mapper;
        }

        #region Import trade file for all and/or individual client.
        public async Task<int> ImportTradeFile(IFormFile formFile, int id, bool overrideData)
        {
            CultureInfo culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            culture.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy";
            Thread.CurrentThread.CurrentCulture = culture;

            var filePath = Path.GetTempFileName();
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }
            var directory = Directory.GetCurrentDirectory() + "\\CRM-Document";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            //Delete file if already exists with same name
            if (File.Exists(Path.Combine(directory, formFile.FileName)))
            {
                File.Delete(Path.Combine(directory, formFile.FileName));
            }
            var localFilePath = Path.Combine(directory, formFile.FileName);
            File.Copy(filePath, localFilePath);

            List<AddSharekhanStocksDto> stockDataList = new List<AddSharekhanStocksDto>();
            using (var fs = new StreamReader(localFilePath))
            {
                // to load the records from the file in my List<CsvLine>
                stockDataList = new CsvReader(fs, culture).GetRecords<AddSharekhanStocksDto>().ToList();
            }
            var mappedStockModel = _mapper.Map<List<TblStockData>>(stockDataList);

            //For individual Trade File
            if (id != 0)
                mappedStockModel.ForEach(s => s.Userid = id);

            //To override existing data
            if (overrideData)
            {
                var stockDataIfExists = await _stocksRepository.GetStockDataForSpecificDateRange(mappedStockModel.First().StDate, mappedStockModel.Last().StDate);

                if (stockDataIfExists.Count > 0)
                    await _stocksRepository.DeleteData(stockDataIfExists);
            }

            return await _stocksRepository.AddData(mappedStockModel);
        }
        #endregion
    }
}
