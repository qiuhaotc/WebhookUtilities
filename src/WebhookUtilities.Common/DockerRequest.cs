using System.Collections.Generic;
using Newtonsoft.Json;

namespace WebhookUtilities.Common.Docker
{
    public class PushData
    {
        [JsonProperty("images")]
        public List<string> Images { get; set; }

        [JsonProperty("pushed_at")]
        public double PushedAt { get; set; }

        [JsonProperty("pusher")]
        public string Pusher { get; set; }

        [JsonProperty("tag")]
        public string Tag { get; set; }
    }

    public class Repository
    {
        [JsonProperty("comment_count")]
        public int CommentCount { get; set; }

        [JsonProperty("date_created")]
        public double DateCreated { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("dockerfile")]
        public string Dockerfile { get; set; }

        [JsonProperty("full_description")]
        public string FullDescription { get; set; }

        [JsonProperty("is_official")]
        public bool IsOfficial { get; set; }

        [JsonProperty("is_private")]
        public bool IsPrivate { get; set; }

        [JsonProperty("is_trusted")]
        public bool IsTrusted { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("namespace")]
        public string Namespace { get; set; }

        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("repo_name")]
        public string RepoName { get; set; }

        [JsonProperty("repo_url")]
        public string RepoUrl { get; set; }

        [JsonProperty("star_count")]
        public int StarCount { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public class DockerRequest
    {
        [JsonProperty("callback_url")]
        public string CallbackUrl { get; set; }

        [JsonProperty("push_data")]
        public PushData PushData { get; set; }

        [JsonProperty("repository")]
        public Repository Repository { get; set; }
    }

    public class DockerRequestWrapper
    {
        public DockerRequestWrapper(DockerRequest dockerRequest, string scriptName)
        {
            DockerRequest = dockerRequest;
            ScriptName = scriptName;
        }

        public DockerRequest DockerRequest { get; set; }
        public string ScriptName { get; set; }
    }
}
