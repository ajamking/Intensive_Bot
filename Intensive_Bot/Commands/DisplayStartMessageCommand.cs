using Intensive_Bot.BLFunctions;
using Intensive_Bot.Entities;
using Intensive_Bot.EntitiesAndModels;

namespace Intensive_Bot.Commands;

internal sealed class DisplayStartMessageCommand : BaseCommand
{
    public string CommandType => "/start";

    public override Task Execute(BotUser botUser)
    {
        var answer = BeautyHelper.MakeItStyled($"Приветствую!\n\nЯ - ваш персональный бот. Моя основная задача - упрощение " +
                 $"мониторинга MergeRequest-ов на GitLab.\n" +
                 $"\nВы можете проверять обновления своих проектов вручную при помощи клавиатурных кнопок " +
                 $"<{AnswerSender.KeyboardWordsDic[KeyboardWords.ShowAllMR]}> или {AnswerSender.KeyboardWordsDic[KeyboardWords.ShowMyMR]}>, " +
                 $"а также настроить периодические оповещения при помощи кнопок " +
                 $"<{AnswerSender.KeyboardWordsDic[KeyboardWords.CustomizeNotification]}> и <{AnswerSender.KeyboardWordsDic[KeyboardWords.SwitchNotification]}>.\n" +
                 $"\nВ случае возникновения неполадок в работе бота - обратитесь в службу поддержки.\n" +
                 $"\np.s. Если вы не администратор бота, то меню вам недоступно и пользоваться функциями бота вы не сможете.", UiTextStyle.Default);

        return AnswerSender.SendMessage(botUser, answer);
    }
}