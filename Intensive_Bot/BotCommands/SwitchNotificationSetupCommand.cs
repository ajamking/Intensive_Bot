using Intensive_Bot.BotCommands.Commands;
using Intensive_Bot.EntitiesAndModels;
using Intensive_Bot.Functions;
using System.Text.RegularExpressions;

namespace Intensive_Bot.BotCommands;

internal sealed class SwitchNotificationSetupCommand : DynamicCommand
{
    private static readonly Regex _hourRegex = new Regex(@"H:([0-9]+)");
    private static readonly Regex _minuteRegex = new Regex(@"m:([0-9]+)");

    public override bool CanExecute(string command)
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