using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomLibrary.Adapter
{
    public interface ILoggerAdapter<T> where T : class
    {
        void LogInformation(string message);

        void LogInformation<T0>(string message, T0 args0);

        void LogInformation<T0, T1>(string message, T0 args0, T1 args1);

        void LogInformation<T0, T1, T2>(string message, T0 args0, T1 args1, T2 args2);

        void LogInformation<T0, T1, T2, T3>(string message, T0 args0, T1 args1, T2 args2, T3 args3);

        void LogDebug(string message);

        void LogDebug<T0>(string message, T0 args0);

        void LogDebug<T0, T1>(string message, T0 args0, T1 args1);

        void LogDebug<T0, T1, T2>(string message, T0 args0, T1 args1, T2 args2);

        void LogDebug<T0, T1, T2, T3>(string message, T0 args0, T1 args1, T2 args2, T3 args3);

        void LogWarning(string message);

        void LogWarning<T0>(string message, T0 args0);

        void LogWarning<T0, T1>(string message, T0 args0, T1 args1);

        void LogWarning<T0, T1, T2>(string message, T0 args0, T1 args1, T2 args2);

        void LogWarning<T0, T1, T2, T3>(string message, T0 args0, T1 args1, T2 args2, T3 args3);

        void LogError(string message);

        void LogError<T0>(string message, T0 args0);

        void LogError<T0, T1>(string message, T0 args0, T1 args1);

        void LogError<T0, T1, T2>(string message, T0 args0, T1 args1, T2 args2);

        void LogError<T0, T1, T2, T3>(string message, T0 args0, T1 args1, T2 args2, T3 args3);

        void LogCritical(string message);

        void LogCritical<T0>(string message, T0 args0);

        void LogCritical<T0, T1>(string message, T0 args0, T1 args1);

        void LogCritical<T0, T1, T2>(string message, T0 args0, T1 args1, T2 args2);

        void LogCritical<T0, T1, T2, T3>(string message, T0 args0, T1 args1, T2 args2, T3 args3);
    }
}
