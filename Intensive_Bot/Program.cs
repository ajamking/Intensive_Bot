using System.Globalization;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Intensive_Bot.BackgroundTasks;
using Newtonsoft.Json;
using Intensive_Bot.Entities;
using Microsoft.Extensions.Hosting;

namespace Intensive_Bot;

class Program
{
    public static readonly BotEnvironment BotEnvironment = JsonConvert.DeserializeObject<BotEnvironment>(System.IO.File.ReadAllText("Environment.json"));

    public static TelegramBotClient BotClient { get; } = new(token: BotEnvironment.BotToken);

    static void Main(string[] args)
    {
        BotBackgroundManager.StartAstync(BotClient);
        BotClient.StartReceiving(HandleUpdateAsync, HandleError);

        Console.OutputEncoding = Encoding.UTF8;
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
        Console.WriteLine("Bot started");

        var host = new HostBuilder()
         .ConfigureHostConfiguration(h => { })
         .UseConsoleLifetime()
         .Build();
        host.Run();
    }

    async static Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        try
        {
            if (update.Type == UpdateType.Message && update?.Message?.Text != null)
            {
                if (false)
                {
                    return;
                }

                if (update.Message.Chat.Type is ChatType.Channel or ChatType.Group or ChatType.Supergroup)
                {
                    return;
                }

                Console.Write($"{DateTime.Now}: Принято сообщение: \"{update.Message.Text}\" от ");

                Console.ForegroundColor = ConsoleColor.Magenta;

                Console.WriteLine($"@{update.Message.Chat.Username}");

                Console.ResetColor();

                await Task.Run(() => Navigation.Execute(botClient, update.Message));

                return;
            }
        }
        catch (Exception e)
        {
            e.LogException(update.Message.Chat.Username, update.Message.Chat.Id, update.Message.Text, "Проигнорировали исключение в HandleUpdateAsync");

            Console.WriteLine("Проигнорировали исключение " + e.Message + "в чате " + update.Message);
        }
    }

    static Task HandleError(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Прилетело исключение {exception.Message}");

        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException =>
            $"Ошбика телеграм API:\n{apiRequestException.ErrorCode}\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(errorMessage);

        return Task.CompletedTask;
    }

}