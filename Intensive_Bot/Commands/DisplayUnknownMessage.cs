using Intensive_Bot.Entities;

namespace Intensive_Bot.Commands;

internal sealed class DisplayUnknownMessage
{ 
    public Task Execute(BotUser botUser)
        => AnswerSender.SendMessage(botUser, "Я на этом свете недавно и еще не знаю таких сложных команд 😢");
}