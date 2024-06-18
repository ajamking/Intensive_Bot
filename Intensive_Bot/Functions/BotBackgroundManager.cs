using Intensive_Bot.Entities;
using Intensive_Bot.EntitiesAndModels;
using System.Net.NetworkInformation;
using System.Text;
using Telegram.Bot;

namespace Intensive_Bot.BLFunctions;

public static class BotBackgroundManager
{
    private static List<MergeRequestInfoUI> _previousAdminsMergeRequests = new List<MergeRequestInfoUI>();

    private static BotUser _admin;

    public static async Task StartAstync(ITelegramBotClient botClient)
    {
        RunAdminsBgTasks(botClient);
    }

    private static async Task RunAdminsBgTasks(ITelegramBotClient botClient)
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

            var _admin = Navigator.BotUsers.FirstOrDefault(x => x.Username == Program.BotEnvironment.AdminUsername);

            if (_admin != null && _admin.AlertsOn && _admin.AlertFrequencyMinutes != 0)
            {
                SendAlertToAdmin();
                BotLogger.LogSystemProcess("Рассылка для администратора отработала корректно.");

                await Task.Delay(TimeSpan.FromMinutes(_admin.AlertFrequencyMinutes));
            }
            else
            {
                await Task.Delay(TimeSpan.FromMinutes(1));
            }
        }
    }

    private static void SendAlertToAdmin()
    {
        if (CheckAdminsMergeRequestUpdates())
        {
            var answer = new StringBuilder(BeautyHelper.MakeItStyled($"Обнаружены новые MergeRequest-ы! Ознакомьтесь:\n", UiTextStyle.Header));

            answer.AppendLine(BeautyHelper.MakeItStyled($"Все ваши активные Merge Request-ы\n", UiTextStyle.Header));
            answer.AppendLine(BotFunctions.GetAllActiveMergeRequests().MakeMrResponseBeautier());

            answer.AppendLine(BeautyHelper.MakeItStyled($"Все закрепленные за вами активные Merge Request-ы\n", UiTextStyle.Header));
            answer.AppendLine(BotFunctions.GetAllAttachedToMeMergeRequests().MakeMrResponseBeautier());

            AnswerSender.SendMessage(_admin, answer.ToString());
        }
        else
        {
            AnswerSender.SendMessage(_admin, BeautyHelper.MakeItStyled($"Новых MergeRequest-ов не обнаружено.", UiTextStyle.Default));
        }
    }

    private static bool CheckAdminsMergeRequestUpdates()
    {
        var newMergeRequests = BotFunctions.GetAllActiveMergeRequests();

        newMergeRequests.AddRange(BotFunctions.GetAllAttachedToMeMergeRequests());

        if (_previousAdminsMergeRequests != newMergeRequests)
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