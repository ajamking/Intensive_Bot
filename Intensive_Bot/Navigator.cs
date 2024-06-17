using Exceptions;
using Intensive_Bot.BLFunctions;
using Intensive_Bot.Entities;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Intensive_Bot;

public static class Navigator
{
    private static readonly List<BotUser> _botUsers = new();

    private static readonly Func<BotUser, bool>[] _handlers = new[]
    {
        HandleKeyboardWordMessage,
        HandleAnyUnknownMessage,
    };

    public static void Execute(ITelegramBotClient botClient, Message message)
    {
        TryAddBotUser(botClient, message);

        var activeBotUser = _botUsers.First(x => x.ChatId == message.Chat.Id);

        ResetBotUsersMessage(activeBotUser, message);

        BotLogger.LogRequest(activeBotUser);

        foreach (var handler in _handlers)
        {
            if (handler.Invoke(activeBotUser))
            {
                if (activeBotUser.Username == Program.BotEnvironment.AdminUsername)
                {
                    AnswerSender.ShowKeyboard(activeBotUser);
                }

                break;
            }
        }
    }

    private static bool HandleKeyboardWordMessage(BotUser botUser)
    {
        if (!AnswerSender.KeyboardWordsDic.ContainsValue(botUser.Message.Text))
        {
            return false;
        }

        try
        {
            switch (botUser.Message.Text)
            {
                case var text when text == AnswerSender.KeyboardWordsDic[KeyboardWords.ShowAllMR]:
                    {
                        AnswerSender.SendMessage(botUser, KeyboardFunctions.GetAllActiveMergeRequests());
                        break;
                    }
                case var text when text == AnswerSender.KeyboardWordsDic[KeyboardWords.ShowMyMR]:
                    {
                        AnswerSender.SendMessage(botUser, KeyboardFunctions.GetAllAttachedToMeMergeRequests());
                        break;
                    }
                case var text when text == AnswerSender.KeyboardWordsDic[KeyboardWords.CustomizeNotification]:
                    {
                        AnswerSender.SendMessage(botUser, "");
                        break;
                    }
                case var text when text == AnswerSender.KeyboardWordsDic[KeyboardWords.SwitchNotification]:
                    {
                        AnswerSender.SendMessage(botUser, KeyboardFunctions.SwitchNotifications(botUser));
                        break;
                    }
                case var text when text == AnswerSender.KeyboardWordsDic[KeyboardWords.AboutInfo]:
                    {

                        break;
                    }
            }
        }
        catch (BadApiResponseException ex)
        {
            BotLogger.LogException(ex, "", botUser);
        }
        catch (BadMarkdownSyntaxException ex)
        {
            BotLogger.LogException(ex, "Скорее всего исключение вызвано причудами кодировки телеграмма", botUser);
        }
        catch (Exception ex)
        {
            BotLogger.LogException(ex, "", botUser);
        }

        return true;
    }

    private static bool HandleAnyUnknownMessage(BotUser botUser)
    {
        botUser.BotClient.SendTextMessageAsync(botUser.Message.Chat.Id,
                          text: "Я на этом свете недавно и еще не знаю таких сложных команд 😢");

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