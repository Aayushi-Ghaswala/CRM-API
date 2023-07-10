using CRM_api.DataAccess.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_api.Services.Dtos.ResponseDto.Sales_Module
{
    public class CampaignDto
    {
        public int Id { get; set; }
        public TblUserMaster TblUserMaster { get; set; }
        public TblSourceTypeMaster TblSourceTypeMaster { get; set; }
        public TblSourceMaster TblSourceMaster { get; set; }
        public TblStatusMaster TblStatusMaster { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Budget { get; set; }
        public decimal RevenueExpected { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
