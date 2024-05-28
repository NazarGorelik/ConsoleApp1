using CsvHelper;
using mwbFairtradeScript.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mwbFairtradeScript.Data.Converters {
    public class ListToCsvFileConverter {

        public static void FillCsvFileFromList(List<ResultTrade> trades) {
            string csvFilePath = "C:\\Users\\Mi\\Desktop\\mwbFairtrade\\mwbFairtradeResult.csv";

            using (var writer = new StreamWriter(csvFilePath)) {
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)) {
                    csv.WriteRecords(trades);
                }
            }
        }
    }
}
