using Aspose.Cells;
using ConsoleApp1.Model;
using Dapper;
using Microsoft.Data.SqlClient;
using mwbFairtradeScript.Model;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Xml;
using static Dapper.SqlMapper;

//date format yyyy-mm-dd for example (2024-03-01)
namespace ConsoleApp1.Data {
    public class DataManipulation {

        private readonly DapperDBContext context;
        public DataManipulation(DapperDBContext context) {
            this.context = context;
        }

        //private string ConnectionString { get; set; }

        //public DataManipulation(string connectionString) {
        //    ConnectionString = connectionString;
        //}


        ////LAST PRICES
        //public double LastBuyPricePerDay(string date, string ISIN) {
        //    using (var connection = new SqlConnection(ConnectionString)) {
        //        connection.Open();
        //        string query = $"SELECT TOP 1 date, time, amount, execution_price, volume, side, isin FROM Trades WHERE date = '{date}' AND isin = '{ISIN}' and side = 'BUY' ORDER BY time DESC";

        //        Trade trade = (Trade)connection.QueryFirstOrDefault<Trade>(query);
        //        return trade.Execution_price;
        //    }
        //    // If no match found, you might want to handle this case appropriately.
        //    throw new InvalidOperationException("No matching record found.");
        //}

        //public double LastSellPricePerDay(string date, string ISIN) {
        //    using (var connection = new SqlConnection(ConnectionString)) {
        //        connection.Open();
        //        string query = $"SELECT TOP 1 date, time, amount, execution_price, volume, side, isin FROM Trades WHERE date = '{date}' AND isin = '{ISIN}' and side = 'SELL' ORDER BY time DESC";

        //        Trade trade = (Trade)connection.QueryFirstOrDefault<Trade>(query);
        //        return trade.Execution_price;
        //    }
        //    // If no match found, you might want to handle this case appropriately.
        //    throw new InvalidOperationException("No matching record found.");
        //}


        //NETTEN
        public ResultTrade GetNetten(string isin, double maxVolume, ref int count) {
            double nettingProfit = 0;
            double closingProfit = 0;
            int closingCounter = 0;
            double totalVolume = 0;
            int orderCounter = 0;
            
            using (var connection = context.CreateConnection()) {
                connection.Open();
                totalVolume = GetTotalVolume(isin, connection);
                orderCounter = GetOrderCounter(isin, connection);
                List<string> dateListAsString = GetUniqueDateList(isin, connection);

                for (int i = 0; i < dateListAsString.Count; i++) {
                    List<Trade> trades = GetTradesProDay(dateListAsString[i], isin, connection);
                    double buyAmount = 0;
                    double sellAmount = 0;
                    double buyVolume = 0;
                    double sellVolume = 0;
                    double averageBuyPrice = 0;
                    double averageSellPrice = 0;

                    for (int j = 0; j < trades.Count;j++) {
                        double amount = trades[j].Amount;
                        double volume = trades[j].Volume;
                        
                        if (trades[j].Side.Equals("BUY")) {
                            buyAmount += amount;
                            buyVolume += volume;
                            averageBuyPrice = buyVolume / buyAmount;
                        } else if(trades[j].Side.Equals("SELL")) {
                            sellAmount += amount;
                            sellVolume += volume;
                            averageSellPrice = sellVolume / sellAmount;
                        }

                        if (Math.Abs(buyVolume - sellVolume) > maxVolume) {
                            if (buyAmount < sellAmount) {
                                nettingProfit += buyAmount * (averageBuyPrice - averageSellPrice);
                                closingProfit += (sellAmount - buyAmount) * (trades[j].Execution_price -                     averageSellPrice);
                            }else if (sellAmount < buyAmount) {
                                nettingProfit += sellAmount * (averageBuyPrice - averageSellPrice);
                                closingProfit += (buyAmount - sellAmount) * (trades[j].Execution_price -                    averageSellPrice);
                            }

                            buyAmount = 0;
                            sellAmount = 0;
                            buyVolume = 0;
                            sellVolume = 0;
                            averageBuyPrice = 0;
                            averageSellPrice = 0;
                            closingCounter++;
                        }
                    }

                    if (buyAmount < sellAmount) {
                        nettingProfit += buyAmount * (averageBuyPrice - averageSellPrice);
                        closingProfit += (sellAmount - buyAmount) * (trades[trades.Count - 1].Execution_price - averageSellPrice);
                    } else if (sellAmount < buyAmount) {
                        nettingProfit += sellAmount * (averageBuyPrice - averageSellPrice);
                        closingProfit += (buyAmount - sellAmount) * (trades[trades.Count - 1].Execution_price - averageSellPrice);
                    }
                    closingCounter++;
                }
            }

            Console.WriteLine(count++);

            return new ResultTrade(isin, totalVolume, closingCounter, orderCounter, nettingProfit);
        }
       

        private List<string> GetUniqueDateList(string isin, IDbConnection connection) {
            List<string> dateListAsString;
           
            string query = $"select distinct date from Trades where isin = '{isin}'";
            var dateListAsDate = connection.Query<DateTime>(query);
            dateListAsString = new List<string>();
            foreach (var obj in dateListAsDate) {
                DateTime.ParseExact(obj.ToString(), "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                dateListAsString.Add(obj.ToString("M.d.yyyy"));
            }
            return dateListAsString;
        }

        private List<Trade> GetTradesProDay(string date, string isin, IDbConnection connection) {
            List<Trade> trades = new List<Trade>();

            string tradesQuery = $"SELECT date, time, amount, execution_price, volume, side, isin FROM Trades WHERE date = '{date}' AND isin = '{isin}'";
            var tradeList = connection.Query<Trade>(tradesQuery);
            foreach (var obj in tradeList) {
                trades.Add(obj);
            }
            return trades;
        }

        private double GetTotalVolume(string isin, IDbConnection connection) {
            string volumeQuery = $"select sum(volume) from Trades where isin = '{isin}'";
            double volume = connection.QueryFirstOrDefault<double>(volumeQuery);
            return volume;
        }

        private int GetOrderCounter(string isin, IDbConnection connection) {
            string orderCounterQuery = $"select count(*) from Trades where isin = '{isin}'";
            int orderCounter = connection.QueryFirstOrDefault<int>(orderCounterQuery);
            return orderCounter;
        }
    }
}

