using System;
using System.Collections.Generic;

namespace CRM_api.DataAccess.Models
{
    public partial class TblInsurancetype
    {
        public TblInsurancetype()
        {
            TblInsuranceCompanylists = new HashSet<TblInsuranceCompanylist>();
        }

        public int InsuranceCompnytypeid { get; set; }
        public string? InsuranceCompanytypename { get; set; }

        public virtual ICollection<TblInsuranceCompanylist> TblInsuranceCompanylists { get; set; }
    }
}
