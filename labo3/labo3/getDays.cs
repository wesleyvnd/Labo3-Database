
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace labo3
{
    public static class getDays
    {
        [FunctionName("getDays")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "Days")]HttpRequest req, ILogger log)
        {
            list<string> days = new list<string>();
            string connectionString = Environment.GetEnvironmentVariable("");

            // SqlConnection connection = new SqlConnection();
            // connection.dispose() --> ruimt alles op.
            // betere verbinding is: 
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = connectionString;
                await connection.OpenAsync();

                using(SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT DISTINCT Dagweek FROM [Registraties]";

                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        string day = reader["Dagweek"].ToString();
                        days.Add(day);
                    }
                }
            }
                return OkObjectResult(days());           
        }
    }
}
