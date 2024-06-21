using Intensive_Bot.BotCommands.Commands;
using Intensive_Bot.EntitiesAndModels;
using Intensive_Bot.Functions;

namespace Intensive_Bot.BotCommands;

internal sealed class DisplayStartMessageCommand : NamedCommand
{
    public override string CommandName => "/start";

    public override CommandType CommandType => CommandType.StartFunc;


    public override Task Execute(BotUser botUser)
    {
        var namedCommands = new Dictionary<CommandType, string>();

        foreach (var command in CommandsManager.NamedCommands)
        {
            namedCommands[command.CommandType] = command.CommandName;
        }

        var answer = BeautyHelper.MakeItStyled($"Приветствую!\n\nЯ - ваш персональный бот. Моя основная задача - упрощение " +
                 $"мониторинга MergeRequest-ов на GitLab.\n" +
                 $"\nВы можете проверять обновления своих проектов вручную при помощи клавиатурных кнопок " +
                 $"<{CommandsManager.NamedCommandsDic[CommandType.ShowAllMR]}> или {CommandsManager.NamedCommandsDic[CommandType.ShowMyMR]}>, " +
                 $"а также настроить периодические оповещения при помощи кнопок " +
                 $"<{CommandsManager.NamedCommandsDic[CommandType.CustomizeNotification]}> и <{CommandsManager.NamedCommandsDic[CommandType.SwitchNotification]}>.\n" +
                 $"\nВ случае возникновения неполадок в работе бота - обратитесь в службу поддержки.\n" +
                 $"\np.s. Если вы не администратор бота, то меню вам недоступно и пользоваться функциями бота вы не сможете.", UiTextStyle.Default);

        return AnswerSender.SendMessage(botUser, answer);
    }
}