using System.Globalization;

namespace CRM_api.Services.Dtos.AddDataDto.Business_Module.MutualFunds_Module
{
    public class AddAMFINAVDto
    {
        public string? SchemeCode { get; set; }
        public string? Isin { get; set; }
        public string? SchemeName { get; set; }
        public string? NetAssetValue { get; set; }
        public DateTime? Date { get; set; }

        public AddAMFINAVDto(string? schemeCode, string? isin, string? schemeName, string? netAssetValue, string? date)
        {
            SchemeCode = schemeCode;
            Isin = isin;
            SchemeName = schemeName;
            NetAssetValue = netAssetValue;
            Date = DateTime.ParseExact(date, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
        }
    }
}
