using Intensive_Bot.BLFunctions;
using Intensive_Bot.Entities;
using Intensive_Bot.EntitiesAndModels;

namespace Intensive_Bot.Commands;

internal sealed class DisplaySelfAssignedMergeRequestsCommand : BaseCommand
{
    public string CommandType => "Показать мои MR";

    public override Task Execute(BotUser botUser)
    {
        var answer = BeautyHelper.MakeItStyled($"Все закрепленные за вами активные Merge Request-ы\n", UiTextStyle.Header);

        answer += BotFunctions.GetAllAttachedToMeMergeRequests().MakeMrResponseBeautier();

        return AnswerSender.SendMessage(botUser, answer);
    }
}