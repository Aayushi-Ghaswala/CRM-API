using Microsoft.AspNetCore.Http;

namespace CRM_api.Services.IServices.Business_Module.MutualFunds_Module
{
    public interface IMutualfundService
    {
        Task<int> ImportNJClientFile(IFormFile file, bool UpdateIfExist);
    }
}
