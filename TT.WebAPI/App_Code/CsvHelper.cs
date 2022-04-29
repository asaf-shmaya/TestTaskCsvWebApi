using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using TT.Models;

namespace TT.WebAPI.App_Code
{
    public static class CsvHelper
    {
        public static IEnumerable<Invoice> GetCSV(string csvFilePath)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
                NewLine = Environment.NewLine,
                Delimiter = ";",
            };
            using (var reader = new StreamReader(csvFilePath))
            using (var csv = new CsvReader(reader, config))
            {
                return csv.GetRecords<Invoice>().ToList();
            }
        }
    }
}
