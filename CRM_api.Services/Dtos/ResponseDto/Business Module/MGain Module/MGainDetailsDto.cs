using CRM_api.Services.Dtos.ResponseDto.HR_Module;

namespace CRM_api.Services.Dtos.ResponseDto.Business_Module.MGain_Module
{
    public class MGainDetailsDto
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public int? MgainAccountnum { get; set; }
        public string? MgainModeholder { get; set; }
        public string? Mgain1stholder { get; set; }
        public string? Mgain1stholderpan { get; set; }
        public decimal? MgainInvamt { get; set; }
        public string? MgainType { get; set; }
        public int? Tenure { get; set; }
        public ProjectMasterDto? ProjectMaster { get; set; }
        public ProjectMasterDto? SecondProjectMaster { get; set; }
        public string? MgainProjectname { get; set; }
        public PlotMasterDto? PlotMaster { get; set; }
        public PlotMasterDto? SecondPlotMaster { get; set; }
        public string? MgainPlotno { get; set; }
        public decimal? MgainAllocatedsqft { get; set; }
        public decimal? MgainAllocatedsqftamt { get; set; }
        public decimal? MgainTotalsqft { get; set; }
        public decimal? MgainTotalplotamt { get; set; }
        public string? MgainAggre { get; set; }
        public DateTime? MgainRedemdate { get; set; }
        public decimal? MgainRedemamt { get; set; }
        public UserMasterDto TblUserMaster { get; set; }
        public bool? MgainIsactive { get; set; }
        public EmployeeMasterDto EmployeeMaster { get; set; }
        public string? MgainSchemename { get; set; }
        public DateTime? Mgain1stholderDob { get; set; }
        public string? Mgain1stholderGender { get; set; }
        public string? Mgain1stholderMaritalstatus { get; set; }
        public string? Mgain1stholderFathername { get; set; }
        public string? Mgain1stholderMothername { get; set; }
        public string? Mgain1stholderAddress { get; set; }
        public string? Mgain1stholderAadhar { get; set; }
        public string? Mgain1stholderMobile { get; set; }
        public string? Mgain1stholderEmail { get; set; }
        public string? Mgain1stholderOccupation { get; set; }
        public string? Mgain1stholderStatus { get; set; }
        public string? Mgain1stholderSignature { get; set; }
        public bool? MgainIsSecondHolder { get; set; }
        public string? Mgain2ndholdername { get; set; }
        public DateTime? Mgain2ndholderDob { get; set; }
        public string? Mgain2ndholderGender { get; set; }
        public string? Mgain2ndholderMaritalStatus { get; set; }
        public string? Mgain2ndholderFatherName { get; set; }
        public string? Mgain2ndholderMotherName { get; set; }
        public string? Mgain2ndholderAddress { get; set; }
        public string? Mgain2ndholderPan { get; set; }
        public string? Mgain2ndholderAadhar { get; set; }
        public string? Mgain2ndholderMobile { get; set; }
        public string? Mgain2ndholderEmail { get; set; }
        public string? Mgain2ndholderOccupation { get; set; }
        public string? Mgain2ndholderStatus { get; set; }
        public string? Mgain2ndholderSignature { get; set; }
        public string? MgainNomineeName { get; set; }
        public string? MgainNomineePan { get; set; }
        public string? MgainNomineeAadhar { get; set; }
        public DateTime? MgainNomineeDob { get; set; }
        public string? MgainNomineeBirthCertificate { get; set; }
        public string? MgainGuardianName { get; set; }
        public string? MgainGuardianAddress { get; set; }
        public string? MgainGuardianMobile { get; set; }
        public bool? MgainIsAnotherBank { get; set; }
        public string? MgainAccHolderName { get; set; }
        public string? MgainBankName { get; set; }
        public string? MgainAccountNumber { get; set; }
        public string? MgainIfsc { get; set; }
        public string? MgainCancelledCheque { get; set; }
        public bool? MgainIsTdsDeduction { get; set; }
        public bool? MgainIsclosed { get; set; }
        public string? _15h15g { get; set; }
        public string? Mgain2ndprojectname { get; set; }
        public string? Mgain2ndplotno { get; set; }
        public decimal? Mgain2ndallocatedsqft { get; set; }
        public decimal? Mgain2ndallocatedsqftamt { get; set; }
        public decimal? Mgain2ndtotalsqft { get; set; }

        public string? Mgain1stholderAadharCard { get; set; }
        public string? Mgain1stholderPanCard { get; set; }
        public string? Mgain1stholderFormCopy { get; set; }
        public string? MgainAgreement { get; set; }

        public MGainSchemeDto TblMgainSchemeMaster { get; set; }

        public List<MGainPaymentDto> TblMgainPaymentMethods { get; set; }
    }
}
