using Intensive_Bot.Entities;
using Serilog;
using Serilog.Events;
using System.Text;

namespace Intensive_Bot;

public static class BotLogger
{
    private static ILogger _consoleLogger;

    static BotLogger()
    {
        _consoleLogger = new LoggerConfiguration()
           .WriteTo.Console()
           .CreateLogger();
    }

    public static void LogUserMessage(BotUser botUser)
    {
        var logString = $"Получено сообщение от пользователя {botUser.Username} ({botUser.FirstName}) [{botUser.ChatId}]\n" +
            $"Текст сообщения: {botUser.Message.Text}";

        _consoleLogger.Information(logString);
        WriteLogToFile(logString, LogEventLevel.Information);
    }

    public static void LogResponse(BotUser botUser, bool isCorrect = true)
    {
        var status = isCorrect ? "Завершен корректно" : "Завершен с ошибкой";

        var logString = $"Отправлен ответ пользователю {botUser.Username} ({botUser.FirstName}) [{botUser.ChatId}]\n" +
            $"Статус: {status}";

        _consoleLogger.Information(logString);
        WriteLogToFile(logString, LogEventLevel.Information);
    }

    public static void LogException(this Exception ex, string extraMessage = "", BotUser botUser = null)
    {
        var logString = new StringBuilder();

        if (botUser == null)
        {
            logString.Append("Произошла неожиданная системная ошибка: ");
        }
        else
        {
            logString.Append($"Сообщение {botUser.Message.Text}, " +
                $"полученное от {botUser.Username} ({botUser.FirstName}) [{botUser.ChatId}]," +
                $"\n вызвало ошибку: ");
        }

        logString.AppendLine(ex?.Message);

        if (!string.IsNullOrEmpty(extraMessage))
        {
            logString.AppendLine($"Сопроводительное сообщение: {extraMessage}");
        }

        logString.AppendLine($"Стектрейс: {ex?.StackTrace}");

        _consoleLogger.Error(logString.ToString());
        WriteLogToFile(logString.ToString(), LogEventLevel.Error);
    }

    public static void LogSystemProcess(string extraMessage)
    {
        var logString = $"Лог системного процесса: {extraMessage}";

        _consoleLogger.Information(logString);
        WriteLogToFile(logString, LogEventLevel.Information);
    }

    private static void WriteLogToFile(string str, LogEventLevel logEventLevel)
    {
        Log.Logger = new LoggerConfiguration()
         .WriteTo.Logger(l => l
             .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information)
             .WriteTo.File(path: Program.BotEnvironment.LogFilePath))
         .WriteTo.Logger(l => l
             .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error)
             .WriteTo.File(path: Program.BotEnvironment.ExceptionLogFilePath))
         .CreateLogger();

        switch (logEventLevel)
        {
            case LogEventLevel.Error:
                Log.Logger.Error(str);
                break;

            default:
                Log.Logger.Information(str);
                break;
        }

        Log.CloseAndFlush();
    }
}