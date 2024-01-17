using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_api.Services.Dtos.ResponseDto.Business_Module.MGain_Module
{
    public class MGainPlotDto
    {
        public int Id { get; set; }
        public int MgainId { get; set; }
        public int ProjectId { get; set; }
        public int PlotId { get; set; }
        public decimal? AllocatedSqFt { get; set; }
        public decimal? AllocatedAmt { get; set; }
        public decimal? TotalSqFt { get; set; }
        public decimal? TotalPlotAmt { get; set; }

        public ProjectMasterDto? TblProjectMaster { get; set; }
        public PlotMasterDto? TblPlotMaster { get; set; }
    }

    public class MGainPlotDetailsDto
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string PlotNo { get; set; }
        public decimal? AllocatedSqFt { get; set; }
        public decimal? AllocatedAmt { get; set; }
        public int MgainId { get; set; }
        public int ProjectId { get; set; }
        public int PlotId { get; set; }

    }
}
