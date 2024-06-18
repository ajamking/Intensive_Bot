using Intensive_Bot.API;
using Intensive_Bot.Entities;
using Intensive_Bot.EntitiesAndModels;
using Newtonsoft.Json;

namespace Intensive_Bot.BLFunctions;

public static class BotFunctions
{
    public static List<MergeRequestInfoUI> GetAllActiveMergeRequests()
    {
        var apiReply = ApiRequestBuilder.CallApi(ApiRequestType.MyOpenedMergreRequests).Result;

        var mergeRequests = JsonConvert.DeserializeObject<List<MergeRequestInfoUI>>(apiReply);

        return mergeRequests;
    }

    public static List<MergeRequestInfoUI> GetAllAttachedToMeMergeRequests()
    {
        var apiReply = ApiRequestBuilder.CallApi(ApiRequestType.AssignedToMeMergeRequests).Result;

        var mergeRequests = JsonConvert.DeserializeObject<List<MergeRequestInfoUI>>(apiReply);

        return mergeRequests;
    }

    public static bool CustomizeNotifications()
    {

        return true;
    }

    public static bool SwitchNotifications(BotUser botUser)
    {
        botUser.AlertsOn = !botUser.AlertsOn;

        return botUser.AlertsOn;
    }

    public static string GetAboutInfo()
    {
        return "";
    }

}