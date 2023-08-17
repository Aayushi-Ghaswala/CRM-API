using Microsoft.AspNetCore.Http;

namespace CRM_api.Services.Dtos.AddDataDto.WBC_Mall_Module
{
    public class UpdateMallCategoryDto
    {
        public int CatId { get; set; }
        public string CatName { get; set; } = null!;
        public bool CatActive { get; set; } = true;
        public string? CatImage { get; set; }
        public IFormFile? FormFile { get; set; }
    }
}
