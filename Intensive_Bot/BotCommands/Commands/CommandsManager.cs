using System.Reflection;

namespace Intensive_Bot.BotCommands.Commands;

internal static class CommandsManager
{
    public static List<NamedCommand> NamedCommands { get; private set; }
    public static Dictionary<CommandType, string> NamedCommandsDic { get; private set; }
    public static List<DynamicCommand> DynamicCommands { get; private set; }

    static CommandsManager()
    {
        NamedCommands = new List<NamedCommand>();

        var named = Assembly.GetExecutingAssembly().GetTypes().Where(i => i.IsSubclassOf(typeof(NamedCommand)));

        foreach (var item in named)
        {
            var instance = (NamedCommand)Activator.CreateInstance(item);

            NamedCommands.Add(instance);
        }

        NamedCommandsDic = new Dictionary<CommandType, string> { };

        foreach (var command in NamedCommands)
        {
            NamedCommandsDic[command.CommandType] = command.CommandName;
        }

        DynamicCommands = new List<DynamicCommand>();

        var dynamic = Assembly.GetExecutingAssembly().GetTypes().Where(i => i.IsSubclassOf(typeof(DynamicCommand)));

        foreach (var item in dynamic)
        {
            var instance = (DynamicCommand)Activator.CreateInstance(item);

            DynamicCommands.Add(instance);
        }
    }
}