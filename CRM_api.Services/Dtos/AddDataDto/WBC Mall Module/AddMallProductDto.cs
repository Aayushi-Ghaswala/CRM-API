using CRM_api.DataAccess.Models;
using Microsoft.AspNetCore.Http;

namespace CRM_api.Services.Dtos.AddDataDto.WBC_Mall_Module
{
    public class AddMallProductDto
    {
        public string ProdName { get; set; } = null!;
        public int ProdCatId { get; set; }
        public int? ProdDiscount { get; set; }
        public IFormFile FormFile { get; set; }
        public decimal? ProdRating { get; set; }
        public string? Description { get; set; }
        public int? ProdAvailableQty { get; set; }
        public decimal? ProdPrice { get; set; }
        public decimal? GoldPointPrice { get; set; }
        public string? ManagedBy { get; set; }
        public bool? IsShowInApp { get; set; }
        public virtual IFormFileCollection FormFiles { get; set; }
    }
}
