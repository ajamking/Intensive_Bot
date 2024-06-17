using Exceptions;
using System.Net.Http.Headers;

namespace Intensive_Bot.API;

public static class ApiRequestBuilder
{
    private static HttpClient _httpClient = new();

    private static string _baseUrl = "https://gitlab.com/api/v4";

    private static Dictionary<ApiRequestType, string> _requestStringDic = new()
    {
        {ApiRequestType.MyOpenedMergreRequests, "merge_requests?state=opened" },
        {ApiRequestType.AssignedToMeMergeRequests, "merge_requests?scope=assigned_to_me" },
    };

    static ApiRequestBuilder()
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Program.BotEnvironment.GitLabAuthToken);
    }

    public static async Task<string> CallApi(ApiRequestType requestType)
    {
        var finalRequestUrl = @$"{_baseUrl}/{_requestStringDic[requestType]}";

        using HttpRequestMessage request = new(HttpMethod.Get, finalRequestUrl);

        using HttpResponseMessage response = _httpClient.Send(request);

        BadApiResponseException.ThrowByPredicate(() => response.IsSuccessStatusCode == false, 
            $"Api call failed! RequestString: {response.RequestMessage}; Reason: {response.ReasonPhrase}");

        await Task.Delay(0);

        return response.Content.ReadAsStringAsync().Result;
    }
}