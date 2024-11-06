using System.Windows;
using DopravniPodnik.Data.service;
using DopravniPodnik.Utils;
using DotNetEnv;

namespace DopravniPodnik
{
    public partial class App : Application
    {
        [STAThread]
        public static void Main()
        {
            Env.Load("../../../.env");
            
            var dbService = new DatabaseService();
            RetryDatabaseConnection(dbService);

            var app = new App();
            app.InitializeComponent();
            app.Run();
        }

        private static void RetryDatabaseConnection(DatabaseService dbService)
        {
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
            }
        }
    }
}