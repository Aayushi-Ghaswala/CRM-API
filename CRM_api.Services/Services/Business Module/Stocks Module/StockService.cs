using AutoMapper;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Business_Module.Stocks_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.Stocks_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.Stocks_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.User_Module;
using CRM_api.Services.IServices.Business_Module.Stocks_Module;
using CsvHelper;
using IronXL;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using Excel = Microsoft.Office.Interop.Excel;

namespace CRM_api.Services.Services.Business_Module.Stocks_Module
{
    public class StockService : IStockService
    {
        private readonly IStocksRepository _stocksRepository;
        private readonly IMapper _mapper;

        public StockService(IStocksRepository stocksRepository, IMapper mapper)
        {
            _stocksRepository = stocksRepository;
            _mapper = mapper;
        }

        #region Get stock user's client names
        public async Task<ResponseDto<UserNameDto>> GetStocksUsersNameAsync(string? scriptName, string? firmName, string? searchingParams, SortingParams sortingParams)
        {
            var usernames = await _stocksRepository.GetStocksUsersName(scriptName, firmName, searchingParams, sortingParams);
            var mappedUsernames = _mapper.Map<ResponseDto<UserNameDto>>(usernames);

            foreach (var user in mappedUsernames.Values)
            {
                user.UserName = user.UserName.ToLower();
            }

            return mappedUsernames;
        }
        #endregion

        #region Get all/client wise script names
        public async Task<ResponseDto<ScriptNamesDto>> GetAllScriptNamesAsync(string clientName, string? firmName, string? searchingParams, SortingParams sortingParams)
        {
            var scriptData = await _stocksRepository.GetAllScriptNames(clientName, firmName, searchingParams, sortingParams);
            var mappedScriptData = _mapper.Map<ResponseDto<ScriptNamesDto>>(scriptData);

            foreach (var script in mappedScriptData.Values)
            {
                script.StScripname = script.StScripname.ToLower();
            }
            return mappedScriptData;
        }
        #endregion

        #region Get All or clientwise stocks data
        public async Task<StockResponseDto<StockMasterDto>> GetStockDataAsync(string clientName, DateTime? fromDate, DateTime? toDate, string scriptName, string firmName, string? searchingParams, SortingParams sortingParams)
        {
            var stocksData = await _stocksRepository.GetStocksTransactions(clientName, fromDate, toDate, scriptName, firmName, searchingParams, sortingParams);
            var stockResult = _mapper.Map<StockResponseDto<StockMasterDto>>(stocksData);
            return stockResult;
        }
        #endregion

