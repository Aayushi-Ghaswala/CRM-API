namespace CRM_api.Services.Dtos.ResponseDto.Business_Module.Fasttrack_Module
{
    public class FasttrackResponseDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string? UserLevel { get; set; }
        public decimal? Commission { get; set; } = 0;
        public int? GoldPoint { get; set; } = 0;
    }
}
