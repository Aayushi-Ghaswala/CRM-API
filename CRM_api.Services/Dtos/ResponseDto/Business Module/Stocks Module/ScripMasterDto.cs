﻿namespace CRM_api.Services.Dtos.ResponseDto.Business_Module.Stocks_Module
{
    public class ScripMasterDto
    {
        public string? Scripsymbol { get; set; }
        public string? Scripname { get; set; }
        public string? Isin { get; set; }
        public string? Exchange { get; set; }
        public decimal? Ltp { get; set; }
        public DateTime? Ltt { get; set; }
        public DateTime? Date { get; set; }
    }
}
