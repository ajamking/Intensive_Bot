using Intensive_Bot.BLFunctions;
using Intensive_Bot.Entities;
using Intensive_Bot.EntitiesAndModels;

namespace Intensive_Bot.Commands;

internal sealed class DisplaySwitchMergeRequestsNotificationCommand : BaseCommand
{
    public string CommandType => "Вкл/Выкл оповещения";

    public override Task Execute(BotUser botUser)
    {
        var answer = BotFunctions.SwitchNotifications(botUser) ?
            BeautyHelper.MakeItStyled("Регулярные оповещения включены! ✅", UiTextStyle.Default) :
            BeautyHelper.MakeItStyled("Регулярные оповещения отключены! 🔴", UiTextStyle.Default);

        return AnswerSender.SendMessage(botUser, answer);
    }
}