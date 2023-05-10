using System;
using System.Collections.Generic;

namespace CRM_api.DataAccess.Models
{
    public partial class TblSubInvesmentType
    {
        public int Id { get; set; }
        public string InvestmentType { get; set; } = null!;
        public int InvesmentTypeId { get; set; }

        public virtual TblInvesmentType InvesmentType { get; set; } = null!;
    }
}
