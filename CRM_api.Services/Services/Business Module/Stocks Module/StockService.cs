﻿using AutoMapper;
using AutoMapper.Configuration.Annotations;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Business_Module.Stocks_Module;
using CRM_api.DataAccess.IRepositories.User_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.Stocks_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.Stocks_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.User_Module;
using CRM_api.Services.IServices.Business_Module.Stocks_Module;
using CsvHelper;
using IronXL;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.IO.Compression;
using Excel = Microsoft.Office.Interop.Excel;

namespace CRM_api.Services.Services.Business_Module.Stocks_Module
{
    public class StockService : IStockService
    {
        private readonly IStocksRepository _stocksRepository;
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IUserMasterRepository _userMasterRepository;
        private readonly IStocksDashboardService _stocksDashboardService;

        public StockService(IStocksRepository stocksRepository, IMapper mapper, IHttpClientFactory httpClientFactory, IUserMasterRepository userMasterRepository, IStocksDashboardService stocksDashboardService)
        {
            _stocksRepository = stocksRepository;
            _mapper = mapper;
            _httpClientFactory = httpClientFactory;
            _userMasterRepository = userMasterRepository;
            _stocksDashboardService = stocksDashboardService;
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
        public async Task<StockResponseDto<StockMasterDto>> GetStockDataAsync(string clientName, DateTime? fromDate, DateTime? toDate, string scriptName, string firmName, string? fileType, string? searchingParams, SortingParams sortingParams)
        {
            var stocksData = await _stocksRepository.GetStocksTransactions(clientName, fromDate, toDate, scriptName, firmName, fileType, searchingParams, sortingParams);

            var stockResult = _mapper.Map<StockResponseDto<StockMasterDto>>(stocksData);
            return stockResult;
        }
        #endregion

        #region Get clientwise script summary
        public async Task<StockSummaryDto<ScripwiseSummaryDto>> GetClientwiseScripSummaryAsync(string? userName, bool? isZero, DateTime? startDate, DateTime? endDate, string? searchingParams, SortingParams sortingParams)
        {
            var stockData = await _stocksRepository.GetStockDataByUserName(userName, startDate, endDate);
            var scripwiseStocks = stockData.GroupBy(x => x.StScripname).ToList();
            var scrips = await _stocksRepository.GetAllScrip();
            double? pageCount = 0;

            List<ScripwiseSummaryDto> scripwiseSummaries = new List<ScripwiseSummaryDto>();

            foreach (var scripwiseStock in scripwiseStocks)
            {
                ScripwiseSummaryDto scripwiseSummary = new ScripwiseSummaryDto();

                scripwiseSummary.StScripname = scripwiseStock.Key;

                scripwiseSummary.TotalBuyQuantity = scripwiseStock.Where(x => x.StType.Equals("B")).Sum(x => x.StQty);
                scripwiseSummary.TotalSellQuantity = scripwiseStock.Where(x => x.StType.Equals("S")).Sum(x => x.StQty);
                scripwiseSummary.TotalAvailableQuantity = scripwiseSummary.TotalBuyQuantity - scripwiseSummary.TotalSellQuantity;

                var scrip = scrips.Where(x => x.Scripname != null && x.Scripname.ToLower().Equals(scripwiseStock.Key.ToLower())).FirstOrDefault();
                if (scrip is not null)
                    scripwiseSummary.NetShareRate = scrip.Ltp;
                else
                    scripwiseSummary.NetShareRate = Math.Round((decimal)scripwiseStock.Last().StNetsharerate, 2);

                scripwiseSummary.TotalCurrentValue = Math.Round((decimal)(scripwiseSummary.TotalAvailableQuantity * scripwiseSummary.NetShareRate), 2);

                scripwiseSummaries.Add(scripwiseSummary);
            }

            if (isZero is true)
                scripwiseSummaries = scripwiseSummaries.Where(x => x.TotalAvailableQuantity == 0).ToList();
            else
                scripwiseSummaries = scripwiseSummaries.Where(x => x.TotalAvailableQuantity != 0).ToList();

            IQueryable<ScripwiseSummaryDto> scriptwiseSummaryDto = scripwiseSummaries.AsQueryable();

            var balanceQuantity = scripwiseSummaries.Sum(x => x.TotalAvailableQuantity);
            var totalAmount = scripwiseSummaries.Sum(x => x.TotalCurrentValue);

            pageCount = Math.Ceiling(scripwiseSummaries.Count() / sortingParams.PageSize);

            if (searchingParams != null)
            {
                scriptwiseSummaryDto = scriptwiseSummaryDto.Where(x => x.StScripname.ToLower().Contains(searchingParams.ToLower()) || x.TotalBuyQuantity.ToString().Contains(searchingParams) || x.TotalSellQuantity.ToString().Contains(searchingParams) || x.TotalAvailableQuantity.ToString().Contains(searchingParams) || x.NetShareRate.ToString().Contains(searchingParams) || x.TotalCurrentValue.ToString().Contains(searchingParams));
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
            var stockData = await _stocksRepository.GetStockDataForSpecificDateRange(startDate, endDate);
            var clientwiseStocks = stockData.GroupBy(x => x.StClientname).ToList();
            var scrips = await _stocksRepository.GetAllScrip();
            double? pageCount = 0;

            List<ScripwiseSummaryDto> clientwiseSummaries = new List<ScripwiseSummaryDto>();

            foreach (var clientwiseStock in clientwiseStocks)
            {
                ScripwiseSummaryDto clientwiseSummary = new ScripwiseSummaryDto();

                clientwiseSummary.StClientname = clientwiseStock.Key;

                clientwiseSummary.TotalBuyQuantity = clientwiseStock.Where(x => x.StType.Equals("B")).Sum(x => x.StQty);
                clientwiseSummary.TotalSellQuantity = clientwiseStock.Where(x => x.StType.Equals("S")).Sum(x => x.StQty);

                var clientwiseScripwiseStocks = clientwiseStock.GroupBy(x => x.StScripname);
                decimal? ncv = 0;

                foreach (var clientwiseScripwiseStock in clientwiseScripwiseStocks)
                {
                    var scrip = scrips.Where(x => x.Scripname != null && x.Scripname.ToLower().Equals(clientwiseScripwiseStock.Key.ToLower())).FirstOrDefault();
                    if (scrip is not null)
                        clientwiseSummary.NetShareRate = scrip.Ltp;
                    else
                        clientwiseSummary.NetShareRate = Math.Round((decimal)clientwiseScripwiseStock.Last().StNetsharerate, 2);

                    var buyQty = clientwiseScripwiseStock.Where(x => x.StType.Equals("B")).Sum(x => x.StQty);
                    var sellQty = clientwiseScripwiseStock.Where(x => x.StType.Equals("S")).Sum(x => x.StQty);
                    clientwiseSummary.TotalAvailableQuantity = buyQty - sellQty;
                    clientwiseSummary.TotalCurrentValue += Math.Round((decimal)(clientwiseSummary.TotalAvailableQuantity * clientwiseSummary.NetShareRate), 2);

                    ncv += clientwiseSummary.NetShareRate;
                }

                clientwiseSummary.NetShareRate = Math.Round((decimal)(ncv / clientwiseScripwiseStocks.Count()), 2);
                clientwiseSummary.TotalAvailableQuantity = clientwiseSummary.TotalBuyQuantity - clientwiseSummary.TotalSellQuantity;
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
                clientwiseSummaryDto = clientwiseSummaryDto.Where(x => x.StClientname.ToLower().Contains(searchingParams.ToLower()) || x.TotalBuyQuantity.ToString().Contains(searchingParams) || x.TotalSellQuantity.ToString().Contains(searchingParams) || x.TotalAvailableQuantity.ToString().Contains(searchingParams) || x.NetShareRate.ToString().Contains(searchingParams) || x.TotalCurrentValue.ToString().Contains(searchingParams));
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

        #region Get All Scrip data for listing
        public async Task<ResponseDto<ScripMasterDto>> GetAllScripDataAsync(string? exchange, string? search, SortingParams sortingParams)
        {
            var scripData = await _stocksRepository.GetAllScripData(exchange, search, sortingParams);
            var mappedScripData = _mapper.Map<ResponseDto<ScripMasterDto>>(scripData);
            return mappedScripData;
        }
        #endregion

        #region Import Sherkhan trade file for all and/or individual client.
        public async Task<int> ImportSherkhanTradeFileAsync(IFormFile formFile, string firmName, int id, bool overrideData)
        {
            try
            {
                var fileType = "EQ";
                CultureInfo culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                culture.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy";
                Thread.CurrentThread.CurrentCulture = culture;

                var filePath = Path.GetTempFileName();
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }
                var directory = Directory.GetCurrentDirectory() + "\\wwwroot" + "\\CRM-Document\\Sherkhan";
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

                List<AddSherkhanStocksDto> stockDataList = new List<AddSherkhanStocksDto>();

                using (var fs = new StreamReader(localFilePath))
                {
                    // to load the records from the file in my List<CsvLine>
                    stockDataList = new CsvReader(fs, culture).GetRecords<AddSherkhanStocksDto>().ToList();
                }
                var mappedStockModel = _mapper.Map<List<TblStockData>>(stockDataList);
                //mappedStockModel.Reverse();

                //For individual Trade File
                if (id != 0)
                    mappedStockModel.ForEach(s => s.Userid = id);

                var scrips = await _stocksRepository.GetAllScrip();

                foreach (var mappedStock in mappedStockModel)
                {
                    var scripList = new List<TblScripMaster>();
                    var n = 1;
                    var scripName = mappedStock.StScripname.Split('.')[0].Split(' ');

                    do
                    {
                        string? scripData = "";
                        for (var j = 0; j <= n; j++)
                        {
                            if (string.IsNullOrEmpty(scripData))
                                scripData = scripName[j];
                            else
                                scripData += " " + scripName[j];
                        }
                        scripList = scrips.Where(x => x.Scripname != null && x.Scripname.ToLower().Contains(scripData.ToLower())).ToList();
                        n += 1;
                    } while (scripList.Count() != 1 && n < scripName.Count());

                    if (scripList.Count() == 1)
                        mappedStock.StScripname = scripList.First().Scripname;

                    mappedStock.FirmName = firmName;
                    mappedStock.FileType = fileType;
                }

                //To override existing data
                if (overrideData)
                {
                    var stockDataIfExists = await _stocksRepository.GetStockDataFromSpecificDateRangeForImport(mappedStockModel.First().StDate, mappedStockModel.Last().StDate, firmName, fileType);

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
                var fileType = "EQ/FO";
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
                        s.FileType = fileType;
                    });

                    if (overrideData)
                    {
                        var listOfDates = new List<DateTime>();
                        listStocks.ForEach(s => listOfDates.Add(s.Date));
                        var stockDataIfExists = await _stocksRepository.GetStockDataFromSpecificDateRangeForImport(listOfDates.Min(), listOfDates.Max(), firmName, fileType);

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

        #region Import BhavCopy (Daily Stock Price) file 
        public async Task<int> ImportDailyStockPriceFileAsync(DateTime date)
        {
            try
            {
                //NSE Equity File Import
                var nseFilePath = $"https://archives.nseindia.com/content/historical/EQUITIES/{date.Year}/{date.ToString("MMM").ToUpper()}/cm{date.ToString("ddMMMyyyy").ToUpper()}bhav.csv.zip";
                nseFilePath = await ExtractFile(nseFilePath);

                //NSE FNO File Import
                var nseFNOFilePath = $"https://archives.nseindia.com/content/historical/DERIVATIVES/{date.Year}/{date.ToString("MMM").ToUpper()}/fo{date.ToString("ddMMMyyyy").ToUpper()}bhav.csv.zip";
                nseFNOFilePath = await ExtractFile(nseFNOFilePath);

                //BSE File Import
                var bseFilePath = $"https://www.bseindia.com/download/BhavCopy/Equity/EQ{date.ToString("ddMMyy")}_CSV.ZIP";
                bseFilePath = await ExtractFile(bseFilePath);

                List<TblScripMaster> UpdateScrips = new List<TblScripMaster>();

                var nseStockDataList = await SaveAndReadFile<AddNSEStockPriceDto>(nseFilePath);
                var nseFNOStockDataList = await SaveAndReadFile<AddFNONSEStockPriceDto>(nseFNOFilePath);
                var bseStockDataList = await SaveAndReadFile<AddBSEStockPriceDto>(bseFilePath);
                var allScrip = await _stocksRepository.GetAllScrip();

                foreach (var stock in nseStockDataList)
                {
                    var scrip = allScrip.FirstOrDefault(x => x.Isin == stock.ISIN && x.Exchange == "NSE");
                    if (scrip is not null)
                    {
                        scrip.Ltp = stock.LAST;
                        scrip.Date = date;

                        UpdateScrips.Add(scrip);
                    }
                    else
                    {
                        AddScripDto newScrip = new AddScripDto();
                        newScrip.Scripsymbol = stock.SYMBOL;
                        newScrip.Isin = stock.ISIN;
                        newScrip.Exchange = "NSE";
                        newScrip.Ltp = stock.LAST;
                        newScrip.Date = date;

                        var mapScrip = _mapper.Map<TblScripMaster>(newScrip);
                        UpdateScrips.Add(mapScrip);
                    }
                }

                foreach (var stock in bseStockDataList)
                {
                    var scrip = allScrip.FirstOrDefault(x => x.Scripsymbol == stock.SCCode && x.Exchange == "BSE");
                    if (scrip is not null)
                    {
                        scrip.Ltp = stock.LAST;
                        scrip.Date = date;

                        UpdateScrips.Add(scrip);
                    }
                    else
                    {
                        AddScripDto newScrip = new AddScripDto();
                        newScrip.Scripsymbol = stock.SCCode;
                        newScrip.Scripname = stock.SCName;
                        newScrip.Exchange = "BSE";
                        newScrip.Ltp = stock.LAST;
                        newScrip.Date = date;

                        var mapScrip = _mapper.Map<TblScripMaster>(newScrip);
                        UpdateScrips.Add(mapScrip);
                    }
                }

                foreach (var stock in nseFNOStockDataList)
                {
                    string? scripSymbol = null;
                    var stockDate = stock.EXPIRY_DT.ToString().Split("-");

                    if (stock.STRIKE_PR == 0)
                    {
                        scripSymbol = stock.SYMBOL + stockDate[2].Substring(2, 2) + stockDate[1] + stockDate[0] + "FUT";
                    }
                    else
                    {
                        scripSymbol = stock.SYMBOL + stockDate[2].Substring(2, 2) + stockDate[1] + stockDate[0] + stock.STRIKE_PR + stock.OPTION_TYP;
                    }

                    var scrip = allScrip.FirstOrDefault(x => x.Scripsymbol == scripSymbol && x.Exchange == "NSE");
                    if (scrip is not null)
                    {
                        scrip.Ltp = stock.CLOSE;
                        scrip.Date = date;

                        UpdateScrips.Add(scrip);
                    }
                    else
                    {
                        AddScripDto newScrip = new AddScripDto();
                        newScrip.Scripsymbol = scripSymbol;
                        newScrip.Exchange = "NSE";
                        newScrip.Ltp = stock.CLOSE;
                        newScrip.Date = date;

                        var mapScrip = _mapper.Map<TblScripMaster>(newScrip);
                        UpdateScrips.Add(mapScrip);
                    }
                }

                return await _stocksRepository.UpdateScripData(UpdateScrips);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        #endregion

        #region Extract File
        private async Task<string?> ExtractFile(string url)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient();
            HttpResponseMessage response = await httpClient.GetAsync(url);
            string? entryPath = null;

            if (response.IsSuccessStatusCode)
            {
                using (Stream zipStream = await response.Content.ReadAsStreamAsync())
                using (ZipArchive archive = new ZipArchive(zipStream))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        var directory = Directory.GetCurrentDirectory() + "\\wwwroot" + "\\CRM-Document\\NSEBSEFile";
                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }
                        entryPath = Path.Combine(directory, entry.FullName);

                        if (File.Exists(entryPath))
                        {
                            File.Delete(entryPath);
                        }

                        using (Stream entryStream = entry.Open())
                        using (FileStream fileStream = System.IO.File.Create(entryPath))
                        {
                            entryStream.CopyTo(fileStream);
                        }
                    }
                }
            }
            return entryPath;
        }
        #endregion

        #region Save And Read File
        public async Task<List<T>> SaveAndReadFile<T>(string filePath)
        {
            CultureInfo culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            culture.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy";
            Thread.CurrentThread.CurrentCulture = culture;

            List<T> stockDataList = new List<T>();

            using (var fs = new StreamReader(filePath))
            {
                // to load the records from the file in my List<CsvLine>
                stockDataList = new CsvReader(fs, culture).GetRecords<T>().ToList();
            }

            return stockDataList;
        }
        #endregion

        #region Import Sherkhan all client trade file.
        public async Task<(int, string)> ImportAllClientSherkhanFileAsync(IFormFile formFile, bool overrideData)
        {
            try
            {
                var xlsxFilePath = "";
                var firmName = "Sherkhan";
                var fileType = "EQ";
                var listStocks = new List<AddSherkhanAllClientStockDto>();
                var filePath = Path.GetTempFileName();
                var scrips = await _stocksRepository.GetAllScrip();
                var users = await _userMasterRepository.GetUserWhichClientCodeNotNull();

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                    await stream.DisposeAsync();
                }

                var directory = Directory.GetCurrentDirectory() + "\\wwwroot" + "\\CRM-Document\\Sherkhan";
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var ex = Path.GetExtension(formFile.FileName);

                //Delete file if already exists with same name
                if (File.Exists(Path.Combine(directory, formFile.FileName)))
                {
                    File.Delete(Path.Combine(directory, formFile.FileName));
                }
                var localFilePath = Path.Combine(directory, formFile.FileName);
                File.Copy(filePath, localFilePath);

                listStocks = await SaveAndReadFile<AddSherkhanAllClientStockDto>(localFilePath);
                var mappedStockModel = _mapper.Map<List<TblStockData>>(listStocks);

                foreach (var stockData in mappedStockModel)
                {
                    var exchange = stockData.StSettno.ToLower().Contains("nse") ? "NSE" : "BSE";
                    var clientCode = stockData.StClientcode;
                    var user = users.Where(x => string.Compare(x.UserClientCode, stockData.StClientcode, true) == 0).FirstOrDefault();
                    var scripList = new List<TblScripMaster>();
                    var n = 1;
                    var scripName = stockData.StScripname.Split('.', StringSplitOptions.RemoveEmptyEntries).ToList();

                    bool flag = false;
                    try
                    {
                        if (stockData.StScripname.Contains("-"))
                        {
                            //JOHNSON CONTROLS -HITACHI AIR
                            if (stockData.StScripname.Split('-', StringSplitOptions.TrimEntries)[0].Contains(' ') && stockData.StScripname.Split('-')[1].Count() > 0)
                                scripName = stockData.StScripname.Split('-')[1].Split(' ').ToList();
                            else
                            {
                                //Kolte - Patil Developers Limited
                                scripName = stockData.StScripname.Split('-')[0].Split(' ').ToList();
                                if (scripName.Any(s => s.Contains(" ")))
                                    scripName[0] += " -";
                                else
                                    scripName[0] += "-";
                                scripName.AddRange(stockData.StScripname.Split('-')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList());
                            }
                        }
                        else
                        {
                            //DR. REDDY LAB. LTD.
                            if (stockData.StScripname.Split('.')[0].Contains(' '))
                                scripName = stockData.StScripname.Split('.', StringSplitOptions.RemoveEmptyEntries)[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
                            else
                            {
                                scripName = stockData.StScripname.Split('.', StringSplitOptions.RemoveEmptyEntries)[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
                                scripName[0] += ".";
                                scripName.AddRange(stockData.StScripname.Split('.', StringSplitOptions.RemoveEmptyEntries)[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList());
                            }
                        }
                        do
                        {
                            scripList = scrips.Where(s => s.Scripname != null && s.Scripname.ToLower().Contains(stockData.StScripname.ToLower())).ToList();
                            if (scripList.Count() == 1)
                                continue;
                            string? scripData = "";
                            for (var j = 0; j <= n; j++)
                            {
                                if (string.IsNullOrEmpty(scripData))
                                    scripData = scripName[j];
                                else
                                {
                                    if (stockData.StScripname.Contains("-") && !stockData.StScripname.Split('-')[0].Split(' ').ToList().Contains(" ") && j <= 1)
                                        scripData += scripName[j];
                                    else if (stockData.StScripname.Contains(".") && !stockData.StScripname.Split('.')[0].Split(' ').ToList().Contains(" ") && j <= 1)
                                        scripData += scripName[j];
                                    else
                                        scripData += " " + scripName[j];
                                }

                            }
                            scripList = scrips.Where(x => x.Scripname != null && x.Scripname.ToLower().Contains(scripData.ToLower()) && x.Exchange.Contains(exchange)).ToList();
                            n += 1;
                        } while (scripList.Count() != 1 && n < scripName.Count());

                        if (scripList.Count() == 0)
                            return (0, $"Unable to find scripName {stockData.StScripname} with client code {stockData.StClientcode} in {exchange}.");
                    }
                    catch (Exception msg)
                    {
                        return (0, $"Unable to find scripName {stockData.StScripname} with client code {stockData.StClientcode} in {exchange}.");
                    }

                    int? userId = null;
                    string userName = "";
                    if (user is not null)
                    {
                        userId = user.UserId;
                        userName = user.UserName;
                    }
                    else
                    {
                        userName = stockData.StClientname;
                    }

                    stockData.Userid = userId;
                    stockData.StClientname = userName;
                    stockData.FirmName = firmName;
                    stockData.StScripname = scripList.FirstOrDefault() is not null ? scripList.FirstOrDefault().Scripname : stockData.StScripname;
                    stockData.FileType = fileType;
                }

                if (overrideData)
                {
                    var listOfDates = new List<DateTime>();
                    listStocks.ForEach(s => listOfDates.Add((DateTime)s.StDate));
                    var stockDataIfExists = await _stocksRepository.GetStockDataFromSpecificDateRangeForImport(listOfDates.Min(), listOfDates.Max(), firmName, fileType);

                    if (stockDataIfExists.Count > 0)
                        await _stocksRepository.DeleteData(stockDataIfExists);
                }
                //Add Data
                return (await _stocksRepository.AddData(mappedStockModel), "File imported sucessfully.");
            }
            catch (Exception ex)
            {
                return (0, ex.Message);
            }
        }
        #endregion

        #region Import NSE FNO Trade file.
        public async Task<(int, string)> ImportNSEFNOTradeFileAsync(IFormFile formFile, bool overrideData)
        {
            try
            {
                var users = await _userMasterRepository.GetUserWhichClientCodeNotNull();
                string? firmName = "Sherkhan";
                string? fileType = "FO";
                List<AddFNONSETradeListDto> addFNONSETradeLists = new List<AddFNONSETradeListDto>();

                var filePath = Path.GetTempFileName();
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }
                var directory = Directory.GetCurrentDirectory() + "\\wwwroot" + "\\CRM-Document\\NSE-FNOFile";
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

                addFNONSETradeLists = await SaveAndReadFile<AddFNONSETradeListDto>(localFilePath);
                foreach (var trade in addFNONSETradeLists.ToList())
                {
                    if (trade.TrxnType.Contains("B/F"))
                        addFNONSETradeLists.Remove(trade);
                }

                var mapStockData = _mapper.Map<List<TblStockData>>(addFNONSETradeLists);

                foreach (var stockData in mapStockData)
                {
                    var user = users.Where(x => string.Compare(x.UserClientCode, stockData.StClientcode, true) == 0).FirstOrDefault();

                    if (user is not null)
                    {
                        stockData.Userid = user.UserId;
                        stockData.StClientname = user.UserName;
                    }

                    //NIFTY FUT 28SEP 23 - NIFTY230928FUT
                    //TCS OPT 28SEP 23 CE  @ 3520 - TCS2309283520CE
                    var scripName = stockData.StScripname;

                    if (scripName.ToLower().Contains("FUT".ToLower()))
                    {
                        var name = scripName.Split(" ");
                        //var dateTemp = DateTime.ParseExact(name[2], "ddMMM", CultureInfo.InvariantCulture).ToString();
                        DateTime.TryParseExact(name[2], "ddMMM", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateConvert);
                        var monthNumber = new string[5];
                        string dateTemp = dateConvert.ToString();
                        if (dateTemp.Contains("-"))
                        {
                            monthNumber = dateTemp.Split("-");
                        }
                        else if (dateTemp.Contains("/"))
                        {
                            monthNumber = dateTemp.Split("/");
                        }
                        //var monthNumber = DateTime.ParseExact(name[2], "ddMMM", CultureInfo.CurrentCulture).ToString().Split("-");
                        var date = name[3] + monthNumber[1] + monthNumber[0];
                        stockData.StScripname = name[0] + date + name[1];
                    }
                    else if (scripName.ToLower().Contains("OPT".ToLower()))
                    {
                        var name = scripName.Split(" ");
                        //var dateTemp = DateTime.ParseExact(name[2], "ddMMM", CultureInfo.CurrentCulture).ToString();
                        DateTime.TryParseExact(name[2], "ddMMM", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateConvert);
                        var monthNumber = new string[5];
                        string dateTemp = dateConvert.ToString();
                        if (dateTemp.Contains("-"))
                        {
                            monthNumber = dateTemp.Split("-");
                        }
                        else if (dateTemp.Contains("/"))
                        {
                            monthNumber = dateTemp.Split("/");
                        }
                        var date = name[3] + monthNumber[1] + monthNumber[0];
                        stockData.StScripname = name[0] + date + name.Last() + name[4];
                    }
                    stockData.StNetcostvalue = Math.Abs((decimal)stockData.StNetcostvalue);
                    stockData.FirmName = firmName;
                    stockData.FileType = fileType;
                }

                if (overrideData)
                {
                    var listOfDates = new List<DateTime>();
                    addFNONSETradeLists.ForEach(s => listOfDates.Add((DateTime)s.StDate));
                    var stockDataIfExists = await _stocksRepository.GetStockDataFromSpecificDateRangeForImport(listOfDates.Min(), listOfDates.Max(), firmName, fileType);

                    if (stockDataIfExists.Count > 0)
                        await _stocksRepository.DeleteData(stockDataIfExists);
                }

                //Add Data
                return (await _stocksRepository.AddData(mapStockData), "File imported sucessfully.");
            }
            catch (Exception ex)
            {
                return (0, ex.Message);
            }
        }
        #endregion 
    }
}

