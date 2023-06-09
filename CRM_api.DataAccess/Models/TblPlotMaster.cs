using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_api.DataAccess.Models
{
    public partial class TblPlotMaster
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

        [ForeignKey(nameof(ProjectId))]
        public virtual TblProjectMaster TblProjectMaster { get; set; }
    }
}
