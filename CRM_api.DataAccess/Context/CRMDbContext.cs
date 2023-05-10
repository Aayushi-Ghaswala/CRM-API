using CRM_api.DataAccess.Model;
using Microsoft.EntityFrameworkCore;

namespace CRM_api.DataAccess.Context
{
    public partial class CRMDbContext : DbContext
    {
        public CRMDbContext()
        {

        }
        public CRMDbContext(DbContextOptions<CRMDbContext> options) : base(options)
        {

        }

        public virtual DbSet<CityMaster> CityMasters { get; set; }
        public virtual DbSet<StateMaster> StateMasters { get; set; }
        public virtual DbSet<CountryMaster> CountryMasters { get; set;}
        public virtual DbSet<UserCategoryMaster> UserCategoryMasters { get; set; }
        public virtual DbSet<UserMaster> UserMasters { get; set; }
        public virtual DbSet<RoleMaster> RoleMasters { get; set; }
        public virtual DbSet<RolePermission> RolePermissions { get; set; }  
        public virtual DbSet<UserRoleAssignment> UserRoleAssignments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer("Name=DefaultConnection");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CountryMaster>(entity =>
            {
                entity.ToTable("tbl_Country_Master");
                entity.Property(e => e.Id).HasColumnName("country_id");
                entity.Property(e => e.Country_Name).HasMaxLength(50).HasColumnName("country_name");
                entity.Property(e => e.Isdcode).HasMaxLength(50).HasColumnName("isdcode");
                entity.Property(e => e.Icon).HasColumnName("icon");
            });

            modelBuilder.Entity<StateMaster>(entity =>
            {
                entity.ToTable("tbl_StateMaster");
                entity.Property(e => e.Id).HasColumnName("state_id");
                entity.Property(e => e.State_Name).HasMaxLength(50).HasColumnName("state_name");
                entity.Property(e => e.Country_Id).HasColumnName("country_id");
                entity.HasOne(c => c.CountryMaster)
                    .WithMany(s => s.StateMasters)
                    .HasForeignKey(s => s.Country_Id)
                    .HasConstraintName("FK_tbl_StateMaster_kaadmin.tbl_Country_Master");
            });

            modelBuilder.Entity<CityMaster>(entity =>
            {
                entity.ToTable("tbl_City_Master");
                entity.Property(e => e.Id).HasColumnName("city_id");
                entity.Property(e => e.City_Name).HasMaxLength(50).HasColumnName("city_name");
                entity.Property(e => e.State_Id).HasColumnName("state_id");
                entity.HasOne(c => c.StateMaster)
                    .WithMany(s => s.CityMasters)
                    .HasForeignKey(s => s.State_Id)
                    .HasConstraintName("FK_tbl_City_Master_tbl_StateMaster");
            });

            modelBuilder.Entity<UserCategoryMaster>(entity =>
            {
                entity.ToTable("tbl_User_Category_Master");
                entity.Property(e => e.Cat_Id).HasColumnName("cat_id");
                entity.Property(e => e.Cat_Name).HasMaxLength(100).HasColumnName("cat_name");
                entity.Property(e => e.Cat_IsActive).HasColumnName("cat_isactive");
            });

            modelBuilder.Entity<UserMaster>(entity =>
            {
                entity.ToTable("tbl_User_Master");
                entity.Property(e => e.User_Id).HasColumnName("user_id");
                entity.Property(e => e.Cat_Id).HasColumnName("cat_id");
                entity.Property(e => e.User_SponId).HasColumnName("user_sponid");
                entity.Property(e => e.User_ParentId).HasColumnName("user_parentid");
                entity.Property(e => e.User_Name).HasMaxLength(100).HasColumnName("user_name");
                entity.Property(e => e.User_Pan).HasMaxLength(50).HasColumnName("user_pan");
                entity.Property(e => e.User_Doj).HasColumnType("datetime").HasColumnName("user_doj");
                entity.Property(e => e.User_Mobile).HasMaxLength(15).HasColumnName("user_mobile");
                entity.Property(e => e.User_Email).HasMaxLength(100).HasColumnName("user_email");
                entity.Property(e => e.User_Addr).HasMaxLength(300).HasColumnName("user_addr");
                entity.Property(e => e.User_Pin).HasMaxLength(10).HasColumnName("user_pin");
                entity.Property(e => e.CountryId).HasColumnName("user_countryid");
                entity.Property(e => e.StateId).HasColumnName("user_stateid");
                entity.Property(e => e.CityId).HasColumnName("user_cityid");
                entity.Property(e => e.User_Uname).HasMaxLength(50).HasColumnName("user_uname");
                entity.Property(e => e.User_Passwd).HasMaxLength(25).HasColumnName("user_passwd");
                entity.Property(e => e.User_IsActive).HasColumnName("user_isactive");
                entity.Property(e => e.User_PurposeId).HasColumnName("user_purposeid");
                entity.Property(e => e.User_ProfilePhoto).HasColumnName("user_profilephoto");
                entity.Property(e => e.User_PromoCode).HasMaxLength(50).HasColumnName("user_promocode");
                entity.Property(e => e.User_SubCategory).HasMaxLength(50).HasColumnName("user_subcategory");
                entity.Property(e => e.User_GstNo).HasMaxLength(20).HasColumnName("user_gstno");
                entity.Property(e => e.User_FcmId).HasColumnName("user_fcmid");
                entity.Property(e => e.User_FcmLastUpdateDateTime).HasColumnType("datetime").HasColumnName("user_fcmlastupdatetime");
                entity.Property(e => e.User_Dob).HasColumnType("date").HasColumnName("user_dob");
                entity.Property(e => e.User_Aadhar).HasMaxLength(15).HasColumnName("user_adhar");
                entity.Property(e => e.User_AccountType).HasColumnName("user_accounttype");
                entity.Property(e => e.User_fastTrack).HasColumnName("user_fasttrack");
                entity.Property(e => e.User_WbcActive).HasColumnName("user_wbcActive");
                entity.Property(e => e.TotalCountofAddContact).HasColumnName("Totalcountofaddcontact");
                entity.Property(e => e.User_Deviceid).HasMaxLength(255).HasColumnName("user_deviceid");
                entity.Property(e => e.User_TermAndCondition).HasColumnName("user_termandcondition");
                entity.Property(e => e.Family_Id).HasColumnName("family_id");
                entity.Property(e => e.User_NjName).HasMaxLength(100).HasColumnName("user_njname");
                entity.Property(e => e.FastTrackActivationDate).HasColumnType("datetime").HasColumnName("fastTrackActivationDate");
            });

            modelBuilder.Entity<RoleMaster>(entity =>
            {
                entity.ToTable("tbl_Role_Master");
                entity.Property(e => e.RoleName).HasMaxLength(100);
            });

            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.ToTable("tbl_Role_Permissions");
                entity.Property(e => e.ModuleName).HasMaxLength(100);
            });

            modelBuilder.Entity<UserRoleAssignment>(entity =>
            {
                entity.ToTable("tbl_User_Role_Assignment");
            });

            OnModelCreatingPartial(modelBuilder);
        }
         partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
