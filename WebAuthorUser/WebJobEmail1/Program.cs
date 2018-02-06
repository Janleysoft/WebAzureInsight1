using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json.Linq;

namespace WebJobEmail1
{
    // To learn more about Microsoft Azure WebJobs SDK, please see https://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        static void Main()
        {
            var config = new JobHostConfiguration();

            var host = new JobHost();
            host.Start();
            var cpuAvg = EmailCheck();
            if (cpuAvg > 0.10)
            {
                // Console.WriteLine("The process cpu percentage over 0.10");
                var emailStatus = InsightAPI.WarnEmail(cpuAvg);
                Console.WriteLine(emailStatus);
            }
            host.Stop();
            //MyQueue(cpuAvg);
            // host.RunAndBlock();
        }
        public static double EmailCheck()
        {
            var json = InsightAPI.GetTelemetry
                ("a38047dd-f0a9-47d4-b398-e8c57dec9ed3", "83e6cxyav0bxwzvglxkqxn46bxcypb760d66c51v", "");
            var result = JObject.Parse(json);
            var startTime = (string)result["value"]["start"];
            Console.WriteLine("start time:" + startTime);
            var endTime = (string)result["value"]["end"];
            Console.WriteLine("end time:" + endTime);
            var cpuAvg = (double)result["value"]["performanceCounters/processCpuPercentage"]["avg"];
            Console.WriteLine("process cpu avg:" + cpuAvg);
            return cpuAvg;
        }
        public static void MyQueue(double cpuAvg)
        {
            string connectionString = AmbientConnectionStringProvider.Instance.GetConnectionString(ConnectionStringNames.Storage);
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference("myqueue");
            queue.CreateIfNotExists();
            CloudQueueMessage message = new CloudQueueMessage(cpuAvg.ToString());
            queue.AddMessage(message);
            Console.WriteLine("Add queue");
        }
    }
}
