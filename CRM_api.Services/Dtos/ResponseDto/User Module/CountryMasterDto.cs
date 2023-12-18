namespace CRM_api.Services.Dtos.ResponseDto
{
    public class CountryMasterDto
    {
        public int CountryId { get; set; }
        public string? CountryName { get; set; }
        public string? Isdcode { get; set; }
        public string? Icon { get; set; }
        public string? CountryIsdCode { get; set; }
    }
}
