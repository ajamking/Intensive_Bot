using Intensive_Bot.Entities;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Intensive_Bot;

public static class Navigation
{
    private static readonly List<BotUser> _botUsers = new();

    private static readonly Func<BotUser, bool>[] _handlers = new[]
    {
        HoldAnyMessageAction,

    };

    public static void Execute(ITelegramBotClient botClient, Message message)
    {
        TryAddBotUser(botClient, message);

        var activeBotUser = _botUsers.First(x => x.ChatId == message.Chat.Id);

        ResetBotUsersMessage(activeBotUser, message);

        foreach (var handler in _handlers)
        {
            if (handler.Invoke(activeBotUser))
            {
                break;
            }
        }
    }

    private static bool HoldAnyMessageAction(BotUser botUser)
    {
        botUser.BotClient.SendTextMessageAsync(botUser.Message.Chat.Id,
                          text: botUser.Message.Text);

        return true;
    }

    private static void TryAddBotUser(ITelegramBotClient telegramBotClient, Message message)
    {
        if (!_botUsers.Any(x => x.ChatId == message.Chat.Id))
        {
            _botUsers.Add(new BotUser(telegramBotClient, message));
        }
    }
    private static void ResetBotUsersMessage(BotUser activeBotUser, Message message)
    {
        activeBotUser.Message = message;
    }
}