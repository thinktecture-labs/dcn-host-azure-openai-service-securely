using Serilog;

namespace HostingAzureOpenAiSample.WebApp.CompositionRoot;

public static class Logging
{
    public static ILogger CreateLogger() =>
        new LoggerConfiguration()
           .WriteTo.Console()
           .CreateLogger();
}