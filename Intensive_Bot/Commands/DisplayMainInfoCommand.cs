using Intensive_Bot.BLFunctions;
using Intensive_Bot.Entities;
using Intensive_Bot.EntitiesAndModels;

namespace Intensive_Bot.Commands;

internal sealed class DisplayMainInfoCommand : BaseCommand
{
    public string CommandType => "Настроить оповещения";

    public override Task Execute(BotUser botUser)
    {
        var answer = BeautyHelper.MakeItStyled($"Приветствую!\n\nЯ - ваш персональный бот. Моя основная задача - упрощение " +
                             $"мониторинга MergeRequest-ов на GitLab.\n" +
                             $"\nВы можете проверять обновления своих проектов вручную при помощи клавиатурных кнопок " +
                             $"<{AnswerSender.KeyboardWordsDic[KeyboardWords.ShowAllMR]}> или <{AnswerSender.KeyboardWordsDic[KeyboardWords.ShowMyMR]}>, " +
                             $"а также настроить периодические оповещения при помощи кнопок " +
                             $"<{AnswerSender.KeyboardWordsDic[KeyboardWords.CustomizeNotification]}> и <{AnswerSender.KeyboardWordsDic[KeyboardWords.SwitchNotification]}>.\n" +
                             $"По умолчанию оповещения отключены, а все настройки действуют лишь в рамках одной рабочей сессии.\n" +
                             $"\nВ случае возникновения неполадок в работе бота - обратитесь в службу поддержки.\n", UiTextStyle.Default);

        return AnswerSender.SendMessage(botUser, answer);
    }
}