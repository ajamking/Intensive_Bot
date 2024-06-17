using Newtonsoft.Json;

namespace Intensive_Bot.EntitiesAndModels;

public class MergeRequestInfoUI
{
    [JsonProperty("project_id")]
    public string ProjectId { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("state")]
    public string State { get; set; }

    [JsonProperty("created_at")]
    public string CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    public string UpdatedAt { get; set; }

    [JsonProperty("target_branch")]
    public string TargetBranch { get; set; }

    [JsonProperty("source_branch")]
    public string SourceBranch { get; set; }

    [JsonProperty("author")]
    public WorkgroupMember Author { get; set; }

    [JsonProperty("assignees")]
    public ICollection<WorkgroupMember> Assignees { get; set; }

    [JsonProperty("assignee")]
    public WorkgroupMember Assignee { get; set; }

    [JsonProperty("reviewers")]
    public ICollection<WorkgroupMember> Reviewers { get; set; }

    [JsonProperty("merge_status")]
    public string MergeStatus { get; set; }

    [JsonProperty("web_url")]
    public string WebUrl { get; set; }
}

public class WorkgroupMember
{
    [JsonProperty("username")]
    public string Username { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }
}