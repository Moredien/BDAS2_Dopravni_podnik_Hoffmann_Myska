using System.Windows;
using DopravniPodnik.Data.service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DotNetEnv;
using System.Diagnostics;

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
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                Console.WriteLine("Oracle DB connection string is empty.");
                Environment.Exit(-1);
            }
            
            var services = ConfigureServices(connectionString);
            var dbService = services.GetRequiredService<DatabaseService>();
            
            //Test pripojeni k db
            if (dbService.TestConnection())
            {
                Console.WriteLine("Database connection successful.");
                
                var app = new App();
                app.InitializeComponent();
                app.Run();
            }
            else
            {
                Environment.Exit(1); //TODO: Zatim se to vypne pokud se to nepripoji. Predelat!
            }
        }

        private static ServiceProvider ConfigureServices(string connectionString)
        {
            var serviceCollection = new ServiceCollection();
            
            serviceCollection.AddDbContext<OracleDbContext>(options => options.UseOracle(connectionString));
            serviceCollection.AddSingleton<DatabaseService>();

            return serviceCollection.BuildServiceProvider();
        }
    }

}
