using AutoMapper;
using AutoMapper.Configuration.Annotations;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Business_Module.MutualFunds_Module;
using CRM_api.DataAccess.IRepositories.User_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.MutualFunds_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.MutualFunds_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.User_Module;
using CRM_api.Services.Helper.Extensions;
using CRM_api.Services.IServices.Business_Module.MutualFunds_Module;
using CsvHelper;
using CsvHelper.Configuration;
using ExcelDataReader;
using IronXL;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using Winnovative.PdfToText;

namespace CRM_api.Services.Services.Business_Module.MutualFunds_Module
{
    public class MutualfundService : IMutualfundService
    {
        private readonly IMutualfundRepository _mutualfundRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMapper _mapper;
        private readonly IUserMasterRepository _userMasterRepository;

        public MutualfundService(IMutualfundRepository mutualfundReposiory, IMapper mapper, IUserMasterRepository userMasterRepository, IHttpClientFactory httpClientFactory)
        {
            _mutualfundRepository = mutualfundReposiory;
            _mapper = mapper;
            _userMasterRepository = userMasterRepository;
            _httpClientFactory = httpClientFactory;
        }

        #region Get Client wise Mutual Fund Transaction
        public async Task<MFTransactionDto<MutualFundDto>> GetClientwiseMutualFundTransactionAsync(int userId, string? schemeName, string? folioNo
            , string? searchingParams, SortingParams sortingParams, DateTime? startDate, DateTime? endDate)
        {
            var mutualFundTransaction = await _mutualfundRepository.GetTblMftransactions(userId, schemeName, folioNo, searchingParams, sortingParams, startDate, endDate);
            var mapMutualFundTransaction = _mapper.Map<MFTransactionDto<MutualFundDto>>(mutualFundTransaction);
            return mapMutualFundTransaction;
        }
        #endregion

        #region Get Client wise MF Summary
        public async Task<MFTransactionDto<MFSummaryDto>> GetMFSummaryAsync(int userId, bool? isBalanceUnitZero, string? searchingParams, SortingParams sortingParams)
        {
            List<MFSummaryDto> mutualFundSummaries = new List<MFSummaryDto>();

            double pageCount = 0;
            var mfSummary = await _mutualfundRepository.GetMFTransactionSummary(userId);
            var allScheme = await _mutualfundRepository.GetAllMFScheme();

            foreach (var records in mfSummary)
            {
                var mfSummaryDto = new MFSummaryDto();
                decimal? redemptionUnit = 0;
                decimal? totalPurchaseUnits = 0;

                var nav = allScheme.FirstOrDefault(x => x.SchemeName.ToLower().Equals(records.Key.Replace("  ", " ").ToLower()));
                if (nav != null) 
                    mfSummaryDto.NAV = Convert.ToDouble(nav.NetAssetValue);
                else
                    mfSummaryDto.NAV = 0;

                mfSummaryDto.SchemeId = records.DistinctBy(x => x.SchemeId).Select(x => x.SchemeId).FirstOrDefault();
                mfSummaryDto.Schemename = records.Key;
                mfSummaryDto.Foliono = records.DistinctBy(x => x.Foliono).Select(x => x.Foliono).FirstOrDefault();

                var redemptionTransaction = records.Where(x => x.Transactiontype == "SWO" || x.Transactiontype == "RED" || x.Transactiontype == "Sale");
                foreach (var transaction in redemptionTransaction)
                {
                    redemptionUnit += transaction.Noofunit;
                }

                var purchaseTransaction = records.Where(x => x.Transactiontype != "SWO" && x.Transactiontype != "RED" && x.Transactiontype != "Sale");
                foreach (var transaction in purchaseTransaction)
                {
                    totalPurchaseUnits += transaction.Noofunit;
                }

                mfSummaryDto.TotalPurchaseUnit = Math.Round((decimal)totalPurchaseUnits, 3);
                mfSummaryDto.TotalRedemptionUnit = Math.Round((decimal)redemptionUnit, 3);
                mfSummaryDto.BalanceUnit = Math.Round((decimal)(mfSummaryDto.TotalPurchaseUnit - mfSummaryDto.TotalRedemptionUnit), 3);
                mfSummaryDto.CurrentValue = Math.Round((decimal)(mfSummaryDto.BalanceUnit * (decimal)mfSummaryDto.NAV), 3);

                mutualFundSummaries.Add(mfSummaryDto);
            }

            if (isBalanceUnitZero is true)
            {
                mutualFundSummaries = mutualFundSummaries.Where(x => x.BalanceUnit == 0).ToList();
            }
            else
            {
                mutualFundSummaries = mutualFundSummaries.Where(x => x.BalanceUnit != 0).ToList();
            }

            IQueryable<MFSummaryDto> mutualFundSummaryDto = mutualFundSummaries.AsQueryable();

            var totalBalanceUnit = mutualFundSummaries.Sum(x => x.BalanceUnit);
            var totalAmount = mutualFundSummaries.Sum(x => x.CurrentValue);
            var totalScheme = mutualFundSummaries.Count();

            if (searchingParams != null)
            {
                mutualFundSummaryDto = mutualFundSummaryDto.Where(x => x.SchemeId.ToString().Contains(searchingParams) || x.Schemename.ToLower().Contains(searchingParams.ToLower()) || x.Foliono.ToLower().Contains(searchingParams.ToLower())
                            || x.TotalPurchaseUnit.ToString().Contains(searchingParams) || x.TotalRedemptionUnit.ToString().Contains(searchingParams)
                            || x.BalanceUnit.ToString().Contains(searchingParams) || x.NAV.ToString().Contains(searchingParams) || x.CurrentValue.ToString().Contains(searchingParams));
            }

            pageCount = Math.Ceiling(mutualFundSummaryDto.Count() / sortingParams.PageSize);

            // Apply Sorting 
            var sortingData = SortingExtensions.ApplySorting(mutualFundSummaryDto, sortingParams.SortBy, sortingParams.IsSortAscending);

            //Apply Pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortingData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var mutualfundData = new ResponseDto<MFSummaryDto>()
            {
                Values = paginatedData,
                Pagination = new PaginationDto()
                {
                    Count = (int)pageCount,
                    CurrentPage = sortingParams.PageNumber
                }
            };

            var mutualfundResponse = new MFTransactionDto<MFSummaryDto>()
            {
                response = mutualfundData,
                totalBalanceUnit = totalBalanceUnit,
                totalAmount = totalAmount,
                totalScheme = totalScheme,
            };

            return mutualfundResponse;
        }
        #endregion

