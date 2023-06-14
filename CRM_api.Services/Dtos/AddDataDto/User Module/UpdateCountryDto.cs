namespace CRM_api.Services.Dtos.AddDataDto.User_Module
{
    public class UpdateCountryDto
    {
        public int CountryId { get; set; }
        public string? CountryName { get; set; }
        public string? Isdcode { get; set; }
        public string? Icon { get; set; }
        public bool IsDeleted { get; set; }
    }
}
