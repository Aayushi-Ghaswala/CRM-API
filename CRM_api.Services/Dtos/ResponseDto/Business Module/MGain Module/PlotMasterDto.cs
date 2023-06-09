namespace CRM_api.Services.Dtos.ResponseDto.Business_Module.MGain_Module
{
    public class PlotMasterDto
    {
        public int Id { get; set; }
        public ProjectMasterDto TblProjectMaster { get; set; }
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
    }
}
