using CRM_api.DataAccess.Models;

namespace CRM_api.Services.Dtos.ResponseDto.WBC_Mall_Module
{
    public class MallProductDto
    {
        public int ProdId { get; set; }
        public string ProdName { get; set; } = null!;
        public MallCategoryDto TblWbcMallCategory { get; set; }
        public DateTime ProdDateAdded { get; set; }
        public int? ProdDiscount { get; set; }
        public string? ProdImage { get; set; }
        public decimal? ProdRating { get; set; }
        public string? Description { get; set; }
        public int? ProdAvailableQty { get; set; }
        public decimal? ProdPrice { get; set; }
        public decimal? GoldPointPrice { get; set; }
        public string? ManagedBy { get; set; }
        public bool? IsShowInApp { get; set; }
        public virtual List<ProductImageDto> TblProductImgs { get; set; }
    }
}
