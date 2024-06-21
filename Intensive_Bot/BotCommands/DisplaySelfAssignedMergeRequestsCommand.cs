using Intensive_Bot.BotCommands.Commands;
using Intensive_Bot.EntitiesAndModels;
using Intensive_Bot.Functions;

namespace Intensive_Bot.BotCommands;

internal sealed class DisplaySelfAssignedMergeRequestsCommand : NamedCommand
{
    public override string CommandName => "Показать мои MR";

    public override CommandType CommandType => CommandType.ShowMyMR;

    public override Task Execute(BotUser botUser)
    {
        var answer = BeautyHelper.MakeItStyled($"Все закрепленные за вами активные Merge Request-ы\n", UiTextStyle.Header);

        answer += BotFunctions.GetAllAttachedToMeMergeRequests().MakeMrResponseBeautier();

        return AnswerSender.SendMessage(botUser, answer);
    }
}