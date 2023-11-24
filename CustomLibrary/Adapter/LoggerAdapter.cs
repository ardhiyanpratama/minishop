using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomLibrary.Adapter
{
    public class LoggerAdapter<T> : ILoggerAdapter<T> where T : class
    {
        private readonly ILogger<T> _logger;

        public LoggerAdapter(ILogger<T> logger)
        {
            _logger = logger;
        }

        public void LogCritical(string message)
        {
            if (_logger.IsEnabled(LogLevel.Critical))
            {
                _logger.LogCritical(message);
            }
        }

        public void LogCritical<T0>(string message, T0 args0)
        {
            if (_logger.IsEnabled(LogLevel.Critical))
            {
                _logger.LogCritical(message, args0);
            }
        }

        public void LogCritical<T0, T1>(string message, T0 args0, T1 args1)
        {
            if (_logger.IsEnabled(LogLevel.Critical))
            {
                _logger.LogCritical(message, args0, args1);
            }
        }

        public void LogCritical<T0, T1, T2>(string message, T0 args0, T1 args1, T2 args2)
        {
            if (_logger.IsEnabled(LogLevel.Critical))
            {
                _logger.LogCritical(message, args0, args1, args2);
            }
        }

        public void LogCritical<T0, T1, T2, T3>(string message, T0 args0, T1 args1, T2 args2, T3 args3)
        {
            if (_logger.IsEnabled(LogLevel.Critical))
            {
                _logger.LogCritical(message, args0, args1, args2, args3);
            }
        }

        public void LogDebug(string message)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug(message);
            }
        }

        public void LogDebug<T0>(string message, T0 args0)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug(message, args0);
            }
        }

        public void LogDebug<T0, T1>(string message, T0 args0, T1 args1)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug(message, args0, args1);
            }
        }

        public void LogDebug<T0, T1, T2>(string message, T0 args0, T1 args1, T2 args2)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug(message, args0, args1, args2);
            }
        }

        public void LogDebug<T0, T1, T2, T3>(string message, T0 args0, T1 args1, T2 args2, T3 args3)
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug(message, args0, args1, args2, args3);
            }
        }

        public void LogError(string message)
        {
            if (_logger.IsEnabled(LogLevel.Error))
            {
                _logger.LogDebug(message);
            }
        }

        public void LogError<T0>(string message, T0 args0)
        {
            if (_logger.IsEnabled(LogLevel.Error))
            {
                _logger.LogDebug(message, args0);
            }
        }

        public void LogError<T0, T1>(string message, T0 args0, T1 args1)
        {
            if (_logger.IsEnabled(LogLevel.Error))
            {
                _logger.LogDebug(message, args0, args1);
            }
        }

        public void LogError<T0, T1, T2>(string message, T0 args0, T1 args1, T2 args2)
        {
            if (_logger.IsEnabled(LogLevel.Error))
            {
                _logger.LogDebug(message, args0, args1, args2);
            }
        }

        public void LogError<T0, T1, T2, T3>(string message, T0 args0, T1 args1, T2 args2, T3 args3)
        {
            if (_logger.IsEnabled(LogLevel.Error))
            {
                _logger.LogDebug(message, args0, args1, args2, args3);
            }
        }

        public void LogInformation(string message)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation(message);
            }
        }

        public void LogInformation<T0>(string message, T0 args0)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation(message, args0);
            }
        }

        public void LogInformation<T0, T1>(string message, T0 args0, T1 args1)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation(message, args0, args1);
            }
        }

        public void LogInformation<T0, T1, T2>(string message, T0 args0, T1 args1, T2 args2)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation(message, args0, args1, args2);
            }
        }

        public void LogInformation<T0, T1, T2, T3>(string message, T0 args0, T1 args1, T2 args2, T3 args3)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation(message, args0, args1, args2, args3);
            }
        }

        public void LogWarning(string message)
        {
            if (_logger.IsEnabled(LogLevel.Warning))
            {
                _logger.LogWarning(message);
            }
        }

        public void LogWarning<T0>(string message, T0 args0)
        {
            if (_logger.IsEnabled(LogLevel.Warning))
            {
                _logger.LogWarning(message, args0);
            }
        }

        public void LogWarning<T0, T1>(string message, T0 args0, T1 args1)
        {
            if (_logger.IsEnabled(LogLevel.Warning))
            {
                _logger.LogWarning(message, args0, args1);
            }
        }

        public void LogWarning<T0, T1, T2>(string message, T0 args0, T1 args1, T2 args2)
        {
            if (_logger.IsEnabled(LogLevel.Warning))
            {
                _logger.LogWarning(message, args0, args1, args2);
            }
        }

        public void LogWarning<T0, T1, T2, T3>(string message, T0 args0, T1 args1, T2 args2, T3 args3)
        {
            if (_logger.IsEnabled(LogLevel.Warning))
            {
                _logger.LogWarning(message, args0, args1, args2, args3);
            }
        }
    }

    public static class LoggerAdapterExtension
    {

        public static void LogEventConsumer<T>(this ILoggerAdapter<T> logger, string eventName, object message) where T : class
        {
            logger.LogInformation("Consuming {0}: {1}", eventName, JsonConvert.SerializeObject(message));
        }
    }
}
