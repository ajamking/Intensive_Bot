using Intensive_Bot.BotCommands.Commands;
using Intensive_Bot.EntitiesAndModels;
using Intensive_Bot.Functions;

namespace Intensive_Bot.BotCommands;

internal sealed class ShowNotificationRulesCommand : NamedCommand
{
    public override string CommandName => "Настроить оповещения";

    public override CommandType CommandType => CommandType.CustomizeNotification;

    public override Task Execute(BotUser botUser)
    {
        var answer = BeautyHelper.MakeItStyled($"Для установки нового интервала оповещений пришлите мне сообщение формата:\n" +
            $"H:3, где H - hours (часы), 3 - количество часов, или формата:\n" +
            $"m:600, где m - minutes (минуты), 600 - количество минут.", UiTextStyle.Default);

        return AnswerSender.SendMessage(botUser, answer);
    }
}