using System;
using System.Reflection;
using ForgeBot.Diagnostics;
using log4net;

namespace ForgeBot
{
    public class Logger
    {
        private static readonly ILog _logger = LogManager.GetLogger ("ivanJo FoE");

        public static void LogInfo(string message)
        {
            _logger.Info(message);
        }
        public static void LogInfo(string message, Exception ex)
        {
            string err = string.Format("Message: {0}. Exception: {1}",message ?? string.Empty,
                                        EventFormatter.FormatEvent(MethodBase.GetCurrentMethod(), ex));
            LogInfo(err);
        }
        public static void LogWarning(string message)
        {
            _logger.Warn(message);
        }
        public static void LogWarning(string message, Exception ex)
        {
            string err = string.Format("Message: {0}. Exception: {1}", message ?? string.Empty,
                                        EventFormatter.FormatEvent(MethodBase.GetCurrentMethod(), ex));
            LogWarning(err);
        }
        public static void LogError(string message, Exception ex)
        {
            string err = string.Format("Message: {0}. Exception: {1}", message ?? string.Empty,
                                        EventFormatter.FormatEvent(MethodBase.GetCurrentMethod(), ex));
            LogError(err);
        }
        public static void LogError(string message)
        {
            _logger.Error(message);
        }

        public static void LogDebug(string message)
        {
            _logger.Debug(message);
        }

    }
}
