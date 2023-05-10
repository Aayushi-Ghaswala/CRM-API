using System;
using System.Collections.Generic;

namespace CRM_api.DataAccess.Models
{
    public partial class TblRetirementCalculator
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? CurrentAge { get; set; }
        public int? RetirementAge { get; set; }
        public int? LifeExpectancy { get; set; }
        public int? MonthlyExpenses { get; set; }
        public int? PreRetirementReturn { get; set; }
        public int? PostRetirementReturn { get; set; }
        public int? CurrentInvestment { get; set; }
        public int? InflationRate { get; set; }
    }
}
