using System;
using System.Threading.Tasks;
using HostingAzureOpenAiSample.WebApp.CompositionRoot;
using Microsoft.AspNetCore.Builder;
using Serilog;

namespace HostingAzureOpenAiSample.WebApp;

public static class Program
{
    public static async Task<int> Main(string[] args)
    {
        Log.Logger = Logging.CreateLogger();
        try
        {
            var app = WebApplication.CreateBuilder(args)
                                    .ConfigureServices(Log.Logger)
                                    .Build()
                                    .ConfigureMiddleware();
            await app.RunAsync();
            return 0;
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "Host terminated unexpectedly");
            return 1;
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
    }
}