namespace CRM_api.Services.Dtos.ResponseDto.Business_Module.Dashboard
{
    public class ClientCurrentInvSnapshotDto
    {
        public decimal? Stocks { get; set; }
        public decimal? MutualFunds { get; set; }
        public decimal? MGain { get; set; }
        public decimal? Assets { get; set; }
        public int? LI { get; set; }
        public int? GI { get; set; }
    }
}
