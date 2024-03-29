﻿using CRM_api.DataAccess.Models;
using CRM_api.Services.Dtos.ResponseDto.User_Module;
using System.ComponentModel.DataAnnotations;

namespace CRM_api.Services.Dtos.ResponseDto
{
    public class UserMasterDto
    {
        [Key]
        public int UserId { get; set; }
        public UserCategoryDto TblUserCategoryMaster { get; set; }
        public UserNameDto ParentName { get; set; }
        public UserNameDto SponserName { get; set; }
        public int? UserParentid { get; set; }
        public string? UserName { get; set; }
        public string? UserPan { get; set; }
        public DateTime? UserDoj { get; set; }
        public string? UserMobile { get; set; }
        public string? UserEmail { get; set; }
        public string? UserWorkemail { get; set; }
        public string? UserAddr { get; set; }
        public string? UserPin { get; set; }
        public int? UserCountryid { get; set; }
        public CountryMasterDto TblCountryMaster { get; set; }
        public StateMasterDto TblStateMaster { get; set; }
        public CityMasterDto TblCityMaster { get; set; }
        public string? UserUname { get; set; }
        public string? UserPasswd { get; set; }
        public bool? UserIsactive { get; set; }
        public int? UserPurposeid { get; set; }
        public string? UserProfilephoto { get; set; }
        public string? UserPromocode { get; set; }
        public string? UserSubcategory { get; set; }
        public string? UserGstno { get; set; }
        public string? UserFcmid { get; set; }
        public DateTime? UserFcmlastupdaetime { get; set; }
        public DateTime? UserDob { get; set; }
        public string? UserAadhar { get; set; }
        public int? UserAccounttype { get; set; }
        public bool? UserFasttrack { get; set; }
        public bool? UserWbcActive { get; set; }
        public int? Totalcountofaddcontact { get; set; }
        public string? UserDeviceid { get; set; }
        public bool? UserTermandcondition { get; set; }
        public int? FamilyId { get; set; }
        public string? UserNjname { get; set; }
        public DateTime? FastTrackActivationDate { get; set; }
        public string UserFasttrackCategory { get; set; }
        public List<UserRoleAssignmentDto> TblRoleAssignments { get; set; }
        public string? UserAadharPath { get; set; }
        public string? UserImagePath { get; set; }
        public string? UserPanPath { get; set; }
        public ICollection<TblAccountMaster>? TblAccountMasters { get; set; }
    }
}
