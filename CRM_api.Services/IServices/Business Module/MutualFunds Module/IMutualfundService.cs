using Microsoft.AspNetCore.Http;

namespace CRM_api.Services.IServices.Business_Module.MutualFunds_Module
{
    public interface IMutualfundService
    {
        Task<int> AddNJMutualfundDetails(IFormFile file, bool UpdateIfExist);

    }
}
