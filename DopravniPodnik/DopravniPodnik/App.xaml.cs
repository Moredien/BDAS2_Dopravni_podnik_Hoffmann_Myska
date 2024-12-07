using System;
using System.Diagnostics;
using System.Windows;
using DopravniPodnik.Data.service;
using DopravniPodnik.Utils;
using DotNetEnv;

namespace DopravniPodnik;

public partial class App : Application
{
    public static UserSession UserSessionInstance { get; private set; } = null!;
    public static Logger LoggerInstance { get; private set; } = null!;

    [STAThread]
    public static void Main()
    {
        Env.Load("../../../.env");

        UserSessionInstance = UserSession.Instance;
        UserSessionInstance.UpdateSession("default", null);
        LoggerInstance = Logger.Instance;

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
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Add a trace listener for WPF binding errors
        PresentationTraceSources.DataBindingSource.Listeners.Add(new ConsoleTraceListener());
        PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Warning | SourceLevels.Error;
    }
}