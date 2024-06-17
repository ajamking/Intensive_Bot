using Intensive_Bot.API;
using Intensive_Bot.Entities;
using Intensive_Bot.EntitiesAndModels;
using Newtonsoft.Json;

namespace Intensive_Bot.BLFunctions;

public static class KeyboardFunctions
{
    public static string GetAllActiveMergeRequests()
    {
        var answer = BeautyHelper.MakeItStyled($"Все ваши активные Merge Request-ы\n", UiTextStyle.Header);

        var apiReply = ApiRequestBuilder.CallApi(ApiRequestType.MyOpenedMergreRequests).Result;

        var mergeRequests = JsonConvert.DeserializeObject<List<MergeRequestInfoUI>>(apiReply);

        answer += mergeRequests.MakeMrResponseBeautier();

        return answer;
    }

    public static string GetAllAttachedToMeMergeRequests()
    {
        var answer = BeautyHelper.MakeItStyled($"Все закрепленные за вами активные Merge Request-ы\n", UiTextStyle.Header);

        var apiReply = ApiRequestBuilder.CallApi(ApiRequestType.AssignedToMeMergeRequests).Result;

        var mergeRequests = JsonConvert.DeserializeObject<List<MergeRequestInfoUI>>(apiReply);

        answer += mergeRequests.MakeMrResponseBeautier();

        return answer;
    }

    public static bool CustomizeNotifications()
    {

        return true;
    }

    public static string SwitchNotifications(BotUser botUser)
    {
        botUser.AlertsOn = !botUser.AlertsOn;

        return botUser.AlertsOn ? "Регулярные оповещения включены! ✅" : "Регулярные оповещения отключены! 🔴";
    }

    public static string GetAboutInfo()
    {
        return "";
    }

}