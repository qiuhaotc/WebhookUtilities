using Newtonsoft.Json;

namespace WebhookUtilities.Common.Docker
{
    public class DockerResponse
    {
        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("context")]
        public string Context { get; set; }

        [JsonProperty("target_url")]
        public string TargetUrl { get; set; }
    }
}
