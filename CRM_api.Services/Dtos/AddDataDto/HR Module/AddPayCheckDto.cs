namespace CRM_api.Services.Dtos.AddDataDto.HR_Module
{
    public class AddPayCheckDto
    {
        public int? DesignationId { get; set; }
        public long? Basic { get; set; }
        public decimal? DA { get; set; }
        public decimal? HRA { get; set; }
        public decimal? Medical { get; set; }
        public decimal? PF { get; set; }
        public decimal? ESIC { get; set; }
        public decimal? Prof_Tax { get; set; }
        public int? Bonus { get; set; }
        public decimal? Tds { get; set; }
        public int? Special_Allowance { get; set; }
        public long? Net_Salary { get; set; }
        public bool? IsDeleted { get; set; } = false;
    }
}
