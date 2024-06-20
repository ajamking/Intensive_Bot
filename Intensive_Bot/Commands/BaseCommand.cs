using Intensive_Bot.BLFunctions;
using Intensive_Bot.Entities;
using Intensive_Bot.EntitiesAndModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Intensive_Bot.Commands;

internal abstract class BaseCommand
{
    public string CommandType { get; }

    public virtual bool CanProcessCommand(string command)
        => CommandType == command;

    public abstract Task Execute(BotUser botUser);
}

internal sealed class HandleStartMessageCommand : BaseCommand
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

internal sealed class ShowAllMergeRequestsCommand : BaseCommand
{
    public string CommandType => "Показать все MR";

    public override Task Execute(BotUser botUser)
    {
        var answer = BeautyHelper.MakeItStyled($"Все ваши активные Merge Request-ы\n", UiTextStyle.Header);

        answer += BotFunctions.GetAllActiveMergeRequests().MakeMrResponseBeautier();

        return AnswerSender.SendMessage(botUser, answer);
    }
}

internal sealed class ShowSelfAssignedMergeRequestsCommand : BaseCommand
{
    public string CommandType => "Показать мои MR";

    public override Task Execute(BotUser botUser)
    {
        var answer = BeautyHelper.MakeItStyled($"Все закрепленные за вами активные Merge Request-ы\n", UiTextStyle.Header);

        answer += BotFunctions.GetAllAttachedToMeMergeRequests().MakeMrResponseBeautier();

        return AnswerSender.SendMessage(botUser, answer);
    }
}

internal sealed class ShowNotificationInfoCommand : BaseCommand
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

internal sealed class SwitchMergeRequestsNotificationCommand : BaseCommand
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

internal sealed class ShowAboutBotInfoCommand : BaseCommand
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

internal sealed class HandleUnknown
{ 
    public Task Execute(BotUser botUser)
        => AnswerSender.SendMessage(botUser, "Я на этом свете недавно и еще не знаю таких сложных команд 😢");
}

