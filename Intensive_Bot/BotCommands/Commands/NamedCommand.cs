namespace Intensive_Bot.BotCommands.Commands;

public abstract class NamedCommand : BaseCommand
{
    public abstract CommandType CommandType { get; }
    public abstract string CommandName { get; }
}
