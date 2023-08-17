namespace CRM_api.Services.Dtos.ResponseDto.WBC_Mall_Module
{
    public class MallCategoryDto
    {
        public int CatId { get; set; }
        public string CatName { get; set; } = null!;
        public bool CatActive { get; set; }
        public string? CatImage { get; set; }
    }
}
