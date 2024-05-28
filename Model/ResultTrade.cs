using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mwbFairtradeScript.Model {
    public class ResultTrade {

        [Name("Isin")]
        public string Isin { get; set; }

        [Name("Volume")]
        public double Volume { get; set; }

        [Name("ClosingCounter")]
        public int ClosingCounter { get; set; }

        [Name("OrderCounter")]
        public int OrderCounter { get; set; }

        [Name("NettingProfit")]
        public double NettingProfit { get; set; }

        public ResultTrade(string isin, double volume, int closingCounter, int orderCounter, double nettingProfit) {
            Isin = isin;
            Volume = volume;
            ClosingCounter = closingCounter;
            OrderCounter = orderCounter;
            NettingProfit = nettingProfit;
        }
    }
}
