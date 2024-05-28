using ConsoleApp1.Data;
using ConsoleApp1.Data.Converters;
using ConsoleApp1.Model;
using Dapper;
using Microsoft.Data.SqlClient;
using mwbFairtradeScript.Data.Converters;
using mwbFairtradeScript.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

public class Program {


    private static string connectionString = "Data Source=NAZAR228;Initial Catalog=mwbFairtrade;Integrated Security=True;Trust Server Certificate=True";

    private static async Task Main(string[] args) {
        await Host
            .CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) => {
                services.AddHostedService<ConsoleHostedService>();
                services.AddSingleton<DapperDBContext>();
                services.AddSingleton<DataManipulation>();
            })
            .RunConsoleAsync();
    }

    internal sealed class ConsoleHostedService(
        ILogger<ConsoleHostedService> logger,
        IHostApplicationLifetime appLifetime,
        DataManipulation dataManipulation) : IHostedService {

        public Task StartAsync(CancellationToken cancellationToken) {
            logger.LogDebug($"Starting with arguments: {string.Join(" ", Environment.GetCommandLineArgs())}");

            appLifetime.ApplicationStarted.Register(() =>
            {
                Task.Run(async () =>
                {
                    //NETTEN 
                    List<ResultTrade> resultTrades = new List<ResultTrade>();
                    HashSet<string> isinSet = GetAllIsin();
                    List<string> isinList = new List<string>(isinSet);
                    int maxVolume = 20000;
                    int count = 0;

                    for (int i = 0; i < isinList.Count; i++) {
                        resultTrades.Add(dataManipulation.GetNetten(isinList[i], maxVolume, ref count));
                    }

                    FillCsvFileFromList(resultTrades);
                });
            });

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) {
            return Task.CompletedTask;
        }
    }

    //private static void Main(string[] args) {
    //    //run it only once to fill the sql table!!!
    //    //FillSqlTableFromCsv();

    //    DataManipulation dataManipulation = new DataManipulation(context);

    //    //NETTEN 
    //    List<ResultTrade> resultTrades = new List<ResultTrade>();
    //    HashSet<string> isinSet = GetAllIsin();
    //    List<string> isinList = new List<string>(isinSet);
    //    int maxVolume = 20000;
    //    int count = 0;

    //    for (int i = 0; i < isinList.Count; i++) {
    //        resultTrades.Add(dataManipulation.GetNetten(isinList[i], maxVolume, ref count));
    //    }

    //    FillCsvFileFromList(resultTrades);
    //}

    private static void FillSqlTableFromCsv() {
        List<Trade> trades = CsvToSqlConverter.GetTradesFromCSV();
        DataTransfer.WriteDataToSqlTable(trades, connectionString);
    }

    //get unique isin from sqlTable
    private static HashSet<string> GetAllIsin() {
        HashSet<string> isinSet = new HashSet<string>();
        using (var connection = new SqlConnection(connectionString)) {
            connection.Open();
            string tradesQuery = $"SELECT isin FROM Trades";
            var isinListI = connection.Query<string>(tradesQuery);
            foreach (var obj in isinListI) {
                isinSet.Add(obj.ToString());
            }
            return isinSet;
        }
    }

    private static void FillCsvFileFromList(List<ResultTrade> resultTrades) {
        ListToCsvFileConverter.FillCsvFileFromList(resultTrades);
    }
}