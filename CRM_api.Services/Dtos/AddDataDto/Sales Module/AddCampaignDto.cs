using CRM_api.DataAccess.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_api.Services.Dtos.AddDataDto.Sales_Module
{
    public class AddCampaignDto
    {
        public int UserId { get; set; }
        public int SourceTypeId { get; set; }
        public int SourceId { get; set; }
        public int StatusId { get; set; }
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
