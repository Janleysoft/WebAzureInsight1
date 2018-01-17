using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace WebJobAuthor1
{
    // https://blogs.msdn.microsoft.com/mustafakasap/2015/12/26/azure-webjobs-jobhostconfiguration/
    class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            JobHostConfiguration config = new
            JobHostConfiguration();
            //config.Queues.BatchSize = 8;
            //config.Queues.MaxDequeueCount = 1;
            // config.Queues.MaxPollingInterval = TimeSpan.FromSeconds(4);
            // DemoData();
            GetBlob();
            var host = new JobHost();
            Console.WriteLine("web job1");
            // The following code ensures that the WebJob will be running continuously
            host.RunAndBlock();
        }
        public static void DemoData()
        {
            string connectionString = AmbientConnectionStringProvider.Instance.GetConnectionString(ConnectionStringNames.Storage);
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference("initialorder");
           // queue.CreateIfNotExists();
            Student student = new Student()
            {
                Id = 1,
                Name = "peter33",
                Address = "ShangHai"
            };
              queue.AddMessage(new CloudQueueMessage(JsonConvert.SerializeObject(student)));
            // queue.AddMessage(new CloudQueueMessage("bb"));
        }
        public static List<string> GetBlob()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
    Microsoft.Azure.CloudConfigurationManager.GetSetting("AzureWebJobsStorage"));

            //Create service client for credentialed access to the Blob service.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            //Get a reference to a container.
            CloudBlobContainer container = blobClient.GetContainerReference("container002");
            var list = container.ListBlobs(useFlatBlobListing: true);
            var listOfFileNames = new List<string>();

            foreach (var blob in list)
            {
                var blobFileName = blob.Uri.Segments.Last();
                Console.WriteLine(blobFileName);
                listOfFileNames.Add(blobFileName);
                
            }

            return listOfFileNames;
        }
      
      
    }
}
