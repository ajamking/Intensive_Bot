using System.Reflection;

namespace Intensive_Bot.Commands;

internal static class AllCommandsConstructor
{
    public static List<BaseCommand> AllCommands { get; set; }

    static AllCommandsConstructor()
    {
        AllCommands = new List<BaseCommand>();

        var childrenTypes = Assembly.GetExecutingAssembly().GetTypes().Where(i => i.IsSubclassOf(typeof(BaseCommand)));

        foreach (var item in childrenTypes)
        {
            var instance = (BaseCommand)Activator.CreateInstance(item);

            AllCommands.Add(instance);
        }
    }
}