        #region Get Client wise MF Summary Category Wise
        public async Task<MFTransactionDto<MFCategoryWiseDto>> GetMFCategoryWiseAsync(int userId, bool? isBalanceUnitZero, string? searchingParams, SortingParams sortingParams)
        {
            List<MFCategoryWiseDto> mutualFundSummaries = new List<MFCategoryWiseDto>();

            double pageCount = 0;
            var mfSummary = await _mutualfundRepository.GetMFTransactionSummaryByCategory(userId);

            foreach (var records in mfSummary)
            {
                var mfCategoryWise = new MFCategoryWiseDto();
                decimal? redemptionUnit = 0;
                decimal? totalPurchaseUnits = 0;

                mfCategoryWise.NAV = Math.Round((double)records.Average(x => x.Nav), 3);
                mfCategoryWise.CategoryName = records.DistinctBy(x => x.TblMfSchemeMaster.SchemeCategorytype).Select(x => x.TblMfSchemeMaster.SchemeName).FirstOrDefault();

                var redemptionTransaction = records.Where(x => x.Transactiontype == "SWO" || x.Transactiontype == "RED" || x.Transactiontype == "Sale");
                foreach (var transaction in redemptionTransaction)
                {
                    redemptionUnit += transaction.Noofunit;
                }

                var purchaseTransaction = records.Where(x => x.Transactiontype != "SWO" && x.Transactiontype != "RED" && x.Transactiontype != "Sale");
                foreach (var transaction in purchaseTransaction)
                {
                    totalPurchaseUnits += transaction.Noofunit;
                }

                mfCategoryWise.TotalPurchaseUnit = Math.Round((decimal)totalPurchaseUnits, 3);
                mfCategoryWise.TotalRedemptionUnit = Math.Round((decimal)redemptionUnit, 3);
                mfCategoryWise.BalanceUnit = Math.Round((decimal)(mfCategoryWise.TotalPurchaseUnit - mfCategoryWise.TotalRedemptionUnit), 3);
                mfCategoryWise.CurrentValue = Math.Round((decimal)(mfCategoryWise.BalanceUnit * (decimal)mfCategoryWise.NAV), 3);

                mutualFundSummaries.Add(mfCategoryWise);
            }

            if (isBalanceUnitZero is true)
            {
                mutualFundSummaries = mutualFundSummaries.Where(x => x.BalanceUnit == 0).ToList();
            }
            else
            {
                mutualFundSummaries = mutualFundSummaries.Where(x => x.BalanceUnit != 0).ToList();
            }

            IQueryable<MFCategoryWiseDto> mutualFundSummaryDto = mutualFundSummaries.AsQueryable();

            var totalBalanceUnit = mutualFundSummaries.Sum(x => x.BalanceUnit);
            var totalAmount = mutualFundSummaries.Sum(x => x.CurrentValue);

            if (searchingParams != null)
            {
                mutualFundSummaryDto = mutualFundSummaryDto.Where(x => x.CategoryName.ToLower().Contains(searchingParams.ToLower()) || x.TotalPurchaseUnit.ToString().Contains(searchingParams)
                            || x.TotalRedemptionUnit.ToString().Contains(searchingParams) || x.BalanceUnit.ToString().Contains(searchingParams)
                            || x.NAV.ToString().Contains(searchingParams) || x.CurrentValue.ToString().Contains(searchingParams));
            }

            pageCount = Math.Ceiling(mutualFundSummaryDto.Count() / sortingParams.PageSize);

            //Apply Sorting
            var sortingData = SortingExtensions.ApplySorting(mutualFundSummaryDto, sortingParams.SortBy, sortingParams.IsSortAscending);

            //Apply Pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortingData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var mutualfundData = new ResponseDto<MFCategoryWiseDto>()
            {
                Values = paginatedData,
                Pagination = new PaginationDto()
                {
                    Count = (int)pageCount,
                    CurrentPage = sortingParams.PageNumber
                }
            };

            var mutualfundResponse = new MFTransactionDto<MFCategoryWiseDto>()
            {
                response = mutualfundData,
                totalBalanceUnit = totalBalanceUnit,
                totalAmount = totalAmount,
            };

            return mutualfundResponse;
        }
        #endregion

