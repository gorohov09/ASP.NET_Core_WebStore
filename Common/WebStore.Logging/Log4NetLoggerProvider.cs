using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Xml;

namespace WebStore.Logging
{
    public class Log4NetLoggerProvider : ILoggerProvider
    {
        private readonly string _ConfigurationFile;
        private readonly ConcurrentDictionary<string, Log4NetLogger> _Loggers = new ConcurrentDictionary<string, Log4NetLogger>();

        public Log4NetLoggerProvider(string ConfigurationFile) => _ConfigurationFile = ConfigurationFile;

        public ILogger CreateLogger(string categoryName) =>
            _Loggers.GetOrAdd(categoryName, category =>
            {
                var xml = new XmlDocument();
                xml.Load(_ConfigurationFile);
                return new Log4NetLogger(category, xml["log4net"]);
            });

        public void Dispose() => _Loggers.Clear();
    }
}
