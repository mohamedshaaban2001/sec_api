using Serilog;
using Serilog.Sinks.Database;
using static ASql.ASqlManager;

namespace LoggerService
{
    public class LoggerManager : ILoggerManager
    {
        private static ILogger logger;

        public LoggerManager(string dataBaseProvider, string dataBaseConnectionString)
        {
            logger = dataBaseProvider == "Oracle" ? new LoggerConfiguration()
              .WriteTo.Database(DBType.Oracle, dataBaseConnectionString, "SerLogs", Serilog.Events.LogEventLevel.Verbose, false, 1)
              .CreateLogger() : dataBaseProvider == "Postgres" ? new LoggerConfiguration()
              .WriteTo.Database(DBType.PostgreSQL, dataBaseConnectionString, "SerLogs", Serilog.Events.LogEventLevel.Verbose, false, 1)
              .CreateLogger() : null;
        }

        
        public void LogDebug(string message) => logger.Debug(message);

        public void LogError(string message) => logger.Error(message);

        public void LogInfo(string message) => logger.Information(message);

        public void LogWarn(string message) => logger.Warning(message);



        public void logDebugWithException(Exception exception, string message)
        {
            logger.Debug(exception, message);
        }

        public void logErrorWithException(Exception exception, string message)
        {
            logger.Error(exception, message);
        }

    }
}
