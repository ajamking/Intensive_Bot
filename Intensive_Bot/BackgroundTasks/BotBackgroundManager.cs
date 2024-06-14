using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Intensive_Bot.BackgroundTasks;

public static class BotBackgroundManager
{
    public static async Task StartAstync(ITelegramBotClient botClient)
    {

    }

    private static bool CheckInternetConnection()
    {
        var myPing = new Ping();

        PingReply reply = myPing.Send("google.com", 10, new byte[32], new PingOptions());

        if (reply.Status == IPStatus.Success)
        {
            Console.WriteLine($"<{DateTime.Now:HH:mm:ss}> Подключение к Internet стабильно");

            return true;
        }
        else
        {
            Console.WriteLine($"<{DateTime.Now:HH:mm:ss}> Нет подключения к Internet");

            return false;
        }
    }

    private static async Task<bool> SendLogFileToAdmin()
    {
       // await Program.BotClient.SendTextMessageAsync(Program.AdminsChatId, "");

        return false;
    }



}