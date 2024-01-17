using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_api.DataAccess.Models
{
    public class TblMgainPlotData
    {
        public int Id { get; set; }
        public int MgainId { get; set; }
        public int ProjectId { get; set; }
        public int PlotId { get; set; }
        public decimal? AllocatedSqFt { get; set; }
        public decimal? AllocatedAmt { get; set; }
        public decimal? TotalSqFt { get; set; }
        public decimal? TotalPlotAmt { get; set; }
      

        [ForeignKey(nameof(MgainId))]
        public virtual TblMgaindetail TblMgainDetails { get; set; }
        [ForeignKey(nameof(ProjectId))]
        public virtual TblProjectMaster TblProjectMaster { get; set; }
        [ForeignKey(nameof(PlotId))]
        public virtual TblPlotMaster TblPlotMaster { get; set; }

    }
}
