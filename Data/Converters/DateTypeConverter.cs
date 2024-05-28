using CsvHelper.Configuration;
using CsvHelper;
using CsvHelper.TypeConversion;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Data.Converters {
    public class DateTypeConverter : DefaultTypeConverter {

        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData) {
            int dotCount = text.Count(c => c == '.');
            int commaCount = text.Count(c => c == ',');
            if (dotCount == 2) {
                if (commaCount == 1) {
                    return DateTime.ParseExact(text, "d.M.yyyy, HH:mm", CultureInfo.InvariantCulture);
                }
                return DateTime.ParseExact(text, "d.M.yyyy", CultureInfo.InvariantCulture);
            }

            if (dotCount == 1) {
                return Double.Parse(text, CultureInfo.InvariantCulture);
            }
            return text;
        }
    }
}
