using Intensive_Bot.BLFunctions;
using Intensive_Bot.Entities;
using Intensive_Bot.EntitiesAndModels;

namespace Intensive_Bot.Commands;

internal sealed class DisplayAllMergeRequestsCommand : BaseCommand
{
    public string CommandType => "Показать все MR";

    public override Task Execute(BotUser botUser)
    {
        var answer = BeautyHelper.MakeItStyled($"Все ваши активные Merge Request-ы\n", UiTextStyle.Header);

        answer += BotFunctions.GetAllActiveMergeRequests().MakeMrResponseBeautier();

        return AnswerSender.SendMessage(botUser, answer);
    }
}