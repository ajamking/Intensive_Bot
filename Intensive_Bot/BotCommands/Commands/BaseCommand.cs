using Intensive_Bot.EntitiesAndModels;

namespace Intensive_Bot.BotCommands.Commands;

public abstract class BaseCommand
{
    public abstract Task Execute(BotUser botUser);
}
