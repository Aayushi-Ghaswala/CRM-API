namespace CRM_api.Services.Dtos.ResponseDto.Business_Module.Dashboard
{
    public class ClientMonthlyTransSnapshotDto
    {
        public decimal? Trading { get; set; } = 0;
        public decimal? Delivery { get; set; } = 0;
        public decimal? MFSip { get; set; } = 0;
        public decimal? MFLumpsum { get; set; } = 0;
        public decimal? LIPremium { get; set; } = 0;
        public decimal? GIPremium { get; set; } = 0;
    }
}
