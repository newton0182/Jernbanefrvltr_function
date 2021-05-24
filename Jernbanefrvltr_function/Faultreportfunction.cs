using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Jernbanefrvltr_function
{
    public static class Faultreportfunction
    {
        [FunctionName("Faultreportfunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log, ExecutionContext context)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            string todoItemTitle = data?.todoItemTitle;

            string connectionString = config.GetConnectionString("DefaultConnection");

            List<Faultreport> taskList = new List<Faultreport>();
            try
            {

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    var query = @"Select * from Faultreport";
                    SqlCommand command = new SqlCommand(query, conn);
                    var reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        Faultreport task = new Faultreport()
                        {
                            ID = (int)reader["ID"],
                            TraintrackID = reader["TraintrackID"].ToString(),
                            EquipmentID = reader["EquipmentID"].ToString()
                            
                        };
                        taskList.Add(task);
                    }

                }
            }
            catch (Exception e)
            {
                log.LogError(e.ToString());
            }
            if (taskList.Count > 0)
            {
                return new OkObjectResult(taskList);
            }
            else
            {
                return new NotFoundResult();
            }

        }
    }
}
