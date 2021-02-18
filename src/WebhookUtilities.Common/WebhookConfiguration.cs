namespace WebhookUtilities.Common
{
    public record WebhookConfiguration
    {
        public string Token { get; set; }
        public string ScriptsFolder { get; set; }
    }
}
