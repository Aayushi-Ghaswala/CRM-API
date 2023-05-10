﻿using System;
using System.Collections.Generic;

namespace CRM_api.DataAccess.Models
{
    public partial class TblInvesmentType
    {
        public TblInvesmentType()
        {
            TblFolioMasters = new HashSet<TblFolioMaster>();
            TblPortfolioReviewRequests = new HashSet<TblPortfolioReviewRequest>();
            TblSubInvesmentTypes = new HashSet<TblSubInvesmentType>();
        }

        public int Id { get; set; }
        public string InvestmentName { get; set; } = null!;

        public virtual ICollection<TblFolioMaster> TblFolioMasters { get; set; }
        public virtual ICollection<TblPortfolioReviewRequest> TblPortfolioReviewRequests { get; set; }
        public virtual ICollection<TblSubInvesmentType> TblSubInvesmentTypes { get; set; }
    }
}
