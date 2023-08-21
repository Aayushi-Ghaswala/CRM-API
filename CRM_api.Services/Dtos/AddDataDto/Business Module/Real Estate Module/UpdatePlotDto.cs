namespace CRM_api.Services.Dtos.AddDataDto.Business_Module.Real_Estate_Module
{
    public class UpdatePlotDto
    {
        public int Id { get; set; }
        public int? ProjectId { get; set; }
        public string? PlotNo { get; set; }
        public decimal? SqMt { get; set; }
        public decimal? SqFt { get; set; }
        public decimal? Yard { get; set; }
        public decimal? WidthFt { get; set; }
        public decimal? HeightFt { get; set; }
        public decimal? Rate { get; set; }
        public decimal? PlotValue { get; set; }
        public decimal? Available_SqFt { get; set; }
        public decimal? Available_PlotValue { get; set; }
        public decimal? FasttrackCommission { get; set; }
        public string? Purpose { get; set; }
    }
}
