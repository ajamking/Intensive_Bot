using Intensive_Bot.BotCommands;
using Intensive_Bot.BotCommands.Commands;
using Intensive_Bot.EntitiesAndModels;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Intensive_Bot;

public static class Navigator
{
    public static readonly Dictionary<long, BotUser> BotUsers = new();

    public static void Execute(ITelegramBotClient botClient, Message message)
    {
        var currentBotUser = CheckInUser(botClient, message);
        ResetUserMessage(currentBotUser, message);
        BotLogger.LogUserMessage(currentBotUser);

        var answered = false;

        foreach (var command in CommandsManager.NamedCommands)
        {
            if (command.CommandName == currentBotUser.Message.Text)
            {
                command.Execute(currentBotUser);

                answered = true;

                break;
            }
        }

        foreach (var command in CommandsManager.DynamicCommands)
        {
            if (command.CanExecute(currentBotUser.Message.Text))
            {
                command.Execute(currentBotUser);

                answered = true;

                break;
            }
        }

        if (answered == false)
        {
            DisplayUnknownMessage.Execute(currentBotUser);
        }

        if (currentBotUser.Username == Program.BotEnvironment.AdminUsername)
        {
            AnswerSender.ShowKeyboard(currentBotUser);
        }
    }

    private static BotUser CheckInUser(ITelegramBotClient telegramBotClient, Message message)
    {
        if (BotUsers.TryGetValue(message.Chat.Id, out var user))
        {
            return user;
        }

        var newUser = new BotUser(telegramBotClient, message);

        BotUsers.Add(message.Chat.Id, newUser);

        return newUser;
    }

    private static void ResetUserMessage(BotUser currentBotUser, Message message)
    {
        currentBotUser.Message = message;
    }
}