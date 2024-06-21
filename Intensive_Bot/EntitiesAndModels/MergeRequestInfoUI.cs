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

    public override bool Equals(object? obj)
    {
        return Equals(obj as MergeRequestInfoUI);
    }

    public bool Equals(MergeRequestInfoUI? mr)
    {
        return mr is MergeRequestInfoUI uI &&
               ProjectId == uI.ProjectId &&
               Description == uI.Description &&
               State == uI.State &&
               CreatedAt == uI.CreatedAt &&
               UpdatedAt == uI.UpdatedAt &&
               TargetBranch == uI.TargetBranch &&
               SourceBranch == uI.SourceBranch &&
               
               MergeStatus == uI.MergeStatus &&
               WebUrl == uI.WebUrl;
    }

    public override int GetHashCode()
    {
        HashCode hash = new HashCode();
        hash.Add(ProjectId);
        hash.Add(Description);
        hash.Add(State);
        hash.Add(CreatedAt);
        hash.Add(UpdatedAt);
        hash.Add(TargetBranch);
        hash.Add(SourceBranch);
        hash.Add(Author);
        hash.Add(Assignees);
        hash.Add(Assignee);
        hash.Add(Reviewers);
        hash.Add(MergeStatus);
        hash.Add(WebUrl);
        return hash.ToHashCode();
    }
}

public class WorkgroupMember
{
    [JsonProperty("username")]
    public string Username { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }
}