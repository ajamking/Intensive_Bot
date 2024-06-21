using Intensive_Bot.BotCommands.Commands;
using Intensive_Bot.EntitiesAndModels;
using Intensive_Bot.Functions;

namespace Intensive_Bot.BotCommands;

internal sealed class SwitchMergeRequestsNotificationCommand : NamedCommand
{
    public override string CommandName => "Вкл/Выкл оповещения";

    public override CommandType CommandType => CommandType.SwitchNotification;

    public override Task Execute(BotUser botUser)
    {
        var answer = BotFunctions.SwitchNotifications(botUser) ?
            BeautyHelper.MakeItStyled("Регулярные оповещения включены! ✅", UiTextStyle.Default) :
            BeautyHelper.MakeItStyled("Регулярные оповещения отключены! 🔴", UiTextStyle.Default);

        return AnswerSender.SendMessage(botUser, answer);
    }
}