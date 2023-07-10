using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_api.DataAccess.Models
{
    public class TblCampaignMaster
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SourceTypeId { get; set; }
        public int SourceId { get; set; }
        public int StatusId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set;}
        public decimal Budget { get; set; }
        public decimal RevenueExpected { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        
        [ForeignKey(nameof(UserId))]
        public virtual TblUserMaster TblUserMaster { get; set; }

        [ForeignKey(nameof(SourceTypeId))]
        public virtual TblSourceTypeMaster TblSourceTypeMaster { get; set; }

        [ForeignKey(nameof(SourceId))]
        public virtual TblSourceMaster TblSourceMaster { get; set; }

        [ForeignKey(nameof(StatusId))]
        public virtual TblStatusMaster TblStatusMaster { get; set; }
    }
}