        #region Get clientwise script summary
        public async Task<StockSummaryDto<ScripwiseSummaryDto>> GetClientwiseScripSummaryAsync(string? userName, bool? isZero, DateTime? startDate, DateTime? endDate, string? searchingParams, SortingParams sortingParams)
        {
            var stockData = await _stocksRepository.GetStockDataByUserName(userName, startDate, endDate);
            var scripwiseStocks = stockData.GroupBy(x => x.StScripname).ToList();
            double? pageCount = 0;

            List<ScripwiseSummaryDto> scripwiseSummaries = new List<ScripwiseSummaryDto>();

            foreach (var scripwiseStock in scripwiseStocks)
            {
                ScripwiseSummaryDto scripwiseSummary = new ScripwiseSummaryDto();

                scripwiseSummary.StScripname = scripwiseStock.Key;

                scripwiseSummary.TotalBuyQuantity = scripwiseStock.Where(x => x.StType.Equals("B")).Sum(x => x.StQty);
                scripwiseSummary.TotalSellQuantity = scripwiseStock.Where(x => x.StType.Equals("S")).Sum(x => x.StQty);
                scripwiseSummary.TotalAvailableQuantity = scripwiseSummary.TotalBuyQuantity - scripwiseSummary.TotalSellQuantity;

                scripwiseSummary.NetCostValue = Math.Round((decimal)scripwiseStock.Average(x => x.StNetsharerate), 2);
                scripwiseSummary.TotalCurrentValue = Math.Round((decimal)(scripwiseSummary.TotalAvailableQuantity * scripwiseSummary.NetCostValue), 2);

                scripwiseSummaries.Add(scripwiseSummary);
            }

            if(isZero is true)
                scripwiseSummaries = scripwiseSummaries.Where(x => x.TotalAvailableQuantity == 0).ToList();
            else
                scripwiseSummaries = scripwiseSummaries.Where(x => x.TotalAvailableQuantity != 0).ToList();

            IQueryable<ScripwiseSummaryDto> scriptwiseSummaryDto = scripwiseSummaries.AsQueryable();

            var balanceQuantity = scripwiseSummaries.Sum(x => x.TotalAvailableQuantity);
            var totalAmount = scripwiseSummaries.Sum(x => x.TotalCurrentValue);

            pageCount = Math.Ceiling(scripwiseSummaries.Count() / sortingParams.PageSize);

            if(searchingParams != null)
            {
                scriptwiseSummaryDto = scriptwiseSummaryDto.Where(x => x.StScripname.ToLower().Contains(searchingParams.ToLower()) || x.TotalBuyQuantity.ToString().Contains(searchingParams) || x.TotalSellQuantity.ToString().Contains(searchingParams) || x.TotalAvailableQuantity.ToString().Contains(searchingParams) || x.NetCostValue.ToString().Contains(searchingParams) || x.TotalCurrentValue.ToString().Contains(searchingParams));
            }

            //Apply Sorting 
            var sortedData = SortingExtensions.ApplySorting(scriptwiseSummaryDto, sortingParams.SortBy, sortingParams.IsSortAscending);

            //Apply Pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var stockSummary = new ResponseDto<ScripwiseSummaryDto>()
            {
                Values = paginatedData,
                Pagination = new PaginationDto()
                {
                    Count = (int)pageCount,
                    CurrentPage = sortingParams.PageNumber
                }
            };

            var stockSummaryResponse = new StockSummaryDto<ScripwiseSummaryDto>()
            {
                response = stockSummary,
                totalBalanceQuantity = balanceQuantity,
                totalAmount = totalAmount
            };

            return stockSummaryResponse;
        }
        #endregion

        #region Get All clientwise summary
        public async Task<StockSummaryDto<ScripwiseSummaryDto>> GetAllClientwiseStockSummaryAsync(bool? isZero, DateTime? startDate, DateTime? endDate, string? searchingParams, SortingParams sortingParams)
        {
            var stockData = await _stocksRepository.GetStockDataForSpecificDateRange(startDate, endDate, null);
            var clientwiseStocks = stockData.GroupBy(x => x.StClientname).ToList();
            double? pageCount = 0;

            List<ScripwiseSummaryDto> clientwiseSummaries = new List<ScripwiseSummaryDto>();

            foreach (var clientwiseStock in clientwiseStocks)
            {
                ScripwiseSummaryDto clientwiseSummary = new ScripwiseSummaryDto();

                clientwiseSummary.StClientname = clientwiseStock.Key;

                clientwiseSummary.TotalBuyQuantity = clientwiseStock.Where(x => x.StType.Equals("B")).Sum(x => x.StQty);
                clientwiseSummary.TotalSellQuantity = clientwiseStock.Where(x => x.StType.Equals("S")).Sum(x => x.StQty);
                clientwiseSummary.TotalAvailableQuantity = clientwiseSummary.TotalBuyQuantity - clientwiseSummary.TotalSellQuantity;

                clientwiseSummary.NetCostValue = Math.Round((decimal)clientwiseStock.Average(x => x.StNetsharerate), 2);
                clientwiseSummary.TotalCurrentValue = Math.Round((decimal)(clientwiseSummary.TotalAvailableQuantity * clientwiseSummary.NetCostValue), 2);

                clientwiseSummaries.Add(clientwiseSummary);
            }

            if (isZero is true)
                clientwiseSummaries = clientwiseSummaries.Where(x => x.TotalAvailableQuantity == 0).ToList();
            else
                clientwiseSummaries = clientwiseSummaries.Where(x => x.TotalAvailableQuantity != 0).ToList();

            IQueryable<ScripwiseSummaryDto> clientwiseSummaryDto = clientwiseSummaries.AsQueryable();

            var balanceQuantity = clientwiseSummaries.Sum(x => x.TotalAvailableQuantity);
            var totalAmount = clientwiseSummaries.Sum(x => x.TotalCurrentValue);

            pageCount = Math.Ceiling(clientwiseSummaries.Count() / sortingParams.PageSize);

            if (searchingParams != null)
            {
                clientwiseSummaryDto = clientwiseSummaryDto.Where(x => x.StClientname.ToLower().Contains(searchingParams.ToLower()) || x.TotalBuyQuantity.ToString().Contains(searchingParams) || x.TotalSellQuantity.ToString().Contains(searchingParams) || x.TotalAvailableQuantity.ToString().Contains(searchingParams) || x.NetCostValue.ToString().Contains(searchingParams) || x.TotalCurrentValue.ToString().Contains(searchingParams));
            }

            //Apply Sorting 
            var sortedData = SortingExtensions.ApplySorting(clientwiseSummaryDto, sortingParams.SortBy, sortingParams.IsSortAscending);

            //Apply Pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortedData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var stockSummary = new ResponseDto<ScripwiseSummaryDto>()
            {
                Values = paginatedData,
                Pagination = new PaginationDto()
                {
                    Count = (int)pageCount,
                    CurrentPage = sortingParams.PageNumber
                }
            };

            var stockSummaryResponse = new StockSummaryDto<ScripwiseSummaryDto>()
            {
                response = stockSummary,
                totalBalanceQuantity = balanceQuantity,
                totalAmount = totalAmount
            };

            return stockSummaryResponse;
        }
        #endregion

