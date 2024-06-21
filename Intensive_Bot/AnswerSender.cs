using Exceptions;
using Intensive_Bot.BotCommands.Commands;
using Intensive_Bot.EntitiesAndModels;
using Intensive_Bot.Functions;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Intensive_Bot;

public static class AnswerSender
{
    private static ReplyKeyboardMarkup _keyboard;

    private static int _maxMessageLength = 4000;

    static AnswerSender()
    {
        _keyboard = new(new[]
        {
        new KeyboardButton[] { CommandsManager.NamedCommandsDic[CommandType.ShowAllMR], CommandsManager.NamedCommandsDic[CommandType.ShowMyMR], },
        new KeyboardButton[] { CommandsManager.NamedCommandsDic[CommandType.CustomizeNotification], CommandsManager.NamedCommandsDic[CommandType.SwitchNotification] },
        new KeyboardButton[] { CommandsManager.NamedCommandsDic[CommandType.AboutInfo], },
        })
        { ResizeKeyboard = true };
    }

    public async static Task SendMessage(BotUser botUser, string message, bool isCorrect = true)
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

    public async static Task<bool> ShowKeyboard(BotUser botUser)
    {
        try
        {
            await botUser.BotClient.SendTextMessageAsync(botUser.Message.Chat.Id,
                  text: "Выберите интересующий пункт из меню или пришлите известную команду...",
                  replyMarkup: _keyboard);

            return true;
        }
        catch
        {
            return false;
        }
    }

    private static bool CheckMessageHaveValidLength(string message) => message.Length > _maxMessageLength ? false : true;
}