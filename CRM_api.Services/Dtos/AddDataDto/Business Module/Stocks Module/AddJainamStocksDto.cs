using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CRM_api.Services.Dtos.AddDataDto.Business_Module.Stocks_Module
{
    public class AddJainamStocksDto
    {
        public string ScriptName { get; set; }
        public string Company { get; set; }
        public DateTime Date { get; set; }
        public string Narration { get; set; }
        public int BuyQty { get; set; }
        public double BuyNetRate { get; set; }
        public int SellQty { get; set; }
        public double SellNetRate { get; set; }
        public double NetRate { get; set; }
        public decimal NetAmount { get; set; }
        public string FirmName { get; set; }

        public AddJainamStocksDto()
        {
            
        }

        public AddJainamStocksDto(AddJainamStocksDto stockObject)
        {
            ScriptName = stockObject.ScriptName;
            Company = stockObject.Company;
            Date = stockObject.Date;
            Narration = stockObject.Narration;
            BuyQty = stockObject.BuyQty;
            BuyNetRate = stockObject.BuyNetRate;
            SellQty = stockObject.SellQty;
            SellNetRate = stockObject.SellNetRate;
            NetRate = stockObject.NetRate;
            NetAmount = stockObject.NetAmount;
        }
    }
}
