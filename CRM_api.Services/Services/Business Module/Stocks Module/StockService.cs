using AutoMapper;
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

        public StockService(IStocksRepository stocksRepository, IMapper mapper, IHttpClientFactory httpClientFactory, IUserMasterRepository userMasterRepository)
        {
            _stocksRepository = stocksRepository;
            _mapper = mapper;
            _httpClientFactory = httpClientFactory;
            _userMasterRepository = userMasterRepository;
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
            var stockData = await _stocksRepository.GetStockDataForSpecificDateRange(startDate, endDate, null);
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
                }

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

        #region Import Daily Stock Price file 
        public async Task<int> ImportDailyStockPriceFileAsync()
        {
            try
            {
                var date = DateTime.Now;

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
                        scrip.Date = DateTime.Now.Date;

                        UpdateScrips.Add(scrip);
                    }
                    else
                    {
                        AddScripDto newScrip = new AddScripDto();
                        newScrip.Scripsymbol = stock.SYMBOL;
                        newScrip.Isin = stock.ISIN;
                        newScrip.Exchange = "NSE";
                        newScrip.Ltp = stock.LAST;
                        newScrip.Date = DateTime.Now.Date;

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
                        scrip.Date = DateTime.Now.Date;

                        UpdateScrips.Add(scrip);
                    }
                    else
                    {
                        AddScripDto newScrip = new AddScripDto();
                        newScrip.Scripsymbol = stock.SCCode;
                        newScrip.Scripname = stock.SCName;
                        newScrip.Exchange = "BSE";
                        newScrip.Ltp = stock.LAST;
                        newScrip.Date = DateTime.Now.Date;

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
                        scrip.Date = DateTime.Now.Date;

                        UpdateScrips.Add(scrip);
                    }
                    else
                    {
                        AddScripDto newScrip = new AddScripDto();
                        newScrip.Scripsymbol = scripSymbol;
                        newScrip.Exchange = "NSE";
                        newScrip.Ltp = stock.CLOSE;
                        newScrip.Date = DateTime.Now.Date;

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

        #region Import Sharekhan trade file.
        public async Task<int> ImportAllClientSherkhanFileAsync(IFormFile formFile, bool overrideData)
        {
            try
            {
                var xlsxFilePath = "";
                var firmName = "sharekhan";
                var listStocks = new List<AddSharekhanStocksDto>();
                var filePath = Path.GetTempFileName();
                var scrips = await _stocksRepository.GetAllScrip();
                var users = await _userMasterRepository.GetUserWhichClientCodeNotNull();

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                    await stream.DisposeAsync();
                }

                var directory = Directory.GetCurrentDirectory() + "\\wwwroot" + "\\CRM-Document\\Sharekhan";
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
                xlsxFilePath = Path.Combine(directory, formFile.FileName);
                if (File.Exists(xlsxFilePath))
                {
                    File.Delete(xlsxFilePath);
                }

                // saving .xls file into folder
                File.Copy(filePath, xlsxFilePath);

                WorkBook workbook = WorkBook.LoadExcel(xlsxFilePath);
                var worksheet = workbook.WorkSheets[0];

                //starts reading data from 2nd row of excel sheet
                for (var i = 1; i < worksheet.Rows.Count(); i++)
                {
                    var clientCode = worksheet.Rows[i].Columns[1].Value.ToString();
                    var user = users.Where(x => x.UserClientCode.Equals(clientCode)).FirstOrDefault();
                    var scripList = new List<TblScripMaster>();
                    var n = 1;
                    var scripName = worksheet.Rows[i].Columns[3].Value.ToString().Split('.')[0].Split(' ');

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

                    int? userId = null;
                    string userName = "";
                    if (user is not null)
                    {
                        userId = user.UserId;
                        userName = user.UserName;
                    }
                    else
                    {
                        userName = worksheet.Rows[i].Columns[2].Value.ToString();
                    }

                    var trans = new AddSharekhanStocksDto()
                    {
                        StScripname = scripList.FirstOrDefault() is null ? worksheet.Rows[i].Columns[3].Value.ToString() : scripList.First().Scripname,
                        StBranch = Convert.ToInt32(worksheet.Rows[i].Columns[0].Value.ToString()),
                        StClientcode = clientCode,
                        StClientname = userName,
                        StSettno = worksheet.Rows[i].Columns[4].Value.ToString(),
                        StDate = Convert.ToDateTime(worksheet.Rows[i].Columns[5].Value.ToString()),
                        StType = worksheet.Rows[i].Columns[6].Value.ToString(),
                        StQty = Convert.ToInt32(worksheet.Rows[i].Columns[7].Value.ToString()),
                        StRate = Convert.ToDecimal(worksheet.Rows[i].Columns[8].Value.ToString()),
                        StBrokerage = Convert.ToDecimal(worksheet.Rows[i].Columns[9].Value.ToString()),
                        StNetrate = Math.Abs(Convert.ToDecimal(worksheet.Rows[i].Columns[10].Value.ToString())),
                        StNetvalue = Convert.ToDecimal(worksheet.Rows[i].Columns[11].Value.ToString()),
                        StCostpershare = Convert.ToDecimal(worksheet.Rows[i].Columns[12].Value.ToString()),
                        StCostvalue = Convert.ToDecimal(worksheet.Rows[i].Columns[13].Value.ToString()),
                        StNetsharerate = Convert.ToDecimal(worksheet.Rows[i].Columns[14].Value.ToString()),
                        StNetcostvalue = Convert.ToDecimal(worksheet.Rows[i].Columns[15].Value.ToString()),
                        Userid = userId
                    };
                    listStocks.Insert(0, trans);
                }

                var mappedStockModel = _mapper.Map<List<TblStockData>>(listStocks);
                mappedStockModel.ForEach(x => x.FirmName = firmName);

                if (overrideData)
                {
                    var listOfDates = new List<DateTime>();
                    listStocks.ForEach(s => listOfDates.Add((DateTime)s.StDate));
                    var stockDataIfExists = await _stocksRepository.GetStockDataForSpecificDateRange(listOfDates.Min(), listOfDates.Max(), firmName);

                    if (stockDataIfExists.Count > 0)
                        await _stocksRepository.DeleteData(stockDataIfExists);
                }
                //Add Data
                return await _stocksRepository.AddData(mappedStockModel);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        #endregion
    }
}

