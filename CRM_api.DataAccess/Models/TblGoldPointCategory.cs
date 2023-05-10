using System;
using System.Collections.Generic;

namespace CRM_api.DataAccess.Models
{
    public partial class TblGoldPointCategory
    {
        public TblGoldPointCategory()
        {
            TblGoldPoints = new HashSet<TblGoldPoint>();
        }

        public int Id { get; set; }
        public string PointCategory { get; set; } = null!;

        public virtual ICollection<TblGoldPoint> TblGoldPoints { get; set; }
    }
}
