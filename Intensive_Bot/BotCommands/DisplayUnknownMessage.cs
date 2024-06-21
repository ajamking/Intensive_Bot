using Intensive_Bot.EntitiesAndModels;

namespace Intensive_Bot.BotCommands;

internal static class DisplayUnknownMessage
{
    public static Task Execute(BotUser botUser)
        => AnswerSender.SendMessage(botUser, "Я на этом свете недавно и еще не знаю таких сложных команд 😢");
}