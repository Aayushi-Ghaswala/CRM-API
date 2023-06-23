namespace CRM_api.Services.Dtos.ResponseDto.Business_Module.Dashboard
{
    public class ClientMonthlyTransSnapshotDto
    {
        public decimal? AvgTrading { get; set; } = 0;
        public decimal? AvgDelivery { get; set; } = 0;
        public decimal? MFSip { get; set; } = 0;
        public decimal? LIPremium { get; set; } = 0;
        public decimal? GIPremium { get; set; } = 0;
    }
}
