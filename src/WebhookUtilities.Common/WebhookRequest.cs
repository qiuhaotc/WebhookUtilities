namespace WebhookUtilities.Common
{
    public class WebhookRequest<T, U> where T : class where U : class
    {
        public T Request { get; set; }
        public string Token { get; set; }
    }
}