        #region Get All Client MF Summary 
        public async Task<MFTransactionDto<AllClientMFSummaryDto>> GetAllClientMFSummaryAsync(bool? isBalanceUnitZero, DateTime fromDate, DateTime toDate, string? searchingParams, SortingParams sortingParams)
        {
            List<AllClientMFSummaryDto> mutualFundSummaries = new List<AllClientMFSummaryDto>();

            double pageCount = 0;
            var mfSummary = await _mutualfundRepository.GetAllCLientMFSummary(fromDate, toDate);
            var allScheme = await _mutualfundRepository.GetAllMFScheme();

            foreach (var records in mfSummary)
            {
                var allClientMFSummary = new AllClientMFSummaryDto();
                decimal? redemptionUnit = 0;
                decimal? totalPurchaseUnits = 0;
                decimal? currentValue = 0;
                decimal? avgNAV = 0;

                allClientMFSummary.Userid = records.DistinctBy(x => x.Userid).Select(x => x.Userid).FirstOrDefault();
                allClientMFSummary.Username = records.Key;
                var purchaseTransaction = records.Where(x => x.Transactiontype != "SWO" && x.Transactiontype != "RED" && x.Transactiontype != "Sale");
                allClientMFSummary.TotalPurchaseUnit = Math.Round((decimal)purchaseTransaction.Sum(x => x.Noofunit), 3);

                var redemptionTransaction = records.Where(x => x.Transactiontype == "SWO" || x.Transactiontype == "RED" || x.Transactiontype == "Sale");
                allClientMFSummary.TotalRedemptionUnit = Math.Round((decimal)redemptionTransaction.Sum(x => x.Noofunit), 3);
                allClientMFSummary.BalanceUnit = allClientMFSummary.TotalPurchaseUnit - allClientMFSummary.TotalRedemptionUnit;

                var schemewiseRecords = records.GroupBy(x => x.Schemename).ToList();

                foreach(var record in schemewiseRecords)
                {
                    decimal? nav = 0;

                    var purchase = record.Where(x => x.Transactiontype != "SWO" && x.Transactiontype != "RED" && x.Transactiontype != "Sale");
                    totalPurchaseUnits = purchase.Sum(x => x.Noofunit);

                    var redeem = record.Where(x => x.Transactiontype == "SWO" || x.Transactiontype == "RED" || x.Transactiontype == "Sale");
                    redemptionUnit = redeem.Sum(x => x.Noofunit);

                    var balanceUnit = totalPurchaseUnits - redemptionUnit;
                    var scheme = allScheme.FirstOrDefault(x => x.SchemeName.ToLower().Equals(record.Key.Replace("  ", " ").ToLower()));
                    if (scheme is not null)
                        nav = Convert.ToDecimal(scheme.NetAssetValue);
                    else
                        nav = 0;

                    currentValue += balanceUnit * nav;
                    avgNAV += nav;
                }
                allClientMFSummary.NAV = Convert.ToDouble(Math.Round((decimal)(avgNAV / schemewiseRecords.Count()), 3));
                allClientMFSummary.CurrentValue = Math.Round((decimal)currentValue, 3);

                mutualFundSummaries.Add(allClientMFSummary);
            }

            if (isBalanceUnitZero is true)
            {
                mutualFundSummaries = mutualFundSummaries.Where(x => x.BalanceUnit == 0).ToList();
            }
            else
            {
                mutualFundSummaries = mutualFundSummaries.Where(x => x.BalanceUnit != 0).ToList();
            }

            IQueryable<AllClientMFSummaryDto> mutualFundSummaryDto = mutualFundSummaries.AsQueryable();

            var totalBalanceUnit = mutualFundSummaries.Sum(x => x.BalanceUnit);
            var totalAmount = mutualFundSummaries.Sum(x => x.CurrentValue);

            if (searchingParams != null)
            {
                mutualFundSummaryDto = mutualFundSummaryDto.Where(x => x.Userid.ToString().Contains(searchingParams) || x.Username.ToLower().Contains(searchingParams.ToLower())
                            || x.TotalPurchaseUnit.ToString().Contains(searchingParams) || x.TotalRedemptionUnit.ToString().Contains(searchingParams)
                            || x.BalanceUnit.ToString().Contains(searchingParams) || x.NAV.ToString().Contains(searchingParams) || x.CurrentValue.ToString().Contains(searchingParams));
            }

            pageCount = Math.Ceiling(mutualFundSummaryDto.Count() / sortingParams.PageSize);

            //Apply Sorting
            var sortingData = SortingExtensions.ApplySorting(mutualFundSummaryDto, sortingParams.SortBy, sortingParams.IsSortAscending);

            //Apply Pagination
            var paginatedData = SortingExtensions.ApplyPagination(sortingData, sortingParams.PageNumber, sortingParams.PageSize).ToList();

            var mutualfundData = new ResponseDto<AllClientMFSummaryDto>()
            {
                Values = paginatedData,
                Pagination = new PaginationDto()
                {
                    Count = (int)pageCount,
                    CurrentPage = sortingParams.PageNumber
                }
            };

            var mutualfundResponse = new MFTransactionDto<AllClientMFSummaryDto>()
            {
                response = mutualfundData,
                totalBalanceUnit = totalBalanceUnit,
                totalAmount = totalAmount,
            };

            return mutualfundResponse;
        }
        #endregion

