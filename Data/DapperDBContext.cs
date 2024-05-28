using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;

namespace ConsoleApp1.Data {
    public class DapperDBContext {

        private readonly IConfiguration configuration;
        private string connectionString = "Data Source=NAZAR228;Initial Catalog=mwbFairtrade;Integrated Security=True;Trust Server Certificate=True";

        public DapperDBContext(IConfiguration configuration) {
            this.configuration = configuration;
        }

        public IDbConnection CreateConnection() => new SqlConnection(connectionString);
    }
}
