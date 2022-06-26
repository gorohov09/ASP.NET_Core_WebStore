using log4net;
using log4net.Repository.Hierarchy;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Xml;

namespace WebStore.Logging
{
    public class Log4NetLogger : ILogger
    {
        private readonly ILog _Log;
        public Log4NetLogger(string Category, XmlElement Configuration)
        {
            var logger_repository = LogManager
                .CreateRepository(
                Assembly.GetEntryAssembly(),
                typeof(Hierarchy));

            _Log = LogManager.GetLogger(logger_repository.Name, Category);

            log4net.Config.XmlConfigurator.Configure(Configuration);
        }

        public IDisposable BeginScope<TState>(TState state) => null!;

        public bool IsEnabled(LogLevel logLevel) => logLevel switch
        {
            LogLevel.Trace => _Log.IsDebugEnabled,
            LogLevel.Debug => _Log.IsDebugEnabled,
            LogLevel.Information => _Log.IsInfoEnabled,
            LogLevel.Warning => _Log.IsWarnEnabled,
            LogLevel.Error => _Log.IsErrorEnabled,
            LogLevel.Critical => _Log.IsFatalEnabled,
            LogLevel.None => false,
            _ => throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null)
        };

        public void Log<TState>(LogLevel logLevel, 
            EventId eventId, TState state, Exception? exception, 
            Func<TState, Exception?, string> formatter)
        {
            if (formatter is null) throw new ArgumentNullException(nameof(formatter));

            if (!IsEnabled(logLevel))
                return;

            var log_string = formatter(state, exception);
            if (string.IsNullOrWhiteSpace(log_string) && exception is null)
                return;

            switch (logLevel)
            {
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);

                case LogLevel.None:
                    break;

                case LogLevel.Trace:
                case LogLevel.Debug:
                    _Log.Debug(log_string);
                    break;

                case LogLevel.Information:
                    _Log.Info(log_string);
                    break;

                case LogLevel.Warning:
                    _Log.Warn(log_string);
                    break;

                case LogLevel.Error:
                    _Log.Error(log_string, exception);
                    break;
                case LogLevel.Critical:
                    _Log.Error(log_string, exception);
                    break;
            }
        }
    }
}
