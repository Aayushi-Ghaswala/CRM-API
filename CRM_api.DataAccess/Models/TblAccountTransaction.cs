using System.ComponentModel.DataAnnotations.Schema;

namespace CRM_api.DataAccess.Models
{
    public partial class TblAccountTransaction
    {
        public int Id { get; set; }
        public DateTime? DocDate { get; set; }
        public string? DocParticulars { get; set; }
        public string? DocType { get; set; }
        public string? DocNo { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
        public int? DocSubType { get; set; }
        public int? DocUserid { get; set; }
        public int? Accountid { get; set; }
        public int? Mgainid { get; set; }
        public int? Companyid { get; set; }
        public string? TransactionType { get; set; }
        public int? Currencyid { get; set; }
        [ForeignKey(nameof(Mgainid))]
        public virtual TblMgaindetail? TblMgaindetail { get; set; }
        [ForeignKey(nameof(DocUserid))]
        public virtual TblUserMaster? UserMaster { get; set; }
        [ForeignKey(nameof(Accountid))]
        public virtual TblAccountMaster? TblAccountMaster { get; set; }
        [ForeignKey(nameof(Companyid))]
        public virtual TblCompanyMaster? CompanyMaster { get; set; }
        [ForeignKey(nameof(Currencyid))]
        public virtual TblMgainCurrancyMaster TblMgainCurrancyMaster { get; set; }

        public TblAccountTransaction()
        {
            
        }
        public TblAccountTransaction(DateTime? docDate, string? docParticulars, string? docType, string? docNo, decimal? debit, decimal? credit, int? docUserid, int? accountid, int? mgainid, int? companyid, string? transactionType, int? currencyid)
        {
            DocDate = docDate;
            DocParticulars = docParticulars;
            DocType = docType;
            DocNo = docNo;
            Debit = debit;
            Credit = credit;
            DocUserid = docUserid;
            Accountid = accountid;
            Mgainid = mgainid;
            Companyid = companyid;
            TransactionType = transactionType;
            Currencyid = currencyid;
        }
    }
}
