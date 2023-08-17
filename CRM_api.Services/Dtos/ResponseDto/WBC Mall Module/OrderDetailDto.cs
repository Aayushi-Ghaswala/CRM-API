using CRM_api.DataAccess.Models;

namespace CRM_api.Services.Dtos.ResponseDto.WBC_Mall_Module
{
    public class OrderDetailDto
    {
        public int Id { get; set; }
        public MallProductDto Product { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal Price { get; set; }
        public string Sku { get; set; } = null!;
        public int Quantity { get; set; }
    }
}
