using Intensive_Bot.EntitiesAndModels;
using System.Net.NetworkInformation;
using System.Text;
using Telegram.Bot;

namespace Intensive_Bot.Functions;

public static class BotBackgroundManager
{
    private static List<MergeRequestInfoUI> _previousAdminsMergeRequests = new List<MergeRequestInfoUI>();

    public static async Task StartAsync(ITelegramBotClient botClient)
    {
        try
        {
            await RunAdminsTasks(botClient);
        }
        catch (Exception ex)
        {
            ex.LogException("Из-за ошибки фоновые задачи были остановлены.");
        }
    }

    private static async Task RunAdminsTasks(ITelegramBotClient botClient)
    {
        while (true)
        {
            while (true)
            {
                if (CheckInternetConnection())
                {
                    break;
                }

                await Task.Delay(TimeSpan.FromSeconds(5));
            }

            BotLogger.LogSystemProcess("Подключение к интернету присутствует.");

            var admin = Navigator.BotUsers.FirstOrDefault(x => x.Value.IsAdmin).Value;

            if (admin != null && admin.NotificationEnabled && admin.NotificationFrequencyMinutes != 0)
            {
                SendAlertToAdmin(admin);
                BotLogger.LogSystemProcess("Рассылка для администратора отработала корректно.");

                await Task.Delay(TimeSpan.FromMinutes(admin.NotificationFrequencyMinutes));
            }
            else
            {
                await Task.Delay(TimeSpan.FromMinutes(1));
            }
        }
    }

    private static void SendAlertToAdmin(BotUser admin)
    {
        if (CheckAdminsMergeRequestUpdates())
        {
            var answer = new StringBuilder(BeautyHelper.MakeItStyled($"Обнаружены новые MergeRequest-ы!\nОзнакомьтесь:\n\n", UiTextStyle.Header));

            answer.AppendLine(BeautyHelper.MakeItStyled($"Все ваши активные Merge Request-ы:\n", UiTextStyle.Default));
            answer.AppendLine(BotFunctions.GetAllActiveMergeRequests().MakeMrResponseBeautier());

            answer.AppendLine(BeautyHelper.MakeItStyled($"Все закрепленные за вами активные Merge Request-ы:\n", UiTextStyle.Default));
            answer.AppendLine(BotFunctions.GetAllAttachedToMeMergeRequests().MakeMrResponseBeautier());

            AnswerSender.SendMessage(admin, answer.ToString());
        }
        else
        {
            AnswerSender.SendMessage(admin, BeautyHelper.MakeItStyled($"Новых или обновленных MergeRequest-ов не обнаружено.", UiTextStyle.Default));
        }
    }

    private static bool CheckAdminsMergeRequestUpdates()
    {
        var newMergeRequests = BotFunctions.GetAllActiveMergeRequests();

        newMergeRequests.AddRange(BotFunctions.GetAllAttachedToMeMergeRequests());

        if (!_previousAdminsMergeRequests.SequenceEqual(newMergeRequests))
        {
            _previousAdminsMergeRequests = newMergeRequests;

            return true;
        }

        return false;
    }

    private static bool CheckInternetConnection()
    {
        var myPing = new Ping();

        PingReply reply = myPing.Send("google.com", 10, new byte[32], new PingOptions());

        if (reply.Status == IPStatus.Success)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}