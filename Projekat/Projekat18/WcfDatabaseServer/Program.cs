using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WcfDatabaseServer.Services;

namespace WcfDatabaseServer
{
    public class Program
    {
        static void Main(string[] args)
        {
            string input;

            do
            {
                Console.WriteLine("Enter file format(CSV, XML or JSON): ");
                input = Console.ReadLine();
            } while (!input.ToLower().Equals("xml") && !input.ToLower().Equals("csv") && !input.ToLower().Equals("json"));

            DatabaseService.ConfigureFormat(input);

            using (ServiceHost host = new ServiceHost(typeof(DatabaseService)))
            {
                host.Open();
                Console.WriteLine("WCF server running at http://localhost:8081/DatabaseService");
                Console.WriteLine("Press Enter to exit...");
                Console.ReadLine();
            }
        }
    }
}
