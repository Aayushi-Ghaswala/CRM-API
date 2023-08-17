namespace CRM_api.Services.Dtos.ResponseDto.WBC_Mall_Module
{
    public class ProductImageDto
    {
        public int Id { get; set; }
        public string Img { get; set; } = null!;
        public int ProductId { get; set; }
        public DateTime CeratedDate { get; set; }
        public int? Modifyby { get; set; }
        public DateTime? ModifyDate { get; set; }
        public bool Isdeleted { get; set; }
    }
}
