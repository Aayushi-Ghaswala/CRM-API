﻿using Microsoft.AspNetCore.Http;

namespace CRM_api.Services.Dtos.AddDataDto.Business_Module.MGain_Module
{
    public class AddMGainDetailsDto
    {
        public DateTime? Date { get; set; }
        public int? MgainUserid { get; set; }
        public int? MgainEmployeeid { get; set; }
        public string? Mgain1stholder { get; set; }
        public DateTime? Mgain1stholderDob { get; set; }
        public string? Mgain1stholderGender { get; set; }
        public string? Mgain1stholderMaritalstatus { get; set; }
        public string? Mgain1stholderFathername { get; set; }
        public string? Mgain1stholderMothername { get; set; }
        public string? Mgain1stholderAddress { get; set; }
        public string? Mgain1stholderpan { get; set; }
        public string? Mgain1stholderAadhar { get; set; }
        public string? Mgain1stholderMobile { get; set; }
        public string? Mgain1stholderEmail { get; set; }
        public string? Mgain1stholderOccupation { get; set; }
        public string? Mgain1stholderStatus { get; set; }
        public IFormFile? Mgain1stholderSignatureFile { get; set; }
        public IFormFile? Mgain1stholderPanFile { get; set; }
        public IFormFile? Mgain1stholderAadharFile { get; set; }
        public IFormFile? Mgain1stholderFormCopyFile { get; set; }
        public decimal? MgainInvamt { get; set; }
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
        public IFormFile? Mgain2ndholderSignatureFile { get; set; }
        public string? MgainNomineeName { get; set; }
        public IFormFile? MgainNomineePanFile { get; set; }
        public IFormFile? MgainNomineeAadharFile { get; set; }
        public DateTime? MgainNomineeDob { get; set; }
        public IFormFile? MgainNomineeBirthCertificateFile { get; set; }
        public string? MgainGuardianName { get; set; }
        public string? MgainGuardianAddress { get; set; }
        public string? MgainGuardianMobile { get; set; }
        public int? MgainSchemeid { get; set; }
        public string? MgainType { get; set; }
        public string? MgainNomineeType { get; set; }
        public int? MgainCompanyId { get; set; }
    }
}
