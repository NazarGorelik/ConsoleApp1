using ConsoleApp1.Model;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ConsoleApp1.Data {

    /*
     * class to read rows from csv, convert these rows to objects of class Trades and insert these objects into sql table using BulkInsertion
     */
    public class DataTransfer {

        private static DataTable CreateDataTableForTrades() {
            var dataTable = new DataTable();
            dataTable.Columns.Add("date", typeof(DateTime));
            dataTable.Columns.Add("time", typeof(DateTime));
            dataTable.Columns.Add("amount", typeof(double));
            dataTable.Columns.Add("execution_price", typeof(double));
            dataTable.Columns.Add("volume", typeof(double));
            dataTable.Columns.Add("side", typeof(string));
            dataTable.Columns.Add("isin", typeof(string));
            return dataTable;
        }

        private static void MapTradesToDataTable(List<Trade> trades, DataTable dataTable) {
            foreach (var trade in trades) {
                dataTable.Rows.Add(trade.Date, trade.Time, trade.Amount, trade.Execution_price, trade.Volume, trade.Side, trade.Isin);
            }
        }

        public static void WriteDataToSqlTable(List<Trade> trades, string connectionString) {

            using (var connection = new SqlConnection(connectionString)) {
                connection.Open();

                using (var transaction = connection.BeginTransaction()) {
                    using (var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction)) {
                        bulkCopy.BulkCopyTimeout = 600;
                        bulkCopy.DestinationTableName = "Trades";
                        var dataTable = CreateDataTableForTrades();
                        MapTradesToDataTable(trades, dataTable);
                        bulkCopy.WriteToServer(dataTable);
                    }
                    transaction.Commit();
                }
            }
        }
    }
}
