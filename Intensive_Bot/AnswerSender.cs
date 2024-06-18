using Exceptions;
using Intensive_Bot.BLFunctions;
using Intensive_Bot.Entities;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Intensive_Bot;

public static class AnswerSender
{
    private static ReplyKeyboardMarkup _keyboard;

    private static int _maxMessageLength = 4000;

    public static Dictionary<KeyboardWords, string> KeyboardWordsDic { get; private set; } = new()
    {
        { KeyboardWords.ShowAllMR, "Показать все MR" },
        { KeyboardWords.ShowMyMR, "Показать мои MR" },
        { KeyboardWords.CustomizeNotification, "Настроить напоминание" },
        { KeyboardWords.SwitchNotification, "Выключить напоминание" },
        { KeyboardWords.AboutInfo, "ℹ️ Справка" },
    };

    static AnswerSender()
    {
        _keyboard = new(new[]
        {
        new KeyboardButton[] { KeyboardWordsDic[KeyboardWords.ShowAllMR], KeyboardWordsDic[KeyboardWords.ShowMyMR], },
        new KeyboardButton[] { KeyboardWordsDic[KeyboardWords.CustomizeNotification], KeyboardWordsDic[KeyboardWords.SwitchNotification], },
        new KeyboardButton[] { KeyboardWordsDic[KeyboardWords.AboutInfo], },
        })
        { ResizeKeyboard = true };
    }

    public async static void SendMessage(BotUser botUser, string message, bool isCorrect = true)
    {
        try
        {
            if (CheckMessageHaveValidLength(message))
            {
                await botUser.BotClient.SendTextMessageAsync(botUser.Message.Chat.Id,
                      text: message.Replace(BeautyHelper.DividerToken, ""),
                      parseMode: ParseMode.MarkdownV2);
            }
            else
            {
                var subMessages = message.Split(BeautyHelper.DividerToken);

                foreach (var subMessage in subMessages)
                {
                    await botUser.BotClient.SendTextMessageAsync(botUser.Message.Chat.Id,
                           text: subMessage,
                           parseMode: ParseMode.MarkdownV2);
                }
            }

            BotLogger.LogResponse(botUser, isCorrect);
        }
        catch (Exception ex)
        {
            throw new BadMarkdownSyntaxException(ex.Message, ex);
        }
    }

    public async static void ShowKeyboard(BotUser botUser)
    {
        try
        {
            await botUser.BotClient.SendTextMessageAsync(botUser.Message.Chat.Id,
                  text: "Выберите интересующий пункт из меню",
                  replyMarkup: _keyboard);
        }
        catch
        {
            return;
        }
    }

    private static bool CheckMessageHaveValidLength(string message) => message.Length > _maxMessageLength ? false : true;
}

public enum KeyboardWords
{
    ShowAllMR,
    ShowMyMR,
    CustomizeNotification,
    SwitchNotification,
    AboutInfo
}