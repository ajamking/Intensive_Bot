using Serilog;

namespace Intensive_Bot;

public static class ExceptionLogger
{
    static ExceptionLogger()
    {
        Log.Logger = new LoggerConfiguration()
         .WriteTo.File(Program.BotEnvironment.ExceptionLogFilePath)
         .CreateLogger();
    }

    public static void LogException(this Exception ex, string userName, long chatId, string userMessage, string dopMessage = "")
    {
        Log.Error($"\n{new string('-', 36)}\n" +
            $"На сообщение {userMessage} от {userName} [{chatId}]\n" +
            $"Произошла ошибка: {ex.Message}\n" +
            $"Дополнительное сообщение: {dopMessage}" +
            $"\nСтактрейс: {ex?.StackTrace}\n");

        Log.CloseAndFlush();
    }
}