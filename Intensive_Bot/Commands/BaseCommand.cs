using Intensive_Bot.Entities;

namespace Intensive_Bot.Commands;

internal abstract class BaseCommand
{
    public string CommandType { get; }

    public virtual bool CanProcessCommand(string command)
        => CommandType == command;

    public abstract Task Execute(BotUser botUser);
}