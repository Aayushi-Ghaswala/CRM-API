using Microsoft.AspNetCore.Http;

namespace CRM_api.Services.Dtos.AddDataDto.WBC_Mall_Module
{
    public class AddMallCategoryDto
    {
        public string CatName { get; set; } = null!;
        public bool CatActive { get; set; } = true;
        public IFormFile FormFile { get; set; }
    }
}
