using Intensive_Bot.BotCommands.Commands;
using Intensive_Bot.EntitiesAndModels;
using Intensive_Bot.Functions;

namespace Intensive_Bot.BotCommands;

internal sealed class DisplayAllMergeRequestsCommand : NamedCommand
{
    public override string CommandName => "Показать все MR";

    public override CommandType CommandType => CommandType.ShowAllMR;

    public override Task Execute(BotUser botUser)
    {
        var answer = BeautyHelper.MakeItStyled($"Все ваши активные Merge Request-ы\n", UiTextStyle.Header);

        answer += BotFunctions.GetAllActiveMergeRequests().MakeMrResponseBeautier();

        return AnswerSender.SendMessage(botUser, answer);
    }
}