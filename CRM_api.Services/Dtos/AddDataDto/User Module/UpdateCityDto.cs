namespace CRM_api.Services.Dtos.AddDataDto.User_Module
{
    public class UpdateCityDto
    {
        public int CityId { get; set; }
        public int? StateId { get; set; }
        public string? CityName { get; set; }
        public bool IsDeleted { get; set; }
    }
}
