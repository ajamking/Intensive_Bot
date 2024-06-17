using System.Globalization;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Newtonsoft.Json;
using Intensive_Bot.Entities;
using Microsoft.Extensions.Hosting;
using Intensive_Bot.API;
using Intensive_Bot.BLFunctions;
using Intensive_Bot.EntitiesAndModels;
using Exceptions;

namespace Intensive_Bot;

class Program
{
    public static BotEnvironment BotEnvironment { get; private set; }

    public static TelegramBotClient BotClient { get; private set; }

    static void Main(string[] args)
    {
        try
        {
            BotEnvironment = JsonConvert.DeserializeObject<BotEnvironment>(System.IO.File.ReadAllText("./EnvironmentFiles/Environment.json"));

            BotClient = new(token: BotEnvironment.BotToken);

            BotBackgroundManager.StartAstync(BotClient);

            BotClient.StartReceiving(HandleUpdateAsync, HandleError);

            Console.OutputEncoding = Encoding.UTF8;
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
            Console.WriteLine("Bot started");

            var result = ApiRequestBuilder.CallApi(ApiRequestType.AssignedToMeMergeRequests).Result;

            var res = JsonConvert.DeserializeObject<List<MergeRequestInfoUI>>(result);

            var date = DateTime.Parse(res.FirstOrDefault().UpdatedAt);

            var host = new HostBuilder()
             .ConfigureHostConfiguration(h => { })
             .UseConsoleLifetime()
             .Build();
            host.Run();
        }
        catch (Exception ex)
        {
            BotLogger.LogException(ex);

            throw new UnknownException(ex.Message, ex);
        }
    }

    async static Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        try
        {
            if (update.Type != UpdateType.Message || update?.Message?.Text == null)
                return;

            if (update.Message.Chat.Type is ChatType.Channel or ChatType.Group or ChatType.Supergroup)
                return;

            if (string.IsNullOrEmpty(update.Message.Chat.Username) || update.Message.Chat.Username != BotEnvironment.AdminUsername)
            {
                await botClient.SendTextMessageAsync(update.Message.Chat.Id,
                          text: "Бот предназначен для частного использования, неавторизованные пользователи не имеют доступа к его функционалу.");
                return;
            }

            await Task.Run(() => Navigator.Execute(botClient, update.Message));

            return;

        }
        catch (Exception ex)
        {
            ex.LogException($"Исключение проигнорировано");
        }
    }

    static Task HandleError(ITelegramBotClient botClient, Exception ex, CancellationToken cancellationToken)
    {
        var errorMessage = ex switch
        {
            ApiRequestException apiRequestException =>
            $"Ошбика телеграм API:\n{apiRequestException.ErrorCode}\n{apiRequestException.Message}",
            _ => ex.ToString()
        };

        ex.LogException($"Исключение проигнорировано: {errorMessage}");

        return Task.CompletedTask;
    }

}