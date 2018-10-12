
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
using labo3.model;
using System.Collections.Generic;

namespace labo3
{
    public static class getVisitors
    {
        [FunctionName("getVisitors")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "visitors/{day}")]HttpRequest req, int day, ILogger log)
        {
            string connectionString = Environment.GetEnvironmentVariable("ConnectionString");
            List<visit> visits = new List<visit>();

            using(SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = connectionString;
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT * FROM Registraties WHERE Dagweek = {day}";
                    command.Parameters.AddWithValue("@day", day);

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        visit v = new visit();
                        v.AantaBezoekers = int.Parse(reader["AantalBezoekers"].ToString());
                        v.TijdstipDag = int.Parse(reader["TijdstipDag"].ToString());
                        v.Dagweek = reader["Dagweek"].ToString();

                        visits.Add(v);

                    }
                }
            }
        return new OkObjectResult(visits);
        }
    }
}
