using Intensive_Bot.API;
using Intensive_Bot.EntitiesAndModels;
using Newtonsoft.Json;

namespace Intensive_Bot.Functions;

public static class BotFunctions
{
    public static List<MergeRequestInfoUI> GetAllActiveMergeRequests()
    {
        var apiReply = GitlabClient.CallApi(ApiRequestType.MyOpenedMergreRequests).Result;

        var mergeRequests = JsonConvert.DeserializeObject<List<MergeRequestInfoUI>>(apiReply);

        return mergeRequests;
    }

    public static List<MergeRequestInfoUI> GetAllAttachedToMeMergeRequests()
    {
        var apiReply = GitlabClient.CallApi(ApiRequestType.AssignedToMeMergeRequests).Result;

        var mergeRequests = JsonConvert.DeserializeObject<List<MergeRequestInfoUI>>(apiReply);

        return mergeRequests;
    }

    public static bool CustomizeNotifications(BotUser botUser, int minutes)
    {
        botUser.NotificationFrequencyMinutes = minutes;

        return true;
    }

    public static bool SwitchNotifications(BotUser botUser)
    {
        botUser.NotificationEnabled = !botUser.NotificationEnabled;

        return botUser.NotificationEnabled;
    }

    public static string GetAboutInfo()
    {
        return "";
    }

}