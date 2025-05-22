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
        private void DeadCodeFunction1()
        {
            string userId = "123"; // Example user input
            string query = "SELECT * FROM Users WHERE UserId = '" + userId + "'"; // Vulnerable to SQL Injection
            // In a real scenario, this query would be executed against a database.
            // For testing purposes, we just define it.
            Console.WriteLine($"Executing query: {query}");
            int x = 42;
            string msg = "This is dead code with a SQL injection vulnerability.";
        }

        private static void VulnerableSqlFunctionExample()
        {
            // Simulate getting user input (e.g., from a web request or other source)
            string username = "testUser' OR '1'='1"; // Example of malicious input

            // Simulate a flawed sanitization attempt, similar to the Python example
            string supposedlySafeInput = FakeSanitizeUserInput(username);
 
            // Vulnerable query construction using string concatenation
            string query = "SELECT * FROM users WHERE username = '" + supposedlySafeInput + "'";
    
            Console.WriteLine($"Simulating execution of vulnerable query: {query}");
            // In a real scenario, this query would be executed against a database.
            // For example, using System.Data.SqlClient.SqlCommand or an ORM like EF Core.
            // e.g., using (var command = new System.Data.SqlClient.SqlCommand(query, connection)) { ... }
        }

        private static string FakeSanitizeUserInput(string userInput)
        {
            // This function mimics an ineffective sanitization attempt,
            // similar to the Python example's 'fake_sanitize_input'.
            // It appends a string, which does not prevent SQL injection.
            return userInput + "someSuffixThatDoesNotHelp";
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
