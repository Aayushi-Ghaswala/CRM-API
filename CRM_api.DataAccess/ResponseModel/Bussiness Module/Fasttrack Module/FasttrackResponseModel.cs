using CRM_api.DataAccess.Models;

namespace CRM_api.DataAccess.ResponseModel.Bussiness_Module.Fasttrack_Module
{
    public class FasttrackResponseModel
    {
        public int UserId { get; set; }
        public TblUserMaster UserMaster { get; set; }
        public string? UserLevel { get; set; }
        public decimal? Commission { get; set; } = 0;
        public int? GoldPoint { get; set; } = 0;
        public string Narration { get; set; }

        public FasttrackResponseModel(int userId, TblUserMaster userMaster, string userLevel, decimal commission, int? goldPoint, string narration)
        {
            UserId = userId;
            UserLevel = userLevel;
            Commission = commission;
            UserMaster = userMaster;
            GoldPoint = goldPoint;
            Narration = narration;
        }

        public FasttrackResponseModel(int userId, string? userLevel, decimal? commission, int? goldPoint)
        {
            UserId = userId;
            UserLevel = userLevel;
            Commission = commission;
            GoldPoint = goldPoint;
        }
    }
}
