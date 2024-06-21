using Intensive_Bot.BLFunctions;
using Intensive_Bot.Entities;
using Intensive_Bot.EntitiesAndModels;

namespace Intensive_Bot.Commands;

internal sealed class DisplayNotificationInfoCommand : BaseCommand
{
    public string CommandType => "Настроить оповещения";

    public override Task Execute(BotUser botUser)
    {
        var answer = BeautyHelper.MakeItStyled($"Для установки нового интервала оповещений пришлите мне сообщение формата:\n" +
            $"H:3, где H - hours (часы), 3 - количество часов, или формата:\n" +
            $"m:600, где m - minutes (минуты), 600 - количество минут.", UiTextStyle.Default);

        return AnswerSender.SendMessage(botUser, answer);
    }
}