        #region Import Sharekhan trade file for all and/or individual client.
        public async Task<int> ImportSharekhanTradeFileAsync(IFormFile formFile, string firmName, int id, bool overrideData)
        {
            try
            {
                CultureInfo culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                culture.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy";
                Thread.CurrentThread.CurrentCulture = culture;

                var filePath = Path.GetTempFileName();
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }
                var directory = Directory.GetCurrentDirectory() + "\\wwwroot" + "\\CRM-Document\\Sharekhan";
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
                //mappedStockModel.Reverse();

                //For individual Trade File
                if (id != 0)
                    mappedStockModel.ForEach(s => s.Userid = id);

                mappedStockModel.ForEach(st => st.FirmName = firmName);

                //To override existing data
                if (overrideData)
                {
                    var stockDataIfExists = await _stocksRepository.GetStockDataForSpecificDateRange(mappedStockModel.First().StDate, mappedStockModel.Last().StDate, firmName);

                    if (stockDataIfExists.Count > 0)
                        await _stocksRepository.DeleteData(stockDataIfExists);
                }

                return await _stocksRepository.AddData(mappedStockModel);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        #endregion

        #region Import Jainam trade file for individual client.
        public async Task<int> ImportJainamTradeFileAsync(IFormFile formFile, string firmName, string clientName, bool overrideData)
        {
            try
            {
                var filename = "";
                var xlsxFilePath = "";
                var listStocks = new List<AddJainamStocksDto>();
                var filePath = Path.GetTempFileName();

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                    await stream.DisposeAsync();
                }

                var directory = Directory.GetCurrentDirectory() + "\\wwwroot" + "\\CRM-Document\\Jainam";
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var ex = Path.GetExtension(formFile.FileName);

                //Delete file if already exists with same name
                // For .xls file
                var tmpFilePath = Path.Combine(directory, formFile.FileName);


                if (File.Exists(tmpFilePath))
                {
                    GC.Collect();
                    File.Delete(tmpFilePath);
                }

                // For .xlsx file
                var name = formFile.FileName.Split(".");
                filename = name[0] + ".xlsx";
                if (File.Exists(Path.Combine(directory, filename)))
                {
                    File.Delete(Path.Combine(directory, filename));
                }

                // saving .xls file into folder
                var localFilePath = Path.Combine(directory, formFile.FileName);
                File.Copy(filePath, localFilePath);

                if (ex.Equals(".xls"))
                {
                    // Create an Excel Application object
                    Excel.Application excelApp = new Excel.Application();

                    //Open the XLS file
                    Excel.Workbook workbooks = excelApp.Workbooks.Open(localFilePath);
                    try
                    {
                        // Save the workbook as XLSX format
                        name = formFile.FileName.Split(".");
                        filename = name[0] + ".xlsx";
                        xlsxFilePath = Path.Combine(directory, filename);
                        workbooks.SaveAs(xlsxFilePath, Excel.XlFileFormat.xlOpenXMLWorkbook);
                    }
                    catch (Exception exe)
                    {
                        // Handle any exceptions that occur during the conversion process
                        Console.WriteLine("Error saving XLSX file: " + exe.Message);
                    }
                    finally
                    {
                        // Close the workbook and Excel application
                        workbooks.Close();
                        excelApp.Quit();

                        // Release the COM objects to avoid memory leaks
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(workbooks);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                    }
                }

                if (File.Exists(Path.Combine(directory, formFile.FileName)))
                {
                    File.Delete(Path.Combine(directory, formFile.FileName));
                }
                //localFilePath = localFilePath.Replace("\\", "/");

                WorkBook workbook = WorkBook.LoadExcel(xlsxFilePath);
                var worksheet = workbook.WorkSheets[0];
                var scriptName = "";
                //starts reading data from 4th row of excel sheet
                for (var i = 3; i < worksheet.RowCount; i++)
                {
                    var col_value = worksheet.Rows[i].Columns[0].Value.ToString();
                    if (String.IsNullOrEmpty(col_value))
                        continue;
                    else if (col_value.StartsWith("Scrip"))
                    {
                        scriptName = col_value.Substring(8);
                    }
                    else
                    {
                        var trans = new AddJainamStocksDto
                        {
                            ScriptName = scriptName,
                            Company = worksheet.Rows[i].Columns[0].Value.ToString(),
                            Date = Convert.ToDateTime(worksheet.Rows[i].Columns[1].Value.ToString()),
                            Narration = worksheet.Rows[i].Columns[2].Value.ToString(),
                            BuyQty = Convert.ToInt32(worksheet.Rows[i].Columns[3].Value),
                            BuyNetRate = Convert.ToDouble(worksheet.Rows[i].Columns[4].Value.ToString()),
                            SellQty = Convert.ToInt32(worksheet.Rows[i].Columns[5].Value),
                            SellNetRate = Convert.ToDouble(worksheet.Rows[i].Columns[6].Value.ToString()),
                            NetRate = Convert.ToDouble(worksheet.Rows[i].Columns[8].Value.ToString()),
                            NetAmount = Math.Abs(Convert.ToDecimal(worksheet.Rows[i].Columns[9].Value.ToString()))
                        };
                        listStocks.Insert(0, trans);
                    }
                }
                //i = 25
                if (listStocks.Count > 0)
                {
                    for (int i = 0; i < listStocks.Count(); i++)
                    {
                        if (listStocks[i].BuyQty > 0 && listStocks[i].SellQty > 0)
                        {
                            var sellQty = listStocks[i].SellQty;
                            var sellNetRate = listStocks[i].SellNetRate;
                            listStocks[i].SellQty = 0;
                            listStocks[i].SellNetRate = 0;

                            var stock = new AddJainamStocksDto(listStocks[i]);
                            stock.BuyQty = 0;
                            stock.BuyNetRate = 0;
                            stock.SellQty = sellQty;
                            stock.SellNetRate = sellNetRate;

                            listStocks.Insert(i + 1, stock);
                        }
                    }

                    //Mapping 
                    var mappedStockModel = _mapper.Map<List<TblStockData>>(listStocks);
                    mappedStockModel.ForEach(s =>
                    {
                        s.StClientname = clientName;
                        s.FirmName = firmName;
                    });

                    if (overrideData)
                    {
                        var listOfDates = new List<DateTime>();
                        listStocks.ForEach(s => listOfDates.Add(s.Date));
                        var stockDataIfExists = await _stocksRepository.GetStockDataForSpecificDateRange(listOfDates.Min(), listOfDates.Max(), firmName);

                        if (stockDataIfExists.Count > 0)
                            await _stocksRepository.DeleteData(stockDataIfExists);
                    }
                    //Add Data
                    return await _stocksRepository.AddData(mappedStockModel);
                }
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        #endregion
    }
}
