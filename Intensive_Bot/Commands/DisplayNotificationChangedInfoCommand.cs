using Intensive_Bot.BLFunctions;
using Intensive_Bot.Entities;
using Intensive_Bot.EntitiesAndModels;
using System.Text.RegularExpressions;

namespace Intensive_Bot.Commands;

internal sealed class DisplayNotificationChangedInfoCommand : BaseCommand
{
    private static readonly Regex _hourRegex = new Regex(@"H:([0-9]+)");
    private static readonly Regex _minuteRegex = new Regex(@"m:([0-9]+)");

    public string CommandType => "";

    public override bool CanProcessCommand(string command)
        => _hourRegex.IsMatch(command) || _minuteRegex.IsMatch(command);

    public override Task Execute(BotUser botUser)
    {
        if (_hourRegex.IsMatch(botUser.Message.Text))
        {
            BotFunctions.CustomizeNotifications(botUser, int.Parse(Regex.Match(botUser.Message.Text, @"\d+").Value) * 60);
        }
        else
        {
            BotFunctions.CustomizeNotifications(botUser, int.Parse(Regex.Match(botUser.Message.Text, @"\d+").Value));
        }

        BotFunctions.SwitchNotifications(botUser);

        var answer = BeautyHelper.MakeItStyled($"Изменения успешно применены!\nТеперь вы будете получать информацию " +
            $"о новых MergeRequest-ах каждые {botUser.NotificationFrequencyMinutes} минут", UiTextStyle.Default);

        return AnswerSender.SendMessage(botUser, answer);
    }
}