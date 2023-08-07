namespace CRM_api.Services.Dtos.AddDataDto.User_Module
{
    public class UpdateUserCategoryDto
    {
        public int CatId { get; set; }
        public string? CatName { get; set; }
        public bool CatIsactive { get; set; } = true;
    }
}
