using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WebhookUtilities.Common;
using WebhookUtilities.Common.Docker;

namespace WebhookUtilities.Business
{
    public class WebhookProcesser
    {
        public WebhookProcesser(WebhookConfiguration webhookConfiguration, ILogger<WebhookProcesser> logger)
        {
            WebhookConfiguration = webhookConfiguration;
            Logger = logger;
        }

        public async Task<U> Process<T, U>(WebhookRequest<T, U> webhookRequest) where T : class where U : class
        {
            return webhookRequest switch
            {
                WebhookRequest<DockerRequestWrapper, DockerResponse> dockerWebhookRequest => (await DockerProcesser.Process(dockerWebhookRequest, WebhookConfiguration)) as U,
                _ => throw new NotImplementedException("Not Support Process " + webhookRequest.Request.ToString()),
            };
        }

        DockerProcesser dockerProcesser;

        public DockerProcesser DockerProcesser => dockerProcesser ??= new DockerProcesser(Logger);

        public WebhookConfiguration WebhookConfiguration { get; }
        public ILogger<WebhookProcesser> Logger { get; }
    }
}
