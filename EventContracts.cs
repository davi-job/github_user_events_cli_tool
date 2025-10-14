using System.Text.Json;
using System.Text.Json.Serialization;

namespace GithubContracts
{
    public class GitHubEvent
    {
        [JsonPropertyName("id")]
        public required string Id { get; set; }

        [JsonPropertyName("type")]
        public required string Type { get; set; }

        [JsonPropertyName("actor")]
        public required Actor Actor { get; set; }

        [JsonPropertyName("repo")]
        public required Repository Repo { get; set; }

        [JsonPropertyName("payload")]
        public JsonElement Payload { get; set; }

        [JsonPropertyName("public")]
        public bool Public { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("org")]
        public Organization? Org { get; set; }
    }

    public class Actor
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("login")]
        public required string Login { get; set; }

        [JsonPropertyName("display_login")]
        public string? DisplayLogin { get; set; }

        [JsonPropertyName("gravatar_id")]
        public string? GravatarId { get; set; }

        [JsonPropertyName("url")]
        public required string Url { get; set; }

        [JsonPropertyName("avatar_url")]
        public string? AvatarUrl { get; set; }
    }

    public class Repository
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("url")]
        public required string Url { get; set; }
    }

    public class Organization
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("login")]
        public required string Login { get; set; }

        [JsonPropertyName("gravatar_id")]
        public string? GravatarId { get; set; }
        [JsonPropertyName("url")]
        public required string Url { get; set; }

        [JsonPropertyName("avatar_url")]
        public string? AvatarUrl { get; set; }
    }

    // Payloads

    public class CreateDeleteEventPayload
    {
        [JsonPropertyName("ref_type")]
        public required string RefType { get; set; }
    }

    public class PushEventPayload
    {
        [JsonPropertyName("size")]
        public required int Size { get; set; }
    }

    public class IssueEventPayload
    {
        [JsonPropertyName("action")]
        public required string Action { get; set; }
    }

    public class PullRequestEventPayload : IssueEventPayload { };

    public class WatchEventPayload : IssueEventPayload { };
}