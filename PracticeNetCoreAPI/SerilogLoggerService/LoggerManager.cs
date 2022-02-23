using Contracts;
using Serilog;
using Serilog.Events;

namespace LoggerService;

public class LoggerManager : ILogger
{
    private static ILogger logger =
        new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateBootstrapLogger();
    public LoggerManager()
    {

    }
    public void LogDebug(string message)
    {
        logger.Debug(message);
    }
    public void LogError(string message)
    {
        logger.Error(message);
    }
    public void LogInfo(string message)
    {
        logger.Information(message);
    }
    public void LogWarn(string message)
    {
        logger.Warning(message);
    }

    public void Write(LogEvent logEvent)
    {
        throw new NotImplementedException();
    }
}
