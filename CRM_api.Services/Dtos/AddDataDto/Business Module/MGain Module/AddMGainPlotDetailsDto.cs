using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_api.Services.Dtos.AddDataDto.Business_Module.MGain_Module
{
    public class AddMGainPlotDetailsDto
    {
        public int? Id { get; set; }
        public int? MgainId { get; set; }
        public int? ProjectId { get; set; }
        public int? PlotId { get; set; }
        public decimal? AllocatedSqFt { get; set; }
        public decimal? AllocatedAmt { get; set; }
        public decimal? TotalSqFt { get; set; }
        public decimal? TotalPlotAmt { get; set; }

    }
}
