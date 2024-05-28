using CsvHelper.Configuration.Attributes;

namespace ConsoleApp1.Model {

    public class Trade {
        //public int Id { get; set; }
        [Name("date")]
        public DateTime Date { get; set; }

        [Name("time")]
        public DateTime Time { get; set; }

        [Name("amount")]
        public double Amount { get; set; }

        [Name("execution_price")]
        public double Execution_price { get; set; }

        [Name("volume")]
        public double Volume { get; set; }

        [Name("side")]
        public string Side { get; set; }

        [Name("isin")]
        public string Isin { get; set; }

        public override string ToString() {
            return string.Format($"Trade: \n date: {Date.ToString()}, time: {Time.ToString()}, amount: {Amount}, execution_price: {Execution_price}, volume: {Volume}, side: {Side}, isin: {Isin}");
        }
    }
}
