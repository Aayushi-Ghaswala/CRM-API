﻿using CRM_api.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace CRM_api.DataAccess.Context
{
    public partial class CRMDbContext : DbContext
    {
        public CRMDbContext()
        {
        }

        public CRMDbContext(DbContextOptions<CRMDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cleanuser> Cleanusers { get; set; } = null!;
        public virtual DbSet<Sheet2> Sheet2s { get; set; } = null!;
        public virtual DbSet<Tbl5paisaResponse> Tbl5paisaResponses { get; set; } = null!;
        public virtual DbSet<TblAccountMaster> TblAccountMasters { get; set; } = null!;
        public virtual DbSet<TblBankMaster> TblBankMasters { get; set; } = null!;
        public virtual DbSet<TblCityMaster> TblCityMasters { get; set; } = null!;
        public virtual DbSet<TblContactMaster> TblContactMasters { get; set; } = null!;
        public virtual DbSet<TblCountryMaster> TblCountryMasters { get; set; } = null!;
        public virtual DbSet<TblDepartmentMaster> TblDepartmentMasters { get; set; } = null!;
        public virtual DbSet<TblDesignationMaster> TblDesignationMasters { get; set; } = null!;
        public virtual DbSet<TblEmisipCalculator> TblEmisipCalculators { get; set; } = null!;
        public virtual DbSet<TblExcelimportUsermaster> TblExcelimportUsermasters { get; set; } = null!;
        public virtual DbSet<TblFamilyMember> TblFamilyMembers { get; set; } = null!;
        public virtual DbSet<TblFaq> TblFaqs { get; set; } = null!;
        public virtual DbSet<TblFasttrackLedger> TblFasttrackLedgers { get; set; } = null!;
        public virtual DbSet<TblFasttrackSchemeMaster> TblFasttrackSchemeMasters { get; set; } = null!;
        public virtual DbSet<TblFasttrackSubscription> TblFasttrackSubscriptions { get; set; } = null!;
        public virtual DbSet<TblFolioDetail> TblFolioDetails { get; set; } = null!;
        public virtual DbSet<TblFolioMaster> TblFolioMasters { get; set; } = null!;
        public virtual DbSet<TblFolioTypeMaster> TblFolioTypeMasters { get; set; } = null!;
        public virtual DbSet<TblFyersResponse> TblFyersResponses { get; set; } = null!;
        public virtual DbSet<TblGoldPoint> TblGoldPoints { get; set; } = null!;
        public virtual DbSet<TblGoldPointCategory> TblGoldPointCategories { get; set; } = null!;
        public virtual DbSet<TblGoldReferral> TblGoldReferrals { get; set; } = null!;
        public virtual DbSet<TblInsuranceCalculator> TblInsuranceCalculators { get; set; } = null!;
        public virtual DbSet<TblInsuranceCompanylist> TblInsuranceCompanylists { get; set; } = null!;
        public virtual DbSet<TblInsuranceTypeMaster> TblInsuranceTypeMasters { get; set; } = null!;
        public virtual DbSet<TblInsuranceclient> TblInsuranceclients { get; set; } = null!;
        public virtual DbSet<TblInsurancetype> TblInsurancetypes { get; set; } = null!;
        public virtual DbSet<TblInvesmentType> TblInvesmentTypes { get; set; } = null!;
        public virtual DbSet<TblLeaveType> TblLeaveTypes { get; set; } = null!;
        public virtual DbSet<TblMfSchemeMaster> TblMfSchemeMasters { get; set; } = null!;
        public virtual DbSet<TblMftransaction> TblMftransactions { get; set; } = null!;
        public virtual DbSet<TblMgainInvesment> TblMgainInvesments { get; set; } = null!;
        public virtual DbSet<TblMgainLedger> TblMgainLedgers { get; set; } = null!;
        public virtual DbSet<TblMgainSchemeMaster> TblMgainSchemeMasters { get; set; } = null!;
        public virtual DbSet<TblMgainTransactionAccountTem> TblMgainTransactionAccountTems { get; set; } = null!;
        public virtual DbSet<TblMgaindetail> TblMgaindetails { get; set; } = null!;
        public virtual DbSet<TblMgaindetailsTruncate> TblMgaindetailsTruncates { get; set; } = null!;
        public virtual DbSet<TblMunafeKiClass> TblMunafeKiClasses { get; set; } = null!;
        public virtual DbSet<TblNotexistuserMftransaction> TblNotexistuserMftransactions { get; set; } = null!;
        public virtual DbSet<TblNotfoundInsuranceclient> TblNotfoundInsuranceclients { get; set; } = null!;
        public virtual DbSet<TblOfferMaster> TblOfferMasters { get; set; } = null!;
        public virtual DbSet<TblOrder> TblOrders { get; set; } = null!;
        public virtual DbSet<TblOrderDetail> TblOrderDetails { get; set; } = null!;
        public virtual DbSet<TblPortfolioReviewRequest> TblPortfolioReviewRequests { get; set; } = null!;
        public virtual DbSet<TblProductBanner> TblProductBanners { get; set; } = null!;
        public virtual DbSet<TblProductImg> TblProductImgs { get; set; } = null!;
        public virtual DbSet<TblRealEastateReview> TblRealEastateReviews { get; set; } = null!;
        public virtual DbSet<TblRealEastateReviewImg> TblRealEastateReviewImgs { get; set; } = null!;
        public virtual DbSet<TblReferralMaster> TblReferralMasters { get; set; } = null!;
        public virtual DbSet<TblRequestDetail> TblRequestDetails { get; set; } = null!;
        public virtual DbSet<TblRetirementCalculator> TblRetirementCalculators { get; set; } = null!;
        public virtual DbSet<TblRoleAssignment> TblRoleAssignments { get; set; } = null!;
        public virtual DbSet<TblRoleMaster> TblRoleMasters { get; set; } = null!;
        public virtual DbSet<TblRolePermission> TblRolePermissions { get; set; } = null!;
        public virtual DbSet<TblScripMaster> TblScripMasters { get; set; } = null!;
        public virtual DbSet<TblSegmentMaster> TblSegmentMasters { get; set; } = null!;
        public virtual DbSet<TblSipCalculator> TblSipCalculators { get; set; } = null!;
        public virtual DbSet<TblStateMaster> TblStateMasters { get; set; } = null!;
        public virtual DbSet<TblStockData> TblStockData { get; set; } = null!;
        public virtual DbSet<TblSubInvesmentType> TblSubInvesmentTypes { get; set; } = null!;
        public virtual DbSet<TblSubsubInvType> TblSubsubInvTypes { get; set; } = null!;
        public virtual DbSet<TblTempUserpan> TblTempUserpans { get; set; } = null!;
        public virtual DbSet<TblTermsCondition> TblTermsConditions { get; set; } = null!;
        public virtual DbSet<TblUserCategoryMaster> TblUserCategoryMasters { get; set; } = null!;
        public virtual DbSet<TblUserDepartment> TblUserDepartments { get; set; } = null!;
        public virtual DbSet<TblUserMaster> TblUserMasters { get; set; } = null!;
        public virtual DbSet<TblVendorMaster> TblVendorMasters { get; set; } = null!;
        public virtual DbSet<TblWbcMallCategory> TblWbcMallCategories { get; set; } = null!;
        public virtual DbSet<TblWbcMallProduct> TblWbcMallProducts { get; set; } = null!;
        public virtual DbSet<TblWbcSchemeMaster> TblWbcSchemeMasters { get; set; } = null!;
        public virtual DbSet<TblWbcTypeMaster> TblWbcTypeMasters { get; set; } = null!;
        public virtual DbSet<Usercleantable> Usercleantables { get; set; } = null!;
        public virtual DbSet<TblLoanMaster> TblLoanMasters { get; set; } = null!;
        public virtual DbSet<TblLoanTypeMaster> TblLoanTypeMasters { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=DefaultConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("kaadmin");

            modelBuilder.Entity<Cleanuser>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("cleanuser");

                entity.Property(e => e.CatId).HasColumnName("cat_id");

                entity.Property(e => e.UseAdhar)
                    .HasMaxLength(255)
                    .HasColumnName("use_adhar");

                entity.Property(e => e.UserAdd)
                    .HasMaxLength(255)
                    .HasColumnName("user_add");

                entity.Property(e => e.UserAddline1)
                    .HasMaxLength(255)
                    .HasColumnName("user_addline1");

                entity.Property(e => e.UserAddline2)
                    .HasMaxLength(255)
                    .HasColumnName("user_addline2");

                entity.Property(e => e.UserAddline3)
                    .HasMaxLength(255)
                    .HasColumnName("user_addline3");

                entity.Property(e => e.UserCityid).HasColumnName("user_cityid");

                entity.Property(e => e.UserCityname)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("user_cityname");

                entity.Property(e => e.UserCountryid).HasColumnName("user_countryid");

                entity.Property(e => e.UserDob)
                    .HasMaxLength(255)
                    .HasColumnName("user_dob");

                entity.Property(e => e.UserEmail)
                    .HasMaxLength(255)
                    .HasColumnName("user_email");

                entity.Property(e => e.UserFirstname)
                    .HasMaxLength(255)
                    .HasColumnName("user_firstname");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.UserLastname)
                    .HasMaxLength(255)
                    .HasColumnName("user_lastname");

                entity.Property(e => e.UserMiddlename)
                    .HasMaxLength(255)
                    .HasColumnName("user_middlename");

                entity.Property(e => e.UserMobile)
                    .HasMaxLength(255)
                    .HasColumnName("user_mobile");

                entity.Property(e => e.UserName)
                    .HasMaxLength(255)
                    .HasColumnName("user_name");

                entity.Property(e => e.UserPan)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("user_pan");

                entity.Property(e => e.UserParentid).HasColumnName("user_parentid");

                entity.Property(e => e.UserPasswd)
                    .HasMaxLength(255)
                    .HasColumnName("user_passwd");

                entity.Property(e => e.UserPin).HasColumnName("user_pin");

                entity.Property(e => e.UserPromocode)
                    .HasMaxLength(255)
                    .HasColumnName("user_promocode");

                entity.Property(e => e.UserSponid).HasColumnName("user_sponid");

                entity.Property(e => e.UserStateid).HasColumnName("user_stateid");

                entity.Property(e => e.UserStatename)
                    .HasMaxLength(255)
                    .HasColumnName("user_Statename");

                entity.Property(e => e.UserSubcategory).HasColumnName("user_subcategory");

                entity.Property(e => e.UserUname)
                    .HasMaxLength(255)
                    .HasColumnName("user_uname");
            });

            modelBuilder.Entity<Sheet2>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Sheet2$", "dbo");

                entity.Property(e => e.CatId).HasColumnName("cat_id");

                entity.Property(e => e.FamilyId)
                    .HasMaxLength(255)
                    .HasColumnName("family_id");

                entity.Property(e => e.Totalcountofaddcontact).HasMaxLength(255);

                entity.Property(e => e.UserAadhar).HasColumnName("user_aadhar");

                entity.Property(e => e.UserAccounttype)
                    .HasMaxLength(255)
                    .HasColumnName("user_accounttype");

                entity.Property(e => e.UserAddr)
                    .HasMaxLength(255)
                    .HasColumnName("user_addr");

                entity.Property(e => e.UserCityid).HasColumnName("user_cityid");

                entity.Property(e => e.UserCityname)
                    .HasMaxLength(255)
                    .HasColumnName("user_cityname");

                entity.Property(e => e.UserCountryid).HasColumnName("user_countryid");

                entity.Property(e => e.UserCountryname)
                    .HasMaxLength(255)
                    .HasColumnName("user_countryname");

                entity.Property(e => e.UserDeviceid)
                    .HasMaxLength(255)
                    .HasColumnName("user_deviceid");

                entity.Property(e => e.UserDob)
                    .HasColumnType("datetime")
                    .HasColumnName("user_dob");

                entity.Property(e => e.UserDoj)
                    .HasColumnType("datetime")
                    .HasColumnName("user_doj");

                entity.Property(e => e.UserEmail)
                    .HasMaxLength(255)
                    .HasColumnName("user_email");

                entity.Property(e => e.UserFasttrack)
                    .HasMaxLength(255)
                    .HasColumnName("user_fasttrack");

                entity.Property(e => e.UserFcmid).HasColumnName("user_fcmid");

                entity.Property(e => e.UserFcmlastupdaetime)
                    .HasMaxLength(255)
                    .HasColumnName("user_fcmlastupdaetime");

                entity.Property(e => e.UserGstno).HasColumnName("user_gstno");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.UserIsactive).HasColumnName("user_isactive");

                entity.Property(e => e.UserMobile).HasColumnName("user_mobile");

                entity.Property(e => e.UserName)
                    .HasMaxLength(255)
                    .HasColumnName("user_name");

                entity.Property(e => e.UserNjname)
                    .HasMaxLength(255)
                    .HasColumnName("user_njname");

                entity.Property(e => e.UserPan)
                    .HasMaxLength(255)
                    .HasColumnName("user_pan");

                entity.Property(e => e.UserParentid).HasColumnName("user_parentid");

                entity.Property(e => e.UserPasswd).HasColumnName("user_passwd");

                entity.Property(e => e.UserPin).HasColumnName("user_pin");

                entity.Property(e => e.UserProfilephoto)
                    .HasMaxLength(255)
                    .HasColumnName("user_profilephoto");

                entity.Property(e => e.UserPromocode).HasColumnName("user_promocode");

                entity.Property(e => e.UserPurposeid).HasColumnName("user_purposeid");

                entity.Property(e => e.UserSponid).HasColumnName("user_sponid");

                entity.Property(e => e.UserStateid).HasColumnName("user_stateid");

                entity.Property(e => e.UserStatename)
                    .HasMaxLength(255)
                    .HasColumnName("user_statename");

                entity.Property(e => e.UserSubcategory).HasColumnName("user_subcategory");

                entity.Property(e => e.UserTermandcondition)
                    .HasMaxLength(255)
                    .HasColumnName("user_termandcondition");

                entity.Property(e => e.UserUname)
                    .HasMaxLength(255)
                    .HasColumnName("user_uname");

                entity.Property(e => e.UserWbcActive)
                    .HasMaxLength(255)
                    .HasColumnName("user_wbcActive");
            });

            modelBuilder.Entity<Tbl5paisaResponse>(entity =>
            {
                entity.ToTable("tbl_5Paisa_Response");

                entity.Property(e => e.RecordDate).HasColumnType("datetime");

                entity.Property(e => e.State)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Token).IsUnicode(false);

                entity.Property(e => e.TokenType)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblAccountMaster>(entity =>
            {
                entity.HasKey(e => e.AccountId);

                entity.ToTable("tbl_account_master");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.AccountName)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("account_name");

                entity.Property(e => e.OpeningBalance)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("opening_balance");

                entity.Property(e => e.UserId).HasColumnName("user_id");
            });

            modelBuilder.Entity<TblBankMaster>(entity =>
            {
                entity.HasKey(e => e.Bankid)
                    .HasName("PK_Sheet1$");

                entity.ToTable("tbl_bank_master");

                entity.Property(e => e.Bankid).HasColumnName("bankid");

                entity.Property(e => e.Bankname)
                    .HasMaxLength(255)
                    .HasColumnName("bankname");
            });

            modelBuilder.Entity<TblCityMaster>(entity =>
            {
                entity.HasKey(e => e.CityId);

                entity.ToTable("tbl_City_Master");

                entity.Property(e => e.CityId).HasColumnName("city_id");

                entity.Property(e => e.CityName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("city_name");

                entity.Property(e => e.StateId).HasColumnName("state_id");

                entity.Property(e => e.IsDeleted).HasColumnName("IsDeleted").HasDefaultValue(0);

                entity.HasOne(d => d.State)
                    .WithMany(p => p.TblCityMasters)
                    .HasForeignKey(d => d.StateId)
                    .HasConstraintName("FK_tbl_City_Master_tbl_StateMaster");
            });

            modelBuilder.Entity<TblContactMaster>(entity =>
            {
                entity.HasKey(e => e.Contactid);

                entity.ToTable("tbl_contact_master");

                entity.Property(e => e.Contactid).HasColumnName("contactid");

                entity.Property(e => e.MobileNo)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("Mobile_no");

                entity.Property(e => e.RefContactdate)
                    .HasColumnType("date")
                    .HasColumnName("ref_contactdate");

                entity.Property(e => e.RefContactname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ref_contactname");

                entity.Property(e => e.RefContactno)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("ref_contactno");

                entity.Property(e => e.RefUserid).HasColumnName("ref_userid");
            });

            modelBuilder.Entity<TblCountryMaster>(entity =>
            {
                entity.HasKey(e => e.CountryId);

                entity.ToTable("tbl_Country_Master");

                entity.Property(e => e.CountryId).HasColumnName("country_id");

                entity.Property(e => e.CountryName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("country_name");

                entity.Property(e => e.Icon)
                    .IsUnicode(false)
                    .HasColumnName("icon");

                entity.Property(e => e.Isdcode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("isdcode");

                entity.Property(e => e.IsDeleted).HasColumnName("IsDeleted").HasDefaultValue(0);
            });

            modelBuilder.Entity<TblDepartmentMaster>(entity =>
            {
                entity.HasKey(e => e.DepartmentId);

                entity.ToTable("tbl_department_master");

                entity.Property(e => e.DepartmentId).HasColumnName("department_id");

                entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<TblDesignationMaster>(entity =>
            {
                entity.HasKey(e => e.DesignationId);

                entity.ToTable("tbl_designation_master");

                entity.Property(e => e.DesignationId).HasColumnName("designation_id");

                entity.Property(e => e.DepartmentId).HasColumnName("department_id");

                entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<TblEmisipCalculator>(entity =>
            {
                entity.ToTable("tbl_EMISIP_Calculator");

                entity.Property(e => e.InterestRateOnInvestment).HasColumnType("decimal(8, 2)");

                entity.Property(e => e.LoanInterestRate).HasColumnType("decimal(8, 2)");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblExcelimportUsermaster>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tbl_excelimport_usermaster");

                entity.Property(e => e.CatId).HasColumnName("cat_id");

                entity.Property(e => e.FamilyId).HasColumnName("family_id");

                entity.Property(e => e.UserAadhar)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("user_aadhar");

                entity.Property(e => e.UserAccounttype).HasColumnName("user_accounttype");

                entity.Property(e => e.UserAddr)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("user_addr");

                entity.Property(e => e.UserCityid).HasColumnName("user_cityid");

                entity.Property(e => e.UserCityname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("user_cityname");

                entity.Property(e => e.UserCountryid).HasColumnName("user_countryid");

                entity.Property(e => e.UserCountryname)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("user_countryname");

                entity.Property(e => e.UserDeviceid)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("user_deviceid");

                entity.Property(e => e.UserDob)
                    .HasColumnType("date")
                    .HasColumnName("user_dob");

                entity.Property(e => e.UserDoj)
                    .HasColumnType("date")
                    .HasColumnName("user_doj");

                entity.Property(e => e.UserEmail)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("user_email");

                entity.Property(e => e.UserFasttrack).HasColumnName("user_fasttrack");

                entity.Property(e => e.UserFcmid)
                    .IsUnicode(false)
                    .HasColumnName("user_fcmid");

                entity.Property(e => e.UserFcmlastupdaetime)
                    .HasColumnType("date")
                    .HasColumnName("user_fcmlastupdaetime");

                entity.Property(e => e.UserGstno)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("user_gstno");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.UserIsactive).HasColumnName("user_isactive");

                entity.Property(e => e.UserMobile)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("user_mobile");

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("user_name");

                entity.Property(e => e.UserNjname)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("user_njname");

                entity.Property(e => e.UserPan)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("user_pan");

                entity.Property(e => e.UserParentid).HasColumnName("user_parentid");

                entity.Property(e => e.UserPasswd)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("user_passwd");

                entity.Property(e => e.UserPin)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("user_pin");

                entity.Property(e => e.UserProfilephoto)
                    .IsUnicode(false)
                    .HasColumnName("user_profilephoto");

                entity.Property(e => e.UserPromocode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("user_promocode");

                entity.Property(e => e.UserPurposeid).HasColumnName("user_purposeid");

                entity.Property(e => e.UserSponid).HasColumnName("user_sponid");

                entity.Property(e => e.UserStateid).HasColumnName("user_stateid");

                entity.Property(e => e.UserStatename)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("user_statename");

                entity.Property(e => e.UserSubcategory)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("user_subcategory");

                entity.Property(e => e.UserTermandcondition).HasColumnName("user_termandcondition");

                entity.Property(e => e.UserUname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("user_uname");

                entity.Property(e => e.UserWbcActive).HasColumnName("user_wbcActive");
            });

            modelBuilder.Entity<TblFamilyMember>(entity =>
            {
                entity.HasKey(e => e.Memberid);

                entity.ToTable("tbl_family_member");

                entity.HasIndex(e => e.Mobileno, "IX_tbl_family_member")
                    .IsUnique();

                entity.Property(e => e.Memberid).HasColumnName("memberid");

                entity.Property(e => e.Familyid).HasColumnName("familyid");

                entity.Property(e => e.Mobileno)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("mobileno");

                entity.Property(e => e.Name)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Relation)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("relation");

                entity.Property(e => e.RelativeUserId).HasColumnName("relativeUserId");

                entity.Property(e => e.Userid).HasColumnName("userid");
            });

            modelBuilder.Entity<TblFaq>(entity =>
            {
                entity.ToTable("tbl_FAQ");

                entity.Property(e => e.Answer)
                    .HasMaxLength(8000)
                    .IsUnicode(false)
                    .HasColumnName("answer");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDate");

                entity.Property(e => e.Isactive).HasColumnName("isactive");

                entity.Property(e => e.Question)
                    .HasMaxLength(8000)
                    .IsUnicode(false)
                    .HasColumnName("question");
            });

            modelBuilder.Entity<TblFasttrackLedger>(entity =>
            {
                entity.HasKey(e => e.FtId);

                entity.ToTable("tbl_Fasttrack_Ledger");

                entity.Property(e => e.FtId).HasColumnName("ft_id");

                entity.Property(e => e.Credit).HasColumnName("credit");

                entity.Property(e => e.Debit).HasColumnName("debit");

                entity.Property(e => e.Narration)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("timestamp");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblFasttrackLedgers)
                    .HasForeignKey(d => d.Userid)
                    .HasConstraintName("FK_tbl_Fasttrack_Ledger_tbl_User_Master");
            });

            modelBuilder.Entity<TblFasttrackSchemeMaster>(entity =>
            {
                entity.ToTable("tbl_fasttrack_scheme_master");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Goldpoint).HasColumnName("goldpoint");

                entity.Property(e => e.Level)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("level");

                entity.Property(e => e.NoOfFasttrackClients).HasColumnName("No_of_fasttrack_clients");

                entity.Property(e => e.NoOfNonFasttrackClients).HasColumnName("No_of_non_fasttrack_clients");

                entity.Property(e => e.TotalClient).HasColumnName("total_client");
            });

            modelBuilder.Entity<TblFasttrackSubscription>(entity =>
            {
                entity.ToTable("tbl_Fasttrack_Subscription");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.MobileNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblFolioDetail>(entity =>
            {
                entity.ToTable("tbl_Folio_Detail");

                entity.Property(e => e.BuyAmt)
                    .HasColumnType("numeric(8, 2)")
                    .HasColumnName("buy_amt");

                entity.Property(e => e.BuyRate)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("buy_rate");

                entity.Property(e => e.Folioid).HasColumnName("folioid");

                entity.Property(e => e.Qty)
                    .HasColumnType("numeric(6, 2)")
                    .HasColumnName("qty");

                entity.Property(e => e.ScripId).HasColumnName("scrip_id");

                entity.HasOne(d => d.Folio)
                    .WithMany(p => p.TblFolioDetails)
                    .HasForeignKey(d => d.Folioid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tbl_Folio_Detail_tbl_Folio_Detail");
            });

            modelBuilder.Entity<TblFolioMaster>(entity =>
            {
                entity.HasKey(e => e.Folioid);

                entity.ToTable("tbl_Folio_Master");

                entity.Property(e => e.Folioid).HasColumnName("folioid");

                entity.Property(e => e.FolioTypeId).HasColumnName("folio_type_id");

                entity.Property(e => e.Pan)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("pan");

                entity.Property(e => e.UploadDate)
                    .HasColumnType("datetime")
                    .HasColumnName("upload_date");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.HasOne(d => d.FolioType)
                    .WithMany(p => p.TblFolioMasters)
                    .HasForeignKey(d => d.FolioTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tbl_Folio_Master_tbl_Invesment_Type");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblFolioMasters)
                    .HasForeignKey(d => d.Userid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tbl_Folio_Master_tbl_Folio_Master");
            });

            modelBuilder.Entity<TblFolioTypeMaster>(entity =>
            {
                entity.HasKey(e => e.FolioTypeId);

                entity.ToTable("tbl_Folio_Type_Master");

                entity.Property(e => e.FolioTypeId).HasColumnName("folio_type_Id");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.FolioType)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("folio_type");
            });

            modelBuilder.Entity<TblFyersResponse>(entity =>
            {
                entity.ToTable("tbl_Fyers_Response");

                entity.Property(e => e.Message)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RecordDate).HasColumnType("datetime");

                entity.Property(e => e.Sattribute)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SAttribute");

                entity.Property(e => e.State)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Token).IsUnicode(false);

                entity.Property(e => e.TokenType)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblGoldPoint>(entity =>
            {
                entity.HasKey(e => e.GpId);

                entity.ToTable("tbl_gold_points");

                entity.Property(e => e.GpId).HasColumnName("gp_id");

                entity.Property(e => e.Credit).HasColumnName("credit");

                entity.Property(e => e.Debit).HasColumnName("debit");

                entity.Property(e => e.PointCategory).HasColumnName("point_category");

                entity.Property(e => e.Timestamp)
                    .HasColumnType("datetime")
                    .HasColumnName("timestamp");

                entity.Property(e => e.Type)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("type");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.Property(e => e.Vendorid).HasColumnName("vendorid");

                entity.HasOne(d => d.PointCategoryNavigation)
                    .WithMany(p => p.TblGoldPoints)
                    .HasForeignKey(d => d.PointCategory)
                    .HasConstraintName("FK_tbl_gold_points_tbl_GoldPoint_Category");
            });

            modelBuilder.Entity<TblGoldPointCategory>(entity =>
            {
                entity.ToTable("tbl_GoldPoint_Category");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.PointCategory)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("point_category");
            });

            modelBuilder.Entity<TblGoldReferral>(entity =>
            {
                entity.HasKey(e => e.RefId);

                entity.ToTable("tbl_Gold_Referral");

                entity.Property(e => e.RefId).HasColumnName("ref_id");

                entity.Property(e => e.CityId).HasColumnName("city_id");

                entity.Property(e => e.RefById).HasColumnName("ref_by_id");

                entity.Property(e => e.RefDate)
                    .HasColumnType("date")
                    .HasColumnName("ref_date");

                entity.Property(e => e.RefEmail)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ref_email");

                entity.Property(e => e.RefIsactive).HasColumnName("ref_isactive");

                entity.Property(e => e.RefMobile)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("ref_mobile");

                entity.Property(e => e.RefName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ref_name");

                entity.Property(e => e.RefReason)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ref_reason");

                entity.Property(e => e.RefRel)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ref_rel");

                entity.Property(e => e.RefStatus)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ref_status");

                entity.Property(e => e.RefType)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ref_type");

                entity.HasOne(d => d.RefBy)
                    .WithMany(p => p.TblGoldReferrals)
                    .HasForeignKey(d => d.RefById)
                    .HasConstraintName("FK_tbl_Gold_Referral_tbl_User_Master");
            });

            modelBuilder.Entity<TblInsuranceCalculator>(entity =>
            {
                entity.HasKey(e => e.Insurancesid);

                entity.ToTable("tbl_insurance_calculator");

                entity.Property(e => e.Insurancesid).HasColumnName("insurancesid");

                entity.Property(e => e.Annualincome).HasColumnName("annualincome");

                entity.Property(e => e.Dob)
                    .HasColumnType("date")
                    .HasColumnName("dob");

                entity.Property(e => e.Existinglifecover).HasColumnName("existinglifecover");

                entity.Property(e => e.Gender)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("gender");

                entity.Property(e => e.InsDate)
                    .HasColumnType("datetime")
                    .HasColumnName("ins_date");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Totalloandue).HasColumnName("totalloandue");

                entity.Property(e => e.Totalsaving).HasColumnName("totalsaving");
            });

            modelBuilder.Entity<TblInsuranceCompanylist>(entity =>
            {
                entity.HasKey(e => e.InsuranceCompanyid)
                    .HasName("PK_Sheet1$_1");

                entity.ToTable("tbl_Insurance_companylist ");

                entity.Property(e => e.InsuranceCompanyid).HasColumnName("insurance_companyid");

                entity.Property(e => e.InsuranceCompanyname)
                    .HasMaxLength(255)
                    .HasColumnName("insurance_companyname");

                entity.Property(e => e.InsuranceCompanytypeid).HasColumnName("insurance_companytypeid");

                entity.HasOne(d => d.InsuranceCompanytype)
                    .WithMany(p => p.TblInsuranceCompanylists)
                    .HasForeignKey(d => d.InsuranceCompanytypeid)
                    .HasConstraintName("FK_tbl_Insurance_companylist _tbl_Insurancetype");
            });

            modelBuilder.Entity<TblInsuranceTypeMaster>(entity =>
            {
                entity.HasKey(e => e.InsPlantypeId);

                entity.ToTable("tbl_insurance_type_master");

                entity.Property(e => e.InsPlantypeId).HasColumnName("ins_plantype_id");

                entity.Property(e => e.InsPlanType)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("ins_plan_type");
            });

            modelBuilder.Entity<TblInsuranceclient>(entity =>
            {
                entity.ToTable("tbl_insuranceclient");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Companyid).HasColumnName("companyid");

                entity.Property(e => e.InsAmount).HasColumnName("ins_amount");

                entity.Property(e => e.InsDuedate)
                    .HasColumnType("datetime")
                    .HasColumnName("ins_duedate");

                entity.Property(e => e.InsEmail)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("ins_email");

                entity.Property(e => e.InsFrequency)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("ins_frequency");

                entity.Property(e => e.InsMobile)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ins_mobile");

                entity.Property(e => e.InsNewpolicy)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("ins_newpolicy");

                entity.Property(e => e.InsPan)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ins_pan");

                entity.Property(e => e.InsPlan)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("ins_plan");

                entity.Property(e => e.InsPlantype)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("ins_plantype");

                entity.Property(e => e.InsPlantypeId).HasColumnName("ins_plantype_id");

                entity.Property(e => e.InsPolicy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ins_ policy");

                entity.Property(e => e.InsPremiumRmdDate)
                    .HasColumnType("date")
                    .HasColumnName("ins_premium_rmd_date");

                entity.Property(e => e.InsStartdate)
                    .HasColumnType("date")
                    .HasColumnName("ins_startdate");

                entity.Property(e => e.InsTerm).HasColumnName("ins_term");

                entity.Property(e => e.InsUserid).HasColumnName("ins_userid");

                entity.Property(e => e.InsUsername)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ins_username");

                entity.Property(e => e.InvSubtype).HasColumnName("inv_subtype");

                entity.Property(e => e.InvType).HasColumnName("inv_type");

                entity.Property(e => e.IsKathrough).HasColumnName("IsKAThrough");

                entity.Property(e => e.PremiumAmount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("premium_amount");

                entity.HasOne(d => d.TblInsuranceTypeMaster)
                    .WithMany(p => p.TblInsuranceclients)
                    .HasForeignKey(d => d.InsPlantypeId)
                    .HasConstraintName("FK_tbl_insuranceclient_tbl_insurance_type_master");
            });

            modelBuilder.Entity<TblInsurancetype>(entity =>
            {
                entity.HasKey(e => e.InsuranceCompnytypeid);

                entity.ToTable("tbl_Insurancetype");

                entity.Property(e => e.InsuranceCompnytypeid).HasColumnName("Insurance_compnytypeid");

                entity.Property(e => e.InsuranceCompanytypename)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("Insurance_companytypename");
            });

            modelBuilder.Entity<TblInvesmentType>(entity =>
            {
                entity.ToTable("tbl_Invesment_Type");

                entity.Property(e => e.InvestmentName)
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblLeaveType>(entity =>
            {
                entity.HasKey(e => e.LeaveId);

                entity.ToTable("tbl_leave_type");

                entity.Property(e => e.LeaveId).HasColumnName("leave_id");

                entity.Property(e => e.AllowedDay).HasColumnName("allowed_day");

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<TblMfSchemeMaster>(entity =>
            {
                entity.HasKey(e => e.SchemeId)
                    .HasName("PK_tbl_Scheme_Master");

                entity.ToTable("tbl_MF_Scheme_Master");

                entity.Property(e => e.SchemeId).HasColumnName("scheme_id");

                entity.Property(e => e.IsinReinvestment)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("ISIN_Reinvestment");

                entity.Property(e => e.Isingrowth)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("ISINGrowth");

                entity.Property(e => e.NetAssetValue)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.SchemeCategorytype)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("scheme_categorytype");

                entity.Property(e => e.SchemeCode)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("scheme_code");

                entity.Property(e => e.SchemeDate)
                    .HasColumnType("date")
                    .HasColumnName("scheme_date");

                entity.Property(e => e.SchemeName)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("scheme_name");
            });

            modelBuilder.Entity<TblMftransaction>(entity =>
            {
                entity.HasKey(e => e.Trnid);

                entity.ToTable("tbl_mftransaction");

                entity.Property(e => e.Trnid).HasColumnName("trnid");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.Foliono)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Invamount).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.Nav).HasColumnName("nav");

                entity.Property(e => e.Noofunit)
                    .HasColumnType("decimal(18, 4)")
                    .HasColumnName("noofunit");

                entity.Property(e => e.Notes)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Schemename)
                    .HasMaxLength(350)
                    .IsUnicode(false);

                entity.Property(e => e.SchemeId).HasColumnName("SchemeId");

                entity.Property(e => e.Transactiontype)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.Property(e => e.Username)
                    .HasMaxLength(70)
                    .IsUnicode(false)
                    .HasColumnName("username");
            });

            modelBuilder.Entity<TblMgainInvesment>(entity =>
            {
                entity.HasKey(e => e.MgainId)
                    .HasName("PK_tbl");

                entity.ToTable("tbl_MGainInvesment");

                entity.Property(e => e.MgainId).HasColumnName("MGainId");

                entity.Property(e => e.Amount).HasColumnType("numeric(10, 2)");

                entity.Property(e => e.InvestmentDate).HasColumnType("datetime");

                entity.Property(e => e.MaturityDate).HasColumnType("datetime");

                entity.Property(e => e.Rate).HasColumnType("numeric(3, 1)");

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("type");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblMgainInvesments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tbl_MGainInvesment_tbl_User_Master");
            });

            modelBuilder.Entity<TblMgainLedger>(entity =>
            {
                entity.HasKey(e => e.LedgerId);

                entity.ToTable("tbl_MGainLedger");

                entity.Property(e => e.Credit).HasColumnType("numeric(10, 0)");

                entity.Property(e => e.Debit).HasColumnType("numeric(10, 2)");

                entity.Property(e => e.InvestmentDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });
            modelBuilder.Entity<TblMgainSchemeMaster>(entity =>
            {
                entity.ToTable("tbl_mgain_scheme_master");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AdditionalInterest10).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.AdditionalInterest4).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.AdditionalInterest5).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.AdditionalInterest6).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.AdditionalInterest7).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.AdditionalInterest8).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.AdditionalInterest9).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.Interst1).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Interst10).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Interst2).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Interst3).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Interst4).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Interst5).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Interst6).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Interst7).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Interst8).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Interst9).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Schemename)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StartDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblMgainTransactionAccountTem>(entity =>
            {
                entity.ToTable("tbl_mgain_transaction_account_tem");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Accountid).HasColumnName("accountid");

                entity.Property(e => e.MgainCredit)
                    .HasColumnType("decimal(25, 2)")
                    .HasColumnName("mgain_credit");

                entity.Property(e => e.MgainDate)
                    .HasColumnType("date")
                    .HasColumnName("mgain_date");

                entity.Property(e => e.MgainDebit)
                    .HasColumnType("decimal(25, 2)")
                    .HasColumnName("mgain_debit");

                entity.Property(e => e.MgainParticular)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("mgain_particular");

                entity.Property(e => e.MgainType)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("mgain_type");

                entity.Property(e => e.MgainUserid).HasColumnName("mgain_userid");

                entity.Property(e => e.MgainVchno).HasColumnName("mgain_vchno");

                entity.Property(e => e.MgainVchtype)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("mgain_vchtype");
            });

            modelBuilder.Entity<TblMgaindetail>(entity =>
            {
                entity.ToTable("tbl_mgaindetails");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Mgain1stholder)
                    .HasMaxLength(90)
                    .IsUnicode(false)
                    .HasColumnName("mgain_1stholder");

                entity.Property(e => e.Mgain1stholderpan)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("mgain_1stholderpan");

                entity.Property(e => e.MgainAccountnum).HasColumnName("mgain_accountnum");

                entity.Property(e => e.MgainAggre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("mgain_aggre");

                entity.Property(e => e.MgainAllocatedsqft)
                    .HasColumnType("decimal(25, 4)")
                    .HasColumnName("mgain_allocatedsqft");

                entity.Property(e => e.MgainAllocatedsqftamt)
                    .HasColumnType("decimal(25, 4)")
                    .HasColumnName("mgain_allocatedsqftamt");

                entity.Property(e => e.MgainInvamt)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("mgain_invamt");

                entity.Property(e => e.MgainIsactive).HasColumnName("mgain_isactive");

                entity.Property(e => e.MgainModeholder)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("mgain_modeholder");

                entity.Property(e => e.MgainPlotno)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("mgain_plotno");

                entity.Property(e => e.MgainProjectname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("mgain_projectname");

                entity.Property(e => e.MgainRedemamt)
                    .HasColumnType("decimal(28, 3)")
                    .HasColumnName("mgain_redemamt");

                entity.Property(e => e.MgainRedemdate)
                    .HasColumnType("datetime")
                    .HasColumnName("mgain_redemdate");

                entity.Property(e => e.MgainTotalplotamt)
                    .HasColumnType("decimal(25, 3)")
                    .HasColumnName("mgain_totalplotamt");

                entity.Property(e => e.MgainTotalsqft)
                    .HasColumnType("decimal(25, 4)")
                    .HasColumnName("mgain_totalsqft");

                entity.Property(e => e.MgainType)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("mgain_type");

                entity.Property(e => e.MgainUserid).HasColumnName("mgain_userid");
            });

            modelBuilder.Entity<TblMgaindetailsTruncate>(entity =>
            {
                entity.ToTable("tbl_mgaindetails_truncate");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Mgain1stholder)
                    .HasMaxLength(90)
                    .IsUnicode(false)
                    .HasColumnName("mgain_1stholder");

                entity.Property(e => e.Mgain1stholderpan)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("mgain_1stholderpan");

                entity.Property(e => e.MgainAccountnum).HasColumnName("mgain_accountnum");

                entity.Property(e => e.MgainAggre)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("mgain_aggre");

                entity.Property(e => e.MgainAllocatedsqft)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("mgain_allocatedsqft");

                entity.Property(e => e.MgainAllocatedsqftamt)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("mgain_allocatedsqftamt");

                entity.Property(e => e.MgainInvamt)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("mgain_invamt");

                entity.Property(e => e.MgainIsactive).HasColumnName("mgain_isactive");

                entity.Property(e => e.MgainModeholder)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("mgain_modeholder");

                entity.Property(e => e.MgainPlotno)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("mgain_plotno");

                entity.Property(e => e.MgainProjectname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("mgain_projectname");

                entity.Property(e => e.MgainRedemamt)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("mgain_redemamt");

                entity.Property(e => e.MgainRedemdate)
                    .HasColumnType("datetime")
                    .HasColumnName("mgain_redemdate");

                entity.Property(e => e.MgainTotalplotamt)
                    .HasColumnType("decimal(18, 3)")
                    .HasColumnName("mgain_totalplotamt");

                entity.Property(e => e.MgainTotalsqft)
                    .HasColumnType("decimal(18, 4)")
                    .HasColumnName("mgain_totalsqft");

                entity.Property(e => e.MgainType)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("mgain_type");

                entity.Property(e => e.MgainUserid).HasColumnName("mgain_userid");
            });

            modelBuilder.Entity<TblMunafeKiClass>(entity =>
            {
                entity.ToTable("tbl_MunafeKiClass");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Entrydate)
                    .HasColumnType("datetime")
                    .HasColumnName("entrydate");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("title");

                entity.Property(e => e.Videolink)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("videolink");
            });

            modelBuilder.Entity<TblNotexistuserMftransaction>(entity =>
            {
                entity.ToTable("tbl_notexistuser_mftransaction");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.Foliono)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Invamount).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.Nav).HasColumnName("nav");

                entity.Property(e => e.Noofunit)
                    .HasColumnType("decimal(18, 4)")
                    .HasColumnName("noofunit");

                entity.Property(e => e.Schemename)
                    .HasMaxLength(350)
                    .IsUnicode(false);

                entity.Property(e => e.SchemeId).HasColumnName("SchemeId");

                entity.Property(e => e.Transactiontype)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.Property(e => e.Username)
                    .HasMaxLength(70)
                    .IsUnicode(false)
                    .HasColumnName("username");

                entity.Property(e => e.Userpan)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("userpan");
            });

            modelBuilder.Entity<TblNotfoundInsuranceclient>(entity =>
            {
                entity.ToTable("tbl_notfound_insuranceclient");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.InsAmount)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("ins_amount");

                entity.Property(e => e.InsDuedate)
                    .HasColumnType("datetime")
                    .HasColumnName("ins_duedate");

                entity.Property(e => e.InsEmail)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("ins_email");

                entity.Property(e => e.InsMobile)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ins_mobile");

                entity.Property(e => e.InsNewpolicy)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("ins_newpolicy");

                entity.Property(e => e.InsPan)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ins_pan");

                entity.Property(e => e.InsPlan)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("ins_plan");

                entity.Property(e => e.InsPlantype)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("ins_plantype");

                entity.Property(e => e.InsPolicy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ins_ policy");

                entity.Property(e => e.InsSumAssuredInsured).HasColumnName("ins_sumAssured_Insured");

                entity.Property(e => e.InsUsername)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ins_username");
            });

            modelBuilder.Entity<TblOfferMaster>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tbl_Offer_Master");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Img)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("img");

                entity.Property(e => e.Isactive).HasColumnName("isactive");

                entity.Property(e => e.Link)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("link");

                entity.Property(e => e.OfferPercentage).HasColumnName("offer_percentage");

                entity.Property(e => e.OfferValidtill)
                    .HasColumnType("datetime")
                    .HasColumnName("offer_validtill");

                entity.Property(e => e.OfferValue).HasColumnName("offer_value");

                entity.Property(e => e.Title)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("title");

                entity.Property(e => e.VendorId).HasColumnName("vendor_id");
            });

            modelBuilder.Entity<TblOrder>(entity =>
            {
                entity.HasKey(e => e.OrderId);

                entity.ToTable("tbl_Order");

                entity.Property(e => e.OrderId).ValueGeneratedNever();

                entity.Property(e => e.Amount).HasColumnType("numeric(7, 2)");

                entity.Property(e => e.CityId).HasColumnName("City_id");

                entity.Property(e => e.CountryId).HasColumnName("Country_id");

                entity.Property(e => e.DeliverType)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.OrderStatus).HasColumnName("Order_status");

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ShipAddress)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ShipDate).HasColumnType("datetime");

                entity.Property(e => e.ShipName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ShippingCharge).HasColumnType("decimal(7, 2)");

                entity.Property(e => e.StateId).HasColumnName("State_id");

                entity.Property(e => e.Tax).HasColumnType("decimal(6, 2)");

                entity.Property(e => e.TrackingNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Zip)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblOrderDetail>(entity =>
            {
                entity.ToTable("tbl_OrderDetails");

                entity.Property(e => e.OderId).HasColumnName("Oder_Id");

                entity.Property(e => e.Price).HasColumnType("numeric(10, 2)");

                entity.Property(e => e.ProductId).HasColumnName("Product_Id");

                entity.Property(e => e.ProductName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Product_Name");

                entity.Property(e => e.Sku)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SKU");

                entity.HasOne(d => d.Oder)
                    .WithMany(p => p.TblOrderDetails)
                    .HasForeignKey(d => d.OderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tbl_OrderDetails_tbl_Order");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.TblOrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tbl_OrderDetails_tbl_WBC_Mall_Products");
            });

            modelBuilder.Entity<TblPortfolioReviewRequest>(entity =>
            {
                entity.HasKey(e => e.RequestId)
                    .HasName("PK_tbl_Porfolio_Review_Request");

                entity.ToTable("tbl_Portfolio_Review_Request");

                entity.Property(e => e.RequestId).HasColumnName("request_Id");

                entity.Property(e => e.PdfPassword)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RequestDate)
                    .HasColumnType("datetime")
                    .HasColumnName("request_date");

                entity.Property(e => e.RequestEmail)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("request_email");

                entity.Property(e => e.RequestMobile)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("request_mobile");

                entity.Property(e => e.RequestPan)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("request_pan");

                entity.Property(e => e.RequestPdf)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("request_pdf");

                entity.Property(e => e.RequestSubtype).HasColumnName("request_subtype");

                entity.Property(e => e.RequestType).HasColumnName("request_type");

                entity.Property(e => e.RequestUserid)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("request_userid");

                entity.Property(e => e.Status)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("status");

                entity.Property(e => e.Stinvtype)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("stinvtype");

                entity.HasOne(d => d.RequestTypeNavigation)
                    .WithMany(p => p.TblPortfolioReviewRequests)
                    .HasForeignKey(d => d.RequestType)
                    .HasConstraintName("FK_tbl_Porfolio_Review_Request_tbl_Invesment_Type");
            });

            modelBuilder.Entity<TblProductBanner>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tbl_Product_Banners");

                entity.Property(e => e.BannerId).HasColumnName("bannerId");

                entity.Property(e => e.BannerImg)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("banner_img");

                entity.Property(e => e.BannerIsactive).HasColumnName("banner_isactive");
            });

            modelBuilder.Entity<TblProductImg>(entity =>
            {
                entity.ToTable("tbl_ProductIMGs");

                entity.Property(e => e.CeratedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("ceratedDate");

                entity.Property(e => e.Img)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("img");

                entity.Property(e => e.ModifyDate)
                    .HasColumnType("datetime")
                    .HasColumnName("modifyDate");

                entity.Property(e => e.Modifyby).HasColumnName("modifyby");

                entity.Property(e => e.ProductId).HasColumnName("productId");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.TblProductImgs)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tbl_ProductIMGs_tbl_ProductIMGs");
            });

            modelBuilder.Entity<TblRealEastateReview>(entity =>
            {
                entity.ToTable("tbl_RealEastate_review");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.BuiltupArea)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CarParking)
                    .HasMaxLength(5)
                    .HasColumnName("carParking")
                    .IsFixedLength();

                entity.Property(e => e.CarpetArea)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("carpetArea");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDate");

                entity.Property(e => e.EnterFacing)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("enterFacing");

                entity.Property(e => e.Location)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Price).HasColumnType("numeric(10, 2)");

                entity.Property(e => e.ProjectName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PropertyType)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("propertyType");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.Property(e => e.Year).HasColumnType("numeric(6, 0)");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblRealEastateReviews)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tbl_RealEastate_review_tbl_User_Master");
            });

            modelBuilder.Entity<TblRealEastateReviewImg>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tbl_RealEastate_reviewImg");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDate");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Img)
                    .IsUnicode(false)
                    .HasColumnName("img");
            });

            modelBuilder.Entity<TblReferralMaster>(entity =>
            {
                entity.HasKey(e => e.RefId)
                    .HasName("PK_tbl_Referral_Contact");

                entity.ToTable("tbl_Referral_Master");

                entity.Property(e => e.RefId).HasColumnName("ref_id");

                entity.Property(e => e.RefDate)
                    .HasColumnType("date")
                    .HasColumnName("ref_date");

                entity.Property(e => e.RefMobileno)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ref_mobileno");

                entity.Property(e => e.RefName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ref_name");

                entity.Property(e => e.RefParentid).HasColumnName("ref_parentid");

                entity.HasOne(d => d.RefParent)
                    .WithMany(p => p.TblReferralMasters)
                    .HasForeignKey(d => d.RefParentid)
                    .HasConstraintName("FK_tbl_Referral_Master_tbl_User_Master");
            });

            modelBuilder.Entity<TblRequestDetail>(entity =>
            {
                entity.HasKey(e => e.InvestmentId);

                entity.ToTable("tbl_request_detail");

                entity.Property(e => e.InvestmentId).HasColumnName("investment_Id");

                entity.Property(e => e.FinancialSector)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.InstallmentAmount)
                    .HasColumnType("numeric(18, 2)")
                    .HasColumnName("Installment_amount");

                entity.Property(e => e.InvestmentSubtypeId).HasColumnName("Investment_subtypeId");

                entity.Property(e => e.InvestmentTypeId).HasColumnName("Investment_typeId");

                entity.Property(e => e.LoanInvAmount)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("Loan_inv_amount");

                entity.Property(e => e.MaturityDate)
                    .HasColumnType("date")
                    .HasColumnName("Maturity_date");

                entity.Property(e => e.PaymentLength)
                    .HasColumnType("numeric(3, 1)")
                    .HasColumnName("Payment_length");

                entity.Property(e => e.PaymentTenure).HasColumnName("Payment_tenure");

                entity.Property(e => e.RateOfInterest)
                    .HasColumnType("numeric(3, 2)")
                    .HasColumnName("Rate_of_interest");

                entity.Property(e => e.RequestId).HasColumnName("request_Id");

                entity.Property(e => e.StartDate)
                    .HasColumnType("date")
                    .HasColumnName("Start_date");
            });

            modelBuilder.Entity<TblRetirementCalculator>(entity =>
            {
                entity.ToTable("tbl_Retirement_Calculator");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblRoleAssignment>(entity =>
            {
                entity.ToTable("tbl_role_assignment");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.IsDeleted).HasColumnName("IsDeleted").HasDefaultValue(0);
            });

            modelBuilder.Entity<TblRoleMaster>(entity =>
            {
                entity.HasKey(e => e.RoleId);

                entity.ToTable("tbl_role_master");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.IsDeleted).HasColumnName("IsDeleted").HasDefaultValue(0);

                entity.Property(e => e.RoleName)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("role_name");
            });

            modelBuilder.Entity<TblRolePermission>(entity =>
            {
                entity.ToTable("tbl_role_permission");

                entity.Property(e => e.AllowAdd).HasColumnName("Allow_Add");

                entity.Property(e => e.AllowDelete).HasColumnName("Allow_Delete");

                entity.Property(e => e.AllowEdit).HasColumnName("Allow_Edit");

                entity.Property(e => e.AllowView).HasColumnName("Allow_View");

                entity.Property(e => e.IsDeleted).HasColumnName("IsDeleted").HasDefaultValue(0);

                entity.Property(e => e.ModuleName)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.RoleId).HasColumnName("role_id");
            });

            modelBuilder.Entity<TblScripMaster>(entity =>
            {
                entity.HasKey(e => e.Scripid);

                entity.ToTable("tbl_Scrip_Master");

                entity.Property(e => e.Scripid).HasColumnName("scripid");

                entity.Property(e => e.Exchange)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("exchange");

                entity.Property(e => e.Isin)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("isin");

                entity.Property(e => e.Ltp)
                    .HasColumnType("numeric(10, 2)")
                    .HasColumnName("LTP");

                entity.Property(e => e.Ltt)
                    .HasColumnType("datetime")
                    .HasColumnName("LTT");

                entity.Property(e => e.Scripname)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("scripname");

                entity.Property(e => e.Scripsymbol)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("scripsymbol");
            });

            modelBuilder.Entity<TblSegmentMaster>(entity =>
            {
                entity.HasKey(e => e.SegmentId);

                entity.ToTable("tbl_segment_master");

                entity.Property(e => e.SegmentId).HasColumnName("segment_id");

                entity.Property(e => e.SegmentName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("segment_name");
            });

            modelBuilder.Entity<TblSipCalculator>(entity =>
            {
                entity.ToTable("tbl_SIP_Calculator");

                entity.Property(e => e.ExpectedReturnPer).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Sipamount).HasColumnName("SIPAmount");
            });

            modelBuilder.Entity<TblStateMaster>(entity =>
            {
                entity.HasKey(e => e.StateId);

                entity.ToTable("tbl_StateMaster");

                entity.Property(e => e.StateId).HasColumnName("state_id");

                entity.Property(e => e.CountryId).HasColumnName("country_id");

                entity.Property(e => e.StateName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("state_name");

                entity.Property(e => e.IsDeleted).HasColumnName("IsDeleted").HasDefaultValue(0);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.TblStateMasters)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("FK_tbl_StateMaster_tbl_Country_Master");
            });

            modelBuilder.Entity<TblStockData>(entity =>
            {
                entity.ToTable("tbl_stock_data");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.StBranch).HasColumnName("st_branch");

                entity.Property(e => e.StBrokerage)
                    .HasColumnType("decimal(18, 3)")
                    .HasColumnName("st_brokerage");

                entity.Property(e => e.StClientcode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("st_clientcode");

                entity.Property(e => e.StClientname)
                    .HasMaxLength(70)
                    .IsUnicode(false)
                    .HasColumnName("st_clientname");

                entity.Property(e => e.StCostpershare)
                    .HasColumnType("decimal(20, 3)")
                    .HasColumnName("st_costpershare");

                entity.Property(e => e.StCostvalue)
                    .HasColumnType("decimal(20, 3)")
                    .HasColumnName("st_costvalue");

                entity.Property(e => e.StDate)
                    .HasColumnType("datetime")
                    .HasColumnName("st_date");

                entity.Property(e => e.StNetcostvalue)
                    .HasColumnType("decimal(20, 3)")
                    .HasColumnName("st_netcostvalue");

                entity.Property(e => e.StNetrate)
                    .HasColumnType("decimal(18, 3)")
                    .HasColumnName("st_netrate");

                entity.Property(e => e.StNetsharerate)
                    .HasColumnType("decimal(20, 3)")
                    .HasColumnName("st_netsharerate");

                entity.Property(e => e.StNetvalue)
                    .HasColumnType("decimal(20, 3)")
                    .HasColumnName("st_netvalue");

                entity.Property(e => e.StQty).HasColumnName("st_qty");

                entity.Property(e => e.StRate)
                    .HasColumnType("decimal(18, 3)")
                    .HasColumnName("st_rate");

                entity.Property(e => e.StScripname)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("st_scripname");

                entity.Property(e => e.StTransactionDetails)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("st_transaction_details");

                entity.Property(e => e.StSettno)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("st_settno");

                entity.Property(e => e.StType)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("st_type");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.Property(e => e.FirmName)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("firm_name");
            });

            modelBuilder.Entity<TblSubInvesmentType>(entity =>
            {
                entity.ToTable("tbl_sub_invesment_type");

                entity.Property(e => e.InvestmentType)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.InvesmentType)
                    .WithMany(p => p.TblSubInvesmentTypes)
                    .HasForeignKey(d => d.InvesmentTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tbl_Sub_Invesment_Type_tbl_Invesment_Type");
            });

            modelBuilder.Entity<TblSubsubInvType>(entity =>
            {
                entity.ToTable("tbl_subsub_inv_type");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.SubInvType)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("sub_inv_type");

                entity.Property(e => e.SubInvTypeId).HasColumnName("sub_inv_type_id");
            });

            modelBuilder.Entity<TblTempUserpan>(entity =>
            {
                entity.ToTable("tbl_temp_userpan");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Username)
                    .HasMaxLength(90)
                    .IsUnicode(false)
                    .HasColumnName("username");

                entity.Property(e => e.Userpan)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("userpan");
            });

            modelBuilder.Entity<TblTermsCondition>(entity =>
            {
                entity.ToTable("tbl_TermsConditions");

                entity.Property(e => e.Description)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Entrydate).HasColumnType("datetime");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("title");
            });

            modelBuilder.Entity<TblUserCategoryMaster>(entity =>
            {
                entity.HasKey(e => e.CatId)
                    .HasName("PK_Table_UserCategory");

                entity.ToTable("tbl_User_Category_Master");

                entity.Property(e => e.CatId).HasColumnName("cat_id");

                entity.Property(e => e.CatIsactive).HasColumnName("cat_isactive");

                entity.Property(e => e.CatName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cat_name");
            });

            modelBuilder.Entity<TblUserDepartment>(entity =>
            {
                entity.ToTable("tbl_user_department");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DepartmentId).HasColumnName("department_id");

                entity.Property(e => e.Isdeleted).HasColumnName("isdeleted");

                entity.Property(e => e.UserId).HasColumnName("user_id");
            });

            modelBuilder.Entity<TblUserMaster>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("tbl_User_Master");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.CatId).HasColumnName("cat_id");

                entity.Property(e => e.FamilyId).HasColumnName("family_id");

                entity.Property(e => e.FastTrackActivationDate).HasColumnType("datetime");

                entity.Property(e => e.UserAadhar)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("user_aadhar");

                entity.Property(e => e.UserAccounttype).HasColumnName("user_accounttype");

                entity.Property(e => e.UserAddr)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("user_addr");

                entity.Property(e => e.UserCityid).HasColumnName("user_cityid");

                entity.Property(e => e.UserCountryid).HasColumnName("user_countryid");

                entity.Property(e => e.UserDeviceid)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("user_deviceid");

                entity.Property(e => e.UserDob)
                    .HasColumnType("date")
                    .HasColumnName("user_dob");

                entity.Property(e => e.UserDoj)
                    .HasColumnType("date")
                    .HasColumnName("user_doj");

                entity.Property(e => e.UserEmail)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("user_email");

                entity.Property(e => e.UserFasttrack).HasColumnName("user_fasttrack");

                entity.Property(e => e.UserFcmid)
                    .IsUnicode(false)
                    .HasColumnName("user_fcmid");

                entity.Property(e => e.UserFcmlastupdaetime)
                    .HasColumnType("datetime")
                    .HasColumnName("user_fcmlastupdaetime");

                entity.Property(e => e.UserGstno)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("user_gstno");

                entity.Property(e => e.UserIsactive).HasColumnName("user_isactive");

                entity.Property(e => e.UserMobile)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("user_mobile");

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("user_name");

                entity.Property(e => e.UserNjname)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("user_njname");

                entity.Property(e => e.UserPan)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("user_pan");

                entity.Property(e => e.UserParentid).HasColumnName("user_parentid");

                entity.Property(e => e.UserPasswd)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("user_passwd");

                entity.Property(e => e.UserPin)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("user_pin");

                entity.Property(e => e.UserProfilephoto)
                    .IsUnicode(false)
                    .HasColumnName("user_profilephoto");

                entity.Property(e => e.UserPromocode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("user_promocode");

                entity.Property(e => e.UserPurposeid).HasColumnName("user_purposeid");

                entity.Property(e => e.UserSponid).HasColumnName("user_sponid");

                entity.Property(e => e.UserStateid).HasColumnName("user_stateid");

                entity.Property(e => e.UserSubcategory)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("user_subcategory");

                entity.Property(e => e.UserTermandcondition).HasColumnName("user_termandcondition");

                entity.Property(e => e.UserUname)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("user_uname");

                entity.Property(e => e.UserWbcActive).HasColumnName("user_wbcActive");
            });

            modelBuilder.Entity<TblVendorMaster>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tbl_Vendor_Master");

                entity.Property(e => e.ImgPath)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("imgPath");

                entity.Property(e => e.Name)
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblWbcMallCategory>(entity =>
            {
                entity.HasKey(e => e.CatId);

                entity.ToTable("tbl_WBC_Mall_Category");

                entity.Property(e => e.CatId).HasColumnName("cat_id");

                entity.Property(e => e.CatActive).HasColumnName("cat_active");

                entity.Property(e => e.CatImage)
                    .IsUnicode(false)
                    .HasColumnName("cat_image");

                entity.Property(e => e.CatName)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("cat_name");
            });

            modelBuilder.Entity<TblWbcMallProduct>(entity =>
            {
                entity.HasKey(e => e.ProdId);

                entity.ToTable("tbl_WBC_Mall_Products");

                entity.Property(e => e.ProdId).HasColumnName("prod_id");

                entity.Property(e => e.Description)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.ProdAvailableQty).HasColumnName("prod_available_qty");

                entity.Property(e => e.ProdCatId).HasColumnName("prod_cat_id");

                entity.Property(e => e.ProdDateAdded)
                    .HasColumnType("datetime")
                    .HasColumnName("prod_date_added");

                entity.Property(e => e.ProdDiscount).HasColumnName("prod_discount");

                entity.Property(e => e.ProdImage)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("prod_image");

                entity.Property(e => e.ProdName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("prod_name");

                entity.Property(e => e.ProdRating)
                    .HasColumnType("decimal(2, 1)")
                    .HasColumnName("prod_rating");
            });

            modelBuilder.Entity<TblWbcSchemeMaster>(entity =>
            {
                entity.ToTable("tbl_wbc_scheme_master");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Business)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FromDate)
                    .HasColumnType("date")
                    .HasColumnName("From_Date");

                entity.Property(e => e.GoldPoint).HasColumnName("Gold_Point");

                entity.Property(e => e.NoOfContactsAllowed).HasColumnName("No_of_Contacts_Allowed");

                entity.Property(e => e.ParticularsId).HasColumnName("Particulars_Id");

                entity.Property(e => e.ParticularsSubTypeId)
                    .HasColumnName("Particulars_SubType_Id");

                entity.Property(e => e.ToDate)
                    .HasColumnType("date")
                    .HasColumnName("To_Date");

                entity.Property(e => e.WbcTypeId).HasColumnName("Wbc_Type_id");
            });

            modelBuilder.Entity<TblWbcTypeMaster>(entity =>
            {
                entity.ToTable("tbl_wbc_type_master");

                entity.Property(e => e.WbcType)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("wbc_type");
            });

            modelBuilder.Entity<Usercleantable>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("usercleantable");

                entity.Property(e => e.CatId).HasColumnName("cat_id");

                entity.Property(e => e.UseAdhar)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("use_adhar");

                entity.Property(e => e.UserAdd)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("user_add");

                entity.Property(e => e.UserAddline1)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("user_addline1");

                entity.Property(e => e.UserAddline2)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("user_addline2");

                entity.Property(e => e.UserAddline3)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("user_addline3");

                entity.Property(e => e.UserCityid).HasColumnName("user_cityid");

                entity.Property(e => e.UserCityname)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("user_cityname");

                entity.Property(e => e.UserCountryid).HasColumnName("user_countryid");

                entity.Property(e => e.UserDob)
                    .HasColumnType("date")
                    .HasColumnName("user_dob");

                entity.Property(e => e.UserEmail)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("user_email");

                entity.Property(e => e.UserFirstname)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("user_firstname");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.UserLastname)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("user_lastname");

                entity.Property(e => e.UserMiddlename)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("user_middlename");

                entity.Property(e => e.UserMobile)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("user_mobile");

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("user_name");

                entity.Property(e => e.UserPan)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("user_pan");

                entity.Property(e => e.UserParentid).HasColumnName("user_parentid");

                entity.Property(e => e.UserPasswd)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("user_passwd");

                entity.Property(e => e.UserPin).HasColumnName("user_pin");

                entity.Property(e => e.UserPromocode)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("user_promocode");

                entity.Property(e => e.UserSponid).HasColumnName("user_sponid");

                entity.Property(e => e.UserStateid).HasColumnName("user_stateid");

                entity.Property(e => e.UserStatename)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("user_Statename");

                entity.Property(e => e.UserSubcategory).HasColumnName("user_subcategory");

                entity.Property(e => e.UserUname)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("user_uname");
            });

            modelBuilder.Entity<TblLoanMaster>(entity =>
            {
                entity.ToTable("tbl_Loan_Master");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Emi).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Frequency)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.IsKathrough).HasColumnName("IsKAThrough");

                entity.Property(e => e.LoanAmount).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.MaturityDate).HasColumnType("date");

                entity.Property(e => e.RateOfInterest).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.StartDate).HasColumnType("date");
            });

            modelBuilder.Entity<TblLoanTypeMaster>(entity =>
            {
                entity.ToTable("tbl_Loan_Type_Master");

                entity.Property(e => e.LoanType)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.HasSequence("OrderId")
                .HasMin(1)
                .HasMax(100000);

            modelBuilder.HasSequence<int>("realEastateId")
                .HasMin(1)
                .HasMax(100000);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
