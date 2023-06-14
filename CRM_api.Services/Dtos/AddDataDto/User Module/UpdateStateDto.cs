namespace CRM_api.Services.Dtos.AddDataDto.User_Module
{
    public class UpdateStateDto
    {
        public int StateId { get; set; }
        public int? CountryId { get; set; }
        public string? StateName { get; set; }
        public bool IsDeleted { get; set; }
    }
}
