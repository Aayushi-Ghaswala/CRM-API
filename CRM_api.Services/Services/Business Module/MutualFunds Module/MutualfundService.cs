using AutoMapper;
using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.IRepositories.Business_Module.MutualFunds_Module;
using CRM_api.DataAccess.IRepositories.User_Module;
using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.AddDataDto.Business_Module.MutualFunds_Module;
using CRM_api.Services.Dtos.ResponseDto.Business_Module.MutualFunds_Module;
using CRM_api.Services.Dtos.ResponseDto.Generic_Response;
using CRM_api.Services.Dtos.ResponseDto.User_Module;
using CRM_api.Services.IServices.Business_Module.MutualFunds_Module;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using System.Data;

namespace CRM_api.Services.Services.Business_Module.MutualFunds_Module
{
    public class MutualfundService : IMutualfundService
    {
        private readonly IMutualfundRepository _mutualfundRepository;
        private readonly IMapper _mapper;
        private readonly IUserMasterRepository _userMasterRepository;

        public MutualfundService(IMutualfundRepository mutualfundReposiory, IMapper mapper, IUserMasterRepository userMasterRepository)
        {
            _mutualfundRepository = mutualfundReposiory;
            _mapper = mapper;
            _userMasterRepository = userMasterRepository;
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
        public async Task<MFTransactionDto<MFSummaryDto>> GetMFSummaryAsync(int userId, string? searchingParams, SortingParams sortingParams)
        {
            List<MFSummaryDto> mutualFundSummaries = new List<MFSummaryDto>();

            double pageCount = 0;
            var mfSummary = await _mutualfundRepository.GetMFTransactionSummary(userId);

            foreach (var records in mfSummary)
            {
                var mfSummaryDto = new MFSummaryDto();
                decimal? redemptionUnit = 0;
                decimal? totalPurchaseUnits = 0;

                mfSummaryDto.NAV = Math.Round((double)records.Average(x => x.Nav), 3);
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
                mfSummaryDto.TotalRedemptionUnit = Math.Round((decimal)(mfSummaryDto.TotalPurchaseUnit - redemptionUnit), 3);
                mfSummaryDto.BalanceUnit = Math.Round((decimal)mfSummaryDto.TotalRedemptionUnit, 3);
                mfSummaryDto.CurrentValue = Math.Round((decimal)(mfSummaryDto.BalanceUnit * (decimal)mfSummaryDto.NAV), 3);

                mutualFundSummaries.Add(mfSummaryDto);
            }
            IQueryable<MFSummaryDto> mutualFundSummaryDto = mutualFundSummaries.AsQueryable();

            var totalBalanceUnit = mutualFundSummaries.Sum(x => x.BalanceUnit);
            var totalAmount = mutualFundSummaries.Sum(x => x.CurrentValue);

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
            };

            return mutualfundResponse;
        }
        #endregion

        #region Get Client wise MF Summary Category Wise
        public async Task<MFTransactionDto<MFCategoryWiseDto>> GetMFCategoryWiseAsync(int userId, string? searchingParams, SortingParams sortingParams)
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
                mfCategoryWise.TotalRedemptionUnit = Math.Round((decimal)(mfCategoryWise.TotalPurchaseUnit - redemptionUnit), 3);
                mfCategoryWise.BalanceUnit = Math.Round((decimal)mfCategoryWise.TotalRedemptionUnit, 3);
                mfCategoryWise.CurrentValue = Math.Round((decimal)(mfCategoryWise.BalanceUnit * (decimal)mfCategoryWise.NAV), 3);

