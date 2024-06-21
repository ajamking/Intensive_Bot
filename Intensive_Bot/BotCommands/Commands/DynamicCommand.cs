namespace Intensive_Bot.BotCommands.Commands;

public abstract class DynamicCommand : BaseCommand
{
    public abstract bool CanExecute(string command);
}