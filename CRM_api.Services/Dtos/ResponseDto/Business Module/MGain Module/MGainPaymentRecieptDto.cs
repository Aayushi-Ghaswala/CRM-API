namespace CRM_api.Services.Dtos.ResponseDto.Business_Module.MGain_Module
{
    public class MGainPaymentRecieptDto
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public string? Mgain1stholder { get; set; }
        public string? Mgain1stholderpan { get; set; }
        public string? Mgain1stholderAddress { get; set; }
        public string? Mgain1stholderMobile { get; set; }
        public string? Mgain1stholderEmail { get; set; }
        public decimal? MgainInvamt { get; set; }
        public string? MgainType { get; set; }
        public string? Mgain2ndholdername { get; set; }
        public string? MgainNomineeName { get; set; }
        public string? MgainNomineePan { get; set; }
        public DateTime ReleaseDate { get; set; }
        public decimal? AverageROI { get; set; }
 
        public MGainSchemeDto TblMgainSchemeMaster { get; set; }
        public List<MGainPaymentDto> TblMgainPaymentMethods { get; set; }
    }
}