                mutualFundSummaries.Add(mfCategoryWise);
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
        public async Task<MFTransactionDto<AllClientMFSummaryDto>> GetAllClientMFSummaryAsync(DateTime fromDate, DateTime toDate, string? searchingParams, SortingParams sortingParams)
        {
            List<AllClientMFSummaryDto> mutualFundSummaries = new List<AllClientMFSummaryDto>();

            double pageCount = 0;
            var mfSummary = await _mutualfundRepository.GetAllCLientMFSummary(fromDate, toDate);

            foreach (var records in mfSummary)
            {
                var allClientMFSummary = new AllClientMFSummaryDto();
                decimal? redemptionUnit = 0;
                decimal? totalPurchaseUnits = 0;

                allClientMFSummary.NAV = Math.Round((double)records.Average(x => x.Nav), 3);
                allClientMFSummary.Userid = records.DistinctBy(x => x.Userid).Select(x => x.Userid).FirstOrDefault();
                allClientMFSummary.Username = records.Key;

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

                allClientMFSummary.TotalPurchaseUnit = Math.Round((decimal)totalPurchaseUnits, 3);
                allClientMFSummary.TotalRedemptionUnit = Math.Round((decimal)(allClientMFSummary.TotalPurchaseUnit - redemptionUnit), 3);
                allClientMFSummary.BalanceUnit = Math.Round((decimal)allClientMFSummary.TotalRedemptionUnit, 3);
                allClientMFSummary.CurrentValue = Math.Round((decimal)(allClientMFSummary.BalanceUnit * (decimal)allClientMFSummary.NAV), 3);

                mutualFundSummaries.Add(allClientMFSummary);
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

            foreach(var user in mapMFUser.Values)
            {
                user.UserName = user.UserName.ToLower();
            }

            return mapMFUser;
        }
        #endregion 

        #region Display SchemeName
        public async Task<ResponseDto<SchemaNameDto>> DisplayschemeNameAsync(int userId, string? searchingParams, SortingParams sortingParams)
        {
            var mutualfunds = await _mutualfundRepository.GetSchemeName(userId, searchingParams, sortingParams);
            var schemeName = _mapper.Map<ResponseDto<SchemaNameDto>>(mutualfunds);

            foreach(var scheme in schemeName.Values)
            {
                scheme.Schemename = scheme.Schemename.ToLower();
            }

            return schemeName;
        }
        #endregion

        #region Display Folio Number List
        public async Task<ResponseDto<SchemaNameDto>> DisplayFolioNoAsync(int userId, int? schemeId, string? searchingParams, SortingParams sortingParams)
        {
            var mutualfunds = await _mutualfundRepository.GetFolioNo(userId, schemeId, searchingParams, sortingParams);
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

            var directoryPath = Directory.GetCurrentDirectory() + "\\CRM-Document\\NJCLientFile";

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

                        var datatable = dataset.Tables[0];

                        var records = datatable.AsEnumerable().SkipLast(5).ToList().ConvertAll<AddMutualfundsDto>(row =>
                        {
                            var obj = new AddMutualfundsDto();

                            obj.Username = row["Investor"] == DBNull.Value ? null : row["Investor"].ToString();
                            obj.Transactiontype = row["Type"] == DBNull.Value ? null : row["Type"].ToString();
                            obj.Schemename = row["Scheme"] == DBNull.Value ? null : row["Scheme"].ToString();
                            obj.Foliono = row["Folio No/Demat A/C"] == DBNull.Value ? null : row["Folio No/Demat A/C"].ToString();
                            obj.Tradeno = row["Tr. No."] == DBNull.Value ? null : row["Tr. No."].ToString();
                            obj.Date = Convert.ToDateTime(row["Purchase Date"] == DBNull.Value ? null : row["Purchase Date"]);
                            obj.Nav = Convert.ToDouble(row["NAV(₹)"] == DBNull.Value ? null : row["NAV(₹)"]);
                            obj.Noofunit = Convert.ToDecimal(row["Purchase units"] == DBNull.Value ? null : row["Purchase units"]);
                            obj.Invamount = Convert.ToDecimal(row["Inv. Amount(₹)"] == DBNull.Value ? null : row["Inv. Amount(₹)"]);
                            obj.Userpan = row["PAN"] == DBNull.Value ? null : row["PAN"].ToString();

                            var userId = _userMasterRepository.GetUserIdByUserPan(obj.Userpan);
                            var schemeId = _mutualfundRepository.GetSchemeIdBySchemeName(obj.Schemename);

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
                                                            mapRecordsForExistUser.First().Date, mapRecordsForExistUser.First().Date);

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
    }
}
