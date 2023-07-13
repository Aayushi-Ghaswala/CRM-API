using CsvHelper;
using DocumentFormat.OpenXml.Office2010.PowerPoint;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace CRM_api.Services.Helper.File_Helper
{
    public class GetCSVHelper<T>
    {
        public byte[] WriteCSVFile(List<T> records)
        {
            using (var memoryStream = new MemoryStream())
            using (var writer = new StreamWriter(memoryStream))
            {
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(records);
                    writer.Flush();
                    return memoryStream.ToArray();
                }
            }
        }
    }
}
