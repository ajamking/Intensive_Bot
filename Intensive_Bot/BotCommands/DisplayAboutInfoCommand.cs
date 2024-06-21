using Intensive_Bot.BotCommands.Commands;
using Intensive_Bot.EntitiesAndModels;
using Intensive_Bot.Functions;

namespace Intensive_Bot.BotCommands;

internal sealed class DisplayAboutInfoCommand : NamedCommand
{
    public override string CommandName => "ℹ️ Справка";

    public override CommandType CommandType => CommandType.AboutInfo;

    public override Task Execute(BotUser botUser)
    {
        var answer = BeautyHelper.MakeItStyled($"Приветствую!\n\nЯ - ваш персональный бот. Моя основная задача - упрощение " +
                             $"мониторинга MergeRequest-ов на GitLab.\n" +
                             $"\nВы можете проверять обновления своих проектов вручную при помощи клавиатурных кнопок " +
                             $"<{CommandsManager.NamedCommandsDic[CommandType.ShowAllMR]} > или < {CommandsManager.NamedCommandsDic[CommandType.ShowMyMR]}>, " +
                             $"а также настроить периодические оповещения при помощи кнопок " +
                             $"<{CommandsManager.NamedCommandsDic[CommandType.CustomizeNotification]} > и < {CommandsManager.NamedCommandsDic[CommandType.SwitchNotification]}>.\n" +
                             $"По умолчанию оповещения отключены, а все настройки действуют лишь в рамках одной рабочей сессии.\n" +
                             $"\nВ случае возникновения неполадок в работе бота - обратитесь в службу поддержки.\n", UiTextStyle.Default);

        return AnswerSender.SendMessage(botUser, answer);
    }
}