using ConsoleApp1.Model;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Data.Converters {
    public sealed class CsvRecordMap : ClassMap<Trade> {

        public CsvRecordMap() {
            AutoMap(CultureInfo.InvariantCulture);
            Map(m => m.Date).TypeConverter(new DateTypeConverter());
            Map(m => m.Time).TypeConverter(new DateTypeConverter());
        }
    }
}
