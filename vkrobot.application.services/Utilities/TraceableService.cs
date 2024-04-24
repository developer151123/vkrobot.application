using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace vkrobot.application.services.Utilities
{
    public class TraceableService
    {
        public void _trace_with_data(ILogger logger, string subject, object? data, [CallerMemberName] string memberName = "")
        {
            string content = JsonConvert.SerializeObject(data);
            string formatted = JsonHelper.FormatJson(content);
            logger.LogTrace("Method: {0} - subject: {1} - content: {2}", memberName, subject, formatted);
        }

        public void _trace(ILogger logger, string message, params object[] args)
        {
            logger.LogTrace(message, args);
        }

        public void _debug_with_data(ILogger logger, string subject, object? data, [CallerMemberName] string memberName = "")
        {
            string content = JsonConvert.SerializeObject(data);
            string formatted = JsonHelper.FormatJson(content);
            logger.LogDebug("Method: {0} - subject: {1} - content: {2}", memberName, subject, formatted);
        }

        public void _debug(ILogger logger, string message, params object[] args)
        {
            logger.LogDebug(message, args);
        }

        public void _info_with_data(ILogger logger, string subject, object? data, [CallerMemberName] string memberName = "")
        {
            string content = JsonConvert.SerializeObject(data);
            string formatted = JsonHelper.FormatJson(content);
            logger.LogInformation("Method: {0} - subject: {1} - content: {2}", memberName, subject, formatted);
        }

        public void _info(ILogger logger, string message, params object[] args)
        {
            logger.LogInformation(message, args);
        }

        public void _error_with_data(ILogger logger, string subject, object? data, [CallerMemberName] string memberName = "")
        {
            string content = JsonConvert.SerializeObject(data);
            string formatted = JsonHelper.FormatJson(content);
            logger.LogError("Method: {0} - subject: {1} - content: {2}", memberName, subject, formatted);
        }

        public void _error(ILogger logger, string message, params object[] args)
        {
            logger.LogError(message, args);
        }

        public void _warning_with_data(ILogger logger, string subject, object? data, [CallerMemberName] string memberName = "")
        {
            string content = JsonConvert.SerializeObject(data);
            string formatted = JsonHelper.FormatJson(content);
            logger.LogWarning("Method: {0} - subject: {1} - content: {2}", memberName, subject, formatted);
        }

        public void _warning(ILogger logger, string message, params object[] args)
        {
            logger.LogWarning(message, args);
        }
    }
}