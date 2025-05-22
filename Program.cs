using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace dvcsharp_core_api
{
    public class Program
    {
        private static void UnusedMethod() // This should be flagged by IDE0051
        {
            Console.WriteLine("This method is not used.");
        }

        private static void UsedMethod()
        {
            Console.WriteLine("This method is used.");
            int unusedVariable = 5; // This might be flagged by IDE0059 (unnecessary assignment)
                                    // or potentially parts of other rules.
                                    // For unread private members, IDE0052.
        }

        private class UnusedPrivateClass // This should also be flagged by IDE0051
        {
            public int X { get; set; }
        }

        private string UnusedPrivateField = "test"; // IDE0051
        private string _unreadPrivateField = "init"; // IDE0052 (if only written to in constructor)    
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
