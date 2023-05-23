using CRM_api.Services.IServices.Business_Module.MutualFunds_Module;
using Microsoft.AspNetCore.Mvc;

namespace CRM_api.Controllers.Business_Module.MutualFunds_Module
{
    [Route("api/[controller]")]
    [ApiController]
    public class MutualfundController : ControllerBase
    {
        private readonly IMutualfundService _mutualfundService;

        public MutualfundController(IMutualfundService mutualfundService)
        {
            _mutualfundService = mutualfundService;
        }

        [HttpPost]
        #region Import Excel File
        public async Task<IActionResult> ImportExcel(IFormFile file, bool UpdateIfExist)
        {
            try
            {
                var flag = await _mutualfundService.AddNJMutualfundDetails(file, UpdateIfExist);

                return (flag == 0) ? Ok("File Import Successfully") : BadRequest("unable To Import File");
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        
    }
}
