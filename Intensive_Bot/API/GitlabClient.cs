using Exceptions;
using System.Net.Http.Headers;

namespace Intensive_Bot.API;

public static class GitlabClient
{
    private static HttpClient _httpClient = new HttpClient();

    private static string _baseUrl = "";

    private static Dictionary<ApiRequestType, string> _requestStringDic = new Dictionary<ApiRequestType, string>()
    {
        {ApiRequestType.MyOpenedMergreRequests, "merge_requests?state=opened" },
        {ApiRequestType.AssignedToMeMergeRequests, "merge_requests?scope=assigned_to_me" },
    };

    static GitlabClient()
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Program.BotEnvironment.GitLabAuthToken);
        _baseUrl = Program.BotEnvironment.GitLabUrl;
    }

    public static async Task<string> CallApi(ApiRequestType requestType)
    {
        var finalRequestUrl = @$"{_baseUrl}/{_requestStringDic[requestType]}";

        using HttpRequestMessage request = new(HttpMethod.Get, finalRequestUrl);

        using HttpResponseMessage response = _httpClient.Send(request);

        BadApiResponseException.ThrowByPredicate(() => response.IsSuccessStatusCode == false,
            $"Api call failed! RequestString: {response.RequestMessage}; Reason: {response.ReasonPhrase}");

        return response.Content.ReadAsStringAsync().Result;
    }
}