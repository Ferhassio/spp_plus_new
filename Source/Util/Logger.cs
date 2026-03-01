using BepInEx.Logging;
using System;

namespace SkillsPlusPlus
{

    /// <summary>
    /// Helper class for logging details to the runtime's output.
    /// It the logging behaviour is internally available however the log
    /// level can be configured if you need visibility of the Skills++ mod.
    /// </summary>
    public class Logger
    {

        private static readonly string LOG_TAG = "Skills++";
        private static readonly ManualLogSource manualLogSource = BepInEx.Logging.Logger.CreateLogSource(LOG_TAG);

        /// <summary>
        /// The different log levels availble for filtering
        /// </summary>
        public enum LogLevel
        {
            /// <summary>
            /// Log nothing
            /// </summary>
            None = 0,
            /// <summary>
            /// Log only errors
            /// </summary>
            Error = 1,
            /// <summary>
            /// Log warnings and errors
            /// </summary>
            Warning = 2,
            /// <summary>
            /// Log debugs, warnings, and errors
            /// </summary>
            Debug = 3
        }

        /// <summary>
        /// The current log level for the logger. Any changes are effective immediately
        /// <remark>
        /// This field is set to to <see cref="LogLevel.Warning"/> by default
        /// </remark>
        /// </summary>
        public static LogLevel LOG_LEVEL = LogLevel.Warning;

        internal static void Debug(string message, params object[] formatArgs)
        {
            if (LOG_LEVEL >= LogLevel.Debug)
            {
                var formattedMessage = "null";
                if (message != null)
                {
                    formattedMessage = String.Format(message, formatArgs);
                }
                manualLogSource.LogInfo(formattedMessage);
            }
        }

        internal static void Debug(object message)
        {
            if (LOG_LEVEL >= LogLevel.Debug)
            {
                if (message == null)
                {
                    message = "null";
                }
                manualLogSource.LogInfo(message);
            }
        }

        internal static void Warn(string message, params object[] formatArgs)
        {
            if (LOG_LEVEL >= LogLevel.Warning)
            {
                var formattedMessage = "null";
                if (message != null)
                {
                    formattedMessage = String.Format(message, formatArgs);
                }
                manualLogSource.LogWarning(formattedMessage);
            }
        }
        internal static void Warn(object message)
        {
            if (LOG_LEVEL >= LogLevel.Warning)
            {
                if (message == null)
                {
                    message = "null";
                }
                manualLogSource.LogWarning(message);
            }
        }

        internal static void Error(string message, params object[] formatArgs)
        {
            if (LOG_LEVEL >= LogLevel.Error)
            {
                var formattedMessage = "null";
                if (message != null)
                {
                    formattedMessage = String.Format(message, formatArgs);
                }
                manualLogSource.LogError(formattedMessage);
            }
        }

        internal static void Error(Exception exception)
        {
            if (LOG_LEVEL >= LogLevel.Error)
            {
                manualLogSource.LogError(exception);
            }
        }

    }
}