        #region Get All MFUserName
        public async Task<ResponseDto<UserNameDto>> GetMFUserNameAsync(string? searchingParams, SortingParams sortingParams)
        {
            var mfUser = await _mutualfundRepository.GetMFUserName(searchingParams, sortingParams);
            var mapMFUser = _mapper.Map<ResponseDto<UserNameDto>>(mfUser);

            foreach (var user in mapMFUser.Values)
            {
                user.UserName = user.UserName.ToLower();
            }

            return mapMFUser;
        }
        #endregion

        #region Get AMFI nav data
        public async Task<ResponseDto<AmfiNavDto>> GetAMFINavDataAsync(string? search, SortingParams sortingParams)
        {
            var navData = await _mutualfundRepository.GetAMFINavList(true, search, sortingParams);
            var mappedNavData = _mapper.Map<ResponseDto<AmfiNavDto>>(navData.Item2);
            return mappedNavData;
        }
        #endregion

        #region Get AMFI scheme data
        public async Task<ResponseDto<AmfiSchemeDto>> GetAMFISchemeDataAsync(string? search, SortingParams sortingParams)
        {
            var navSchemes = await _mutualfundRepository.GetAMFISchemesList(true, search, sortingParams);
            var mappedSchemeData = _mapper.Map<ResponseDto<AmfiSchemeDto>>(navSchemes.Item2);
            return mappedSchemeData;
        }
        #endregion

        #region Display SchemeName
        public async Task<ResponseDto<SchemaNameDto>> DisplayschemeNameAsync(int userId, string? folioNo, string? searchingParams, SortingParams sortingParams)
        {
            var mutualfunds = await _mutualfundRepository.GetSchemeName(userId, folioNo, searchingParams, sortingParams);
            var schemeName = _mapper.Map<ResponseDto<SchemaNameDto>>(mutualfunds);

            foreach (var scheme in schemeName.Values)
            {
                scheme.Schemename = scheme.Schemename.ToLower();
            }

            return schemeName;
        }
        #endregion

        #region Display Folio Number List
        public async Task<ResponseDto<SchemaNameDto>> DisplayFolioNoAsync(int userId, string? schemeName, string? searchingParams, SortingParams sortingParams)
        {
            var mutualfunds = await _mutualfundRepository.GetFolioNo(userId, schemeName, searchingParams, sortingParams);
            var folioNo = _mapper.Map<ResponseDto<SchemaNameDto>>(mutualfunds);

            return folioNo;
        }
        #endregion

