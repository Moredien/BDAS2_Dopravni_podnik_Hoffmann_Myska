using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using DopravniPodnik.Data.service;
using DopravniPodnik.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DotNetEnv;

namespace DopravniPodnik
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        [STAThread]
        public static void Main()
        {
            Env.Load("../../../.env");
            var connectionString = Environment.GetEnvironmentVariable("ORACLE_DB_CONNECTION");

            if (string.IsNullOrWhiteSpace(connectionString)) // Pokud se connection string nenacte nema smysl poustet aplikaci
            {
                Console.WriteLine("Oracle DB connection string is empty.");
                Environment.Exit(-1);
            }
            
            var services = ConfigureServices(connectionString);
            
            Task.Run(() => RetryDatabaseConnection(services));
            
            var app = new App();
            app.InitializeComponent();
            app.Run();
        }

        private static ServiceProvider ConfigureServices(string connectionString)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDbContext<OracleDbContext>(options => options.UseOracle(connectionString));
            
            serviceCollection.AddSingleton<DatabaseService>();
            serviceCollection.AddSingleton<AuthService>();
            serviceCollection.AddSingleton<UserSession>();
            
            return serviceCollection.BuildServiceProvider();
        }

        private static async Task RetryDatabaseConnection(ServiceProvider services)
        {
            var dbService = services.GetRequiredService<DatabaseService>();

            while (true)
            {
                try
                {
                    if (dbService.TestConnection())
                    {
                        Console.WriteLine("Database connection successful.");
                        break;
                    }

                    Console.WriteLine("Database connection failed. Retrying...");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Database connection exception: {ex.Message}");
                }
                
                await Task.Delay(5000); // Proto aby se to opakovalo jednou za 5 sekund
            }
        }
    }
}
