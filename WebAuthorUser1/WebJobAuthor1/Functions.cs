using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;

namespace WebJobAuthor1
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
    public class Functions
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        //public static void ProcessQueueMessage([QueueTrigger("initialorder")] string message, TextWriter log)
        //{
        //    log.WriteLine(message);
        //}
        //public static void MultipleOutput(
        //    [QueueTrigger("initialorder")] Student student,
        //    [Blob("orders/{Name}")] out string studentBlob,
        //    [Queue("myqueue")] out string orders)
        //{
        //    studentBlob = student.Name;
        //    orders = student.Name.ToString();
        //}
        //public static void QueueToBlob(
        //    [QueueTrigger("myqueue")] string orders,
        //    IBinder binder)
        //{
        //    TextWriter writer = binder.Bind<TextWriter>(new BlobAttribute("orders/" + orders));
        //    writer.Write("Completed");
        //}
        /// <summary>
        /// This function will always fail. It is used to demonstrate the poison queue message handling.
        /// After a binding or a function fails 5 times, the trigger message is moved into a poison queue
        /// </summary>
        //public static void FailAlways(
        //    [QueueTrigger("bad")] string message,
        //    int dequeueCount,
        //    TextWriter log)
        //{

        //    throw new InvalidOperationException("Simulated failure");
        //}

        /////// <summary>
        /////// This function will be invoked when a message is put in the poison queue
        /////// </summary>
        //public static void BindToPoisonQueue([QueueTrigger("bad-poison")] string message, TextWriter log)
        //{
        //    log.Write("This message couldn't be processed by the original function: " + message);
        //}
    }
}
