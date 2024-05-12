using Microsoft.Extensions.Configuration;

namespace TodoUI;

internal static class Program
{
    public static IConfiguration Configuration { get; private set; }

    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        // Konfiguration laden
        Configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();


        TodoLibrary.GlobalConfig.InitializeConnections();


        Application.Run(new TodoViewerForm());
    }
}
