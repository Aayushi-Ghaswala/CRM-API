namespace CRM_api.Services.Dtos.ResponseDto.Business_Module.Dashboard
{
    public class ClientCurrentInvSnapshotDto
    {
        public decimal? Stocks { get; set; } = 0;
        public decimal? MutualFunds { get; set; } = 0;
        public decimal? MGain { get; set; } = 0;
        public decimal? Assets { get; set; } = 0;
        public int? LI { get; set; } = 0;
        public int? GI { get; set; } = 0;
    }
}
