using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;
using CRM_api.DataAccess.ResponseModel.Stocks_Module;

namespace CRM_api.DataAccess.IRepositories.Business_Module.Stocks_Module
{
    public interface IStocksRepository
    {
        Task<Response<UserNameResponse>> GetStocksUsersName(string? searchingParams, SortingParams sortingParams);
        Task<Response<TblStockData>> GetAllScriptNames(string clientName, string? searchingParams, SortingParams sortingParams);
        Task<StocksResponse<TblStockData>> GetStocksTransactions(string clientName, DateTime? fromDate, DateTime? toDate, string scriptName, string firmName, string? searchingParams, SortingParams sortingParams);
        Task<List<TblStockData>> GetStockDataForSpecificDateRange(DateTime? startDate, DateTime? endDate, string firmName);
        Task<int> AddData(List<TblStockData> tblStockData);
        Task<int> DeleteData(List<TblStockData> tblStockData);
    }
}
