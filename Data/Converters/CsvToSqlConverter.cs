using ConsoleApp1.Model;
using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Data.Converters {
    public class CsvToSqlConverter {

        public static List<Trade> GetTradesFromCSV() {
            string csvFilePath = "C:\\Users\\Mi\\Desktop\\mwbFairtrade\\mwbFairtrade.csv";
            using (var reader = new StreamReader(csvFilePath)) {
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture))) {
                    csv.Context.RegisterClassMap<CsvRecordMap>();
                    return csv.GetRecords<Trade>().ToList();
                }
            }
        }
    }
}
