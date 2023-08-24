using CRM_api.DataAccess.Helper;
using CRM_api.DataAccess.Models;
using CRM_api.DataAccess.ResponseModel.Generic_Response;

namespace CRM_api.DataAccess.IRepositories.Business_Module.Investment_Module
{
    public interface IInvestmentRepository
    {
        Task<Response<TblInvesmentType>> GetInvestmentType(string? search, SortingParams sortingParams, bool? isActive);
        Task<TblInvesmentType> GetInvestmentTypeById(int id);
        Task<Response<TblSubInvesmentType>> GetSubInvestmentType(string? search, SortingParams sortingParams, bool? isActive);
        Task<Response<TblSubInvesmentType>> GetSubInvestmentTypeByInvId(int invId, string? search, SortingParams sortingParams);
        Task<Response<TblSubsubInvType>> GetSubsubInvestmentType(string? search, SortingParams sortingParams, bool? isActive);
        Task<Response<TblSubsubInvType>> GetSubsubInvestmentTypeBySubInvId(int subInvId, string? search, SortingParams sortingParams);
        Task<List<TblSubInvesmentType>> GetAllSubInvTypes();
        Task<int> GetInvTypeByName(string invType);
        Task<int> AddInvestmentType(TblInvesmentType tblInvesmentType);
        Task<int> AddSubInvestmentType(TblSubInvesmentType tblSubInvesment);
        Task<int> AddSubsubInvestmentType(TblSubsubInvType tblSubsubInvType);
        Task<int> UpdateInvestmentType(TblInvesmentType tblInvesmentType);
        Task<int> UpdateSubInvestmentType(TblSubInvesmentType tblSubInvesmentType);
        Task<int> UpdateSubsubInvestmentType(TblSubsubInvType tblSubsubInvType);
        Task<int> DeactiveInvestmentType(int id);
        Task<int> DeactiveSubInvestmentType(int id);
        Task<int> DeactiveSubsubInvestmentType(int id);
    }
}
