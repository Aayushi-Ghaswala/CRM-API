using System;
using System.Collections.Generic;

namespace CRM_api.DataAccess.Models
{
    public partial class TblUserDepartment
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? DepartmentId { get; set; }
        public bool? Isdeleted { get; set; }
    }
}