        #region Import NJ Client File
        public async Task<int> ImportNJClientFileAsync(IFormFile file, bool updateIfExist)
        {
            List<AddMutualfundsDto> existUserTransaction = new List<AddMutualfundsDto>();
            List<AddMutualfundsDto> notExistUserTransaction = new List<AddMutualfundsDto>();

            var filePath = System.IO.Path.GetTempFileName();
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var directoryPath = Directory.GetCurrentDirectory() + "\\wwwroot" + "\\CRM-Document\\NJCLientFile";

            if (!(Directory.Exists(directoryPath)))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var localFilePath = System.IO.Path.Combine(directoryPath, file.FileName);

            if (File.Exists(localFilePath))
            {
                File.Delete(localFilePath);
            }
            File.Copy(filePath, localFilePath);

            var user = await _userMasterRepository.GetAllUser();
            var allScheme = await _mutualfundRepository.GetAllMFScheme();

            using (var stream = File.Open(localFilePath, FileMode.Open, FileAccess.Read))
            {
                try
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var dataset = reader.AsDataSet(new ExcelDataSetConfiguration
                        {
                            ConfigureDataTable = _ => new ExcelDataTableConfiguration
                            {
                                UseHeaderRow = true,
                                ReadHeaderRow = rowReader =>
                                {
                                    // Read the header row starting from the fourth row
                                    for (var i = 0; i < 3; i++)
                                    {
                                        rowReader.Read();
                                    }
                                },
                            }
                        });

                        var records = new List<AddMutualfundsDto>();

                        foreach (DataTable datatable in dataset.Tables)
                        {
                            var sheetRecords = datatable.AsEnumerable().SkipLast(5).ToList().ConvertAll<AddMutualfundsDto>(row =>
                            {
                                var obj = new AddMutualfundsDto();

                                obj.Username = row["Investor"] == DBNull.Value ? null : row["Investor"].ToString();
                                obj.Transactiontype = row["Type"] == DBNull.Value ? null : row["Type"].ToString();
                                obj.Schemename = row["Scheme"] == DBNull.Value ? null : row["Scheme"].ToString();
                                obj.Foliono = row["Folio No/Demat A/C"] == DBNull.Value ? null : row["Folio No/Demat A/C"].ToString().Trim('*').Replace("/", "");
                                obj.Tradeno = row["Tr. No."] == DBNull.Value ? null : row["Tr. No."].ToString();
                                obj.Date = datatable.TableName.Contains("Purchase Report") ? Convert.ToDateTime(row["Purchase Date"] == DBNull.Value ? null : row["Purchase Date"]) : Convert.ToDateTime(row["Redemption Date"] == DBNull.Value ? null : row["Redemption Date"]);
                                obj.Nav = Convert.ToDouble(row["NAV(₹)"] == DBNull.Value ? null : row["NAV(₹)"]);
                                obj.Noofunit = datatable.TableName.Contains("Purchase Report") ? Convert.ToDecimal(row["Purchase units"] == DBNull.Value ? null : row["Purchase units"]) : Convert.ToDecimal(row["No. of Units"] == DBNull.Value ? null : row["No. of Units"]);
                                obj.Invamount = datatable.TableName.Contains("Purchase Report") ? Convert.ToDecimal(row["Gross Inv. Amount(₹)"] == DBNull.Value ? null : row["Gross Inv. Amount(₹)"]) : Convert.ToDecimal(row["Amount(₹)"] == DBNull.Value ? null : row["Amount(₹)"]);
                                obj.Userpan = row["PAN"] == DBNull.Value ? null : row["PAN"].ToString();


                                var userId = user.FirstOrDefault(x => x.UserPan?.ToLower() == obj.Userpan?.ToLower())?.UserId;
                                var schemeId = allScheme.FirstOrDefault(x => x.SchemeName.ToLower() == obj.Schemename.ToLower())?.SchemeId;

                                if (userId == 0)
                                    obj.Userid = null;
                                else
                                    obj.Userid = userId;

                                if (schemeId == 0)
                                    obj.SchemeId = null;
                                else
                                    obj.SchemeId = schemeId;

                                return obj;
                            });

                            records.AddRange(sheetRecords);
                        }

                        var notExistUser = records.Where(x => x.Userid == null).ToList();
                        notExistUserTransaction.AddRange(notExistUser);

                        var existUser = records.Where(x => x.Userid != null).ToList();
                        existUserTransaction.AddRange(existUser);

                        var mapRecordsForExistUser = _mapper.Map<List<TblMftransaction>>(existUserTransaction);
                        var mapRecordsForNotExistUser = _mapper.Map<List<TblNotexistuserMftransaction>>(notExistUserTransaction);

                        if (updateIfExist)
                        {
                            if (mapRecordsForExistUser.Count > 0)
                            {
                                var GetDataForExistUser = await _mutualfundRepository.GetMFInSpecificDateForExistUser(
                                                            mapRecordsForExistUser.First().Date, mapRecordsForExistUser.Last().Date);

                                if (GetDataForExistUser.Count > 0)
                                {
                                    foreach (var record in GetDataForExistUser)
                                    {
                                        await _mutualfundRepository.DeleteMFForUserExist(record);
                                    }
                                }
                            }

                            if (mapRecordsForNotExistUser.Count > 0)
                            {
                                var GetDataForNotExistUser = await _mutualfundRepository.GetMFInSpecificDateForNotExistUser(
                                                                mapRecordsForNotExistUser.First().Date, mapRecordsForNotExistUser.Last().Date);

                                if (GetDataForNotExistUser.Count > 0)
                                {
                                    foreach (var record in GetDataForNotExistUser)
                                    {
                                        await _mutualfundRepository.DeleteMFForNotUserExist(record);
                                    }
                                }
                            }
                        }
                        if (mapRecordsForExistUser.Count > 0)
                        {
                            await _mutualfundRepository.AddMFDataForExistUser(mapRecordsForExistUser);
                        }
                        await _mutualfundRepository.AddMFDataForNotExistUser(mapRecordsForNotExistUser);
                    }
                    return 1;
                }
                catch (Exception)
                {
                    return 0;
                }

            }
        }
        #endregion

        #region Import CAMS/Karvy Pdf file
        public async Task<int> ImportCAMSFileAsync(IFormFile file, string password, bool UpdateIfExist)
        {
            try
            {
                if (file == null || file.Length <= 0)
                {
                    throw new Exception("Invalid File.");
                }

                List<AddMutualfundsDto> mutualfundDto = new List<AddMutualfundsDto>();
                List<AddMutualfundsDto> existUserTransaction = new List<AddMutualfundsDto>();
                List<AddMutualfundsDto> notExistUserTransaction = new List<AddMutualfundsDto>();
                List<TblMftransaction> mapRecordForExistUser = new List<TblMftransaction>();
                List<TblNotexistuserMftransaction> mapRecordforNotExistUser = new List<TblNotexistuserMftransaction>();

                PdfToTextConverter pdfToTextConverter = new PdfToTextConverter();
                pdfToTextConverter.UserPassword = password;
                //pdfToTextConverter.LicenseKey = "C4WVhJaXhJSElZKKlISWl4qUkIqWlJaXhJQ=";

                pdfToTextConverter.Layout = TextLayout.TableModeLayout;
                pdfToTextConverter.MarkPageBreaks = false;

                // extract text from PDF
                string extractedText = pdfToTextConverter.ConvertToText(file.OpenReadStream());

                string[] lines = extractedText.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);

                var folioNo = string.Empty;
                var panNo = string.Empty;
                var schemeName = string.Empty;
                var userName = string.Empty;

                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];

                    if (line.Contains("***"))
                        continue;

                    // Get UserName
                    if (line.Contains("Email Id:"))
                    {
                        int index = i;
                        var splitStringUserName = lines[index + 2].Split("balances", StringSplitOptions.RemoveEmptyEntries).ToArray();
                        userName = splitStringUserName[0].Trim();
                    }

                    if (line.Contains("Folio No:") || line.Contains("Folio No :"))
                    {
                        // Get Pan No
                        if (line.Contains("PAN: OK") || line.Contains("PAN : OK"))
                        {
                            string[] splitStringPan = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                            int index = Array.IndexOf(splitStringPan, "PAN:");
                            panNo = splitStringPan[index + 1];
                        }
                        else
                        {
                            panNo = "";
                        }

                        // Get Folio No
                        string[] splitStringFolio = line.Split(" ");
                        if (line.Contains("Folio No :"))
                        {
                            splitStringFolio = line.Replace("Folio No :", "Folio No:").Split(" ");
                            int index = Array.IndexOf(splitStringFolio, "PAN:");
                            splitStringFolio = splitStringFolio.Take(index).ToArray();
                        }

                        folioNo = string.Join(" ", splitStringFolio, 2, Math.Min(splitStringFolio.Length - 2, 3));
                    }

                    // Get Scheme Name
                    if (line.Contains("Registrar"))
                    {
                        string[] splitSchemeString;
                        string replaceSchemeString;
                        if (line.Contains('('))
                        {
                            replaceSchemeString = Regex.Replace(line, @"\([^()]*\)", string.Empty).Replace("-", " ").Replace(@"\s+", "");
                            splitSchemeString = replaceSchemeString.Split(" ").Skip(1).TakeWhile(x => String.Compare("Registrar", x, true) != 0).Where(x => x != "").ToArray();
                            schemeName = string.Join(" ", splitSchemeString);

                            if (schemeName.Contains("Advisor:"))
                            {
                                splitSchemeString = schemeName.Split("(Advisor:", StringSplitOptions.RemoveEmptyEntries).ToArray();
                                schemeName = string.Join(" ", splitSchemeString);
                            }
                        }
                        else
                        {
                            replaceSchemeString = Regex.Replace(line.Replace("-", " "), @"\s+", " ");
                            splitSchemeString = replaceSchemeString.Split(" ").Skip(1).TakeWhile(x => String.Compare("Registrar", x, true) != 0).ToArray();
                            schemeName = string.Join(" ", splitSchemeString);
                        }
                    }

                    // Get Transaction
                    if (line.Contains("CAMSCASWS"))
                        line = line.Split("CAMSCASWS")[0].TrimEnd();

                    string[] parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    decimal decimalNum;
                    if ((DateTime.TryParseExact(parts[0], "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime invDate)) && Decimal.TryParse(parts[^1], out decimalNum))
                    {
                        if (parts.Length > 0)
                        {
                            string date = parts[0];
                            string purchaseSIP = string.Join(" ", parts, 1, Math.Min(parts.Length - 1, parts.Length - 5));
                            string amount = parts[^4].StartsWith('(') ? parts[^4].TrimStart('(').TrimEnd(')') : parts[^4];
                            string units = parts[^3].StartsWith('(') ? parts[^3].TrimStart('(').TrimEnd(')') : parts[^3];
                            string price = parts[^2].StartsWith('(') ? parts[^2].TrimStart('(').TrimEnd(')') : parts[^2];
                            string unitBalance = parts[^1];

                            AddMutualfundsDto addMutualfunds = new AddMutualfundsDto();
                            addMutualfunds.Username = userName;
                            addMutualfunds.Foliono = folioNo;
                            addMutualfunds.Userpan = panNo;
                            addMutualfunds.Schemename = schemeName;
                            addMutualfunds.Date = Convert.ToDateTime(parts[0]);
                            addMutualfunds.Transactiontype = purchaseSIP.GetMFTransactionType();
                            addMutualfunds.Invamount = Convert.ToDecimal(amount);
                            addMutualfunds.Noofunit = Convert.ToDecimal(units);
                            addMutualfunds.Nav = Convert.ToDouble(price);
                            addMutualfunds.Unitbalance = Convert.ToDecimal(unitBalance);

                            mutualfundDto.Add(addMutualfunds);
                        }
                    }
                }

                var existUser = mutualfundDto.Where(x => x.Userid != null);
                existUserTransaction.AddRange(existUser);

                var notExistUser = mutualfundDto.Where(x => x.Userid == null);
                notExistUserTransaction.AddRange(notExistUser);

                mapRecordForExistUser = _mapper.Map<List<TblMftransaction>>(existUserTransaction);
                mapRecordforNotExistUser = _mapper.Map<List<TblNotexistuserMftransaction>>(notExistUserTransaction);

                if (UpdateIfExist)
                {
                    if (mapRecordForExistUser.Count > 0)
                    {
                        var getExistUserRecord = await _mutualfundRepository.GetMFInSpecificDateForExistUser(mapRecordForExistUser.First().Date, mapRecordForExistUser.Last().Date);

                        if (getExistUserRecord.Count > 0)
                        {
                            foreach (var record in getExistUserRecord)
                            {
                                await _mutualfundRepository.DeleteMFForUserExist(record);
                            }
                        }
                    }

                    if (mapRecordforNotExistUser.Count > 0)
                    {
                        var getRecordNotExistUserRecord = await _mutualfundRepository.GetMFInSpecificDateForNotExistUser(mapRecordforNotExistUser.First().Date, mapRecordforNotExistUser.Last().Date);

                        if (getRecordNotExistUserRecord.Count > 0)
                        {
                            foreach (var record in getRecordNotExistUserRecord)
                            {
                                await _mutualfundRepository.DeleteMFForNotUserExist(record);
                            }
                        }
                    }
                }

                if (mapRecordForExistUser.Count > 0)
                {
                    await _mutualfundRepository.AddMFDataForExistUser(mapRecordForExistUser);
                }
                await _mutualfundRepository.AddMFDataForNotExistUser(mapRecordforNotExistUser);

                return 0;
            }
            catch (Exception)
            {
                return -1;
            }
        }
        #endregion

        #region Import Daily NJ Price
        public async Task<int> ImportNJDailyPriceFileAsync(IFormFile formFile)
        {
            try
            {
                var listNJPrice = new List<TblMfSchemeMaster>();
                CultureInfo culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                culture.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy";
                Thread.CurrentThread.CurrentCulture = culture;
                var filePath = Path.GetTempFileName();
                List<AddNJDailyPriceDto> mutualfundNAVs = new List<AddNJDailyPriceDto>();
                var allScheme = await _mutualfundRepository.GetAllMFScheme();

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }

                var directory = Directory.GetCurrentDirectory() + "\\wwwroot" + "\\CRM-Document\\NJStockPriceFile";
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                if (File.Exists(Path.Combine(directory, formFile.FileName)))
                {
                    File.Delete(Path.Combine(directory, formFile.FileName));
                }
                var localFilePath = Path.Combine(directory, formFile.FileName);
                File.Copy(filePath, localFilePath);

                using (var fs = new StreamReader(localFilePath))
                {
                    var cvsReader = new CsvReader(fs, culture);
                    cvsReader.Read();
                    mutualfundNAVs = cvsReader.GetRecords<AddNJDailyPriceDto>().ToList();
                }
                var mapMutualfundNAVs = _mapper.Map<List<TblMfSchemeMaster>>(mutualfundNAVs);

                foreach (var mutualfundNAV in mapMutualfundNAVs)
                {
                    var scheme = allScheme.FirstOrDefault(x => x.SchemeName.ToLower().Equals(mutualfundNAV.SchemeName.ToLower()));
                    if (scheme is not null)
                    {
                        scheme.NetAssetValue = mutualfundNAV.NetAssetValue;
                        scheme.SchemeDate = mutualfundNAV.SchemeDate;
                        listNJPrice.Add(scheme);
                    }
                    else
                    {
                        var NJPrice = new AddNJDailyPriceDto();
                        NJPrice.SchemeName = mutualfundNAV.SchemeName;
                        NJPrice.NetAssetValue = mutualfundNAV.NetAssetValue;
                        NJPrice.SchemeDate = mutualfundNAV.SchemeDate;
                        var mapScheme = _mapper.Map<TblMfSchemeMaster>(NJPrice);
                        listNJPrice.Add(mapScheme);
                    }
                }

                return await _mutualfundRepository.UpdateMFScheme(listNJPrice);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        #endregion

        #region Import AMFI NAV/Scheme file
        public async Task<(int, string)> ImportAMFINAVFileAsync()
        {
            try
            {
                //AMFI NAV File Import
                var navFilePath = $"https://www.amfiindia.com/spages/NAVAll.txt?t={DateTime.Now.ToString("ddMMyyyy")}";
                var schemeFilePath = $"https://portal.amfiindia.com/DownloadSchemeData_Po.aspx?mf=0";

                HttpClient httpClient = _httpClientFactory.CreateClient();
                HttpResponseMessage response = await httpClient.GetAsync(navFilePath);
                var count = 0;
                List<AddAMFINAVDto> navDataDtoList = new List<AddAMFINAVDto>();
                List<TblAmfiNav> updateList = new List<TblAmfiNav>();
                var navDataListResponse = await _mutualfundRepository.GetAMFINavList(false, null, null);
                var navDataList = navDataListResponse.Item1;

                if (response.IsSuccessStatusCode)
                {
                    using (Stream stream = await response.Content.ReadAsStreamAsync())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            while (count <= 3)
                            {
                                var line = await reader.ReadLineAsync();
                                if (string.IsNullOrEmpty(line) || line == " ") count++;
                                else
                                {
                                    if (Regex.IsMatch(line, @"^[0-9]") && line.Contains(";"))
                                    {
                                        var splitLine = line.Split(";");
                                        var isin = (splitLine[1] + splitLine[2]).Replace("-", "");
                                        navDataDtoList.Add(new AddAMFINAVDto(splitLine[0], isin, splitLine[3], splitLine[4], splitLine[5]));
                                    }
                                    count = 0;
                                }
                            }
                        }
                    }

                    foreach (var data in navDataDtoList)
                    {
                        var navData = navDataList.FirstOrDefault(x => x.SchemeCode == data.SchemeCode && x.Isin == data.Isin);
                        if (navData is not null)
                        {
                            navData.NetAssetValue = data.NetAssetValue;
                            navData.Date = data.Date;
                            updateList.Add(navData);
                        }
                        else
                        {
                            updateList.Add(_mapper.Map<TblAmfiNav>(data));
                        }
                    }

                    var res = await _mutualfundRepository.UpdateAMFINav(updateList);
                }

                response = await httpClient.GetAsync(schemeFilePath);
                if (response.IsSuccessStatusCode)
                {
                    var schemesListResponse = await _mutualfundRepository.GetAMFISchemesList(false, null, null);
                    var schemesList = schemesListResponse.Item1;
                    List<TblAmfiSchemeMaster> amfiSchemeMasters = new List<TblAmfiSchemeMaster>();
                    
                    List<AddAmfiSchemeDto> schemeDtoList = new List<AddAmfiSchemeDto>();
                    
                    using (var reader = new StreamReader(response.Content.ReadAsStream()))
                    using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        // Ignore bad data
                        BadDataFound = context =>
                        {
                            Console.WriteLine($"Bad data found: {context.RawRecord}");
                        }
                    }))
                    {
                        schemeDtoList = csv.GetRecords<AddAmfiSchemeDto>().ToList();
                    }  
                    

                    foreach (var scheme in schemeDtoList)
                    {
                        var amfiScheme = schemesList.Where(x => x.SchemeCode.Equals(scheme.SchemeCode)).FirstOrDefault();
                        if (amfiScheme is not null)
                        {
                            amfiScheme.Update(scheme.Amc, scheme.SchemeName, scheme.SchemeType, scheme.SchemeCategory, scheme.SchemeNavname, scheme.SchemeMinAmt, scheme.LaunchDate, scheme.ClosureDate, scheme.Isin);
                            amfiSchemeMasters.Add(amfiScheme);
                        }
                        else
                        {
                            amfiSchemeMasters.Add(_mapper.Map<TblAmfiSchemeMaster>(scheme));
                        }
                    }

                    await _mutualfundRepository.UpdateAMFISchemes(amfiSchemeMasters);
                }

                return (1, "File imported successfully.");
            }
            catch (Exception ex)
            {
                return (0, ex.Message);
            }
        }
        #endregion
    }
}
