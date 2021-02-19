using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebhookUtilities.Common;
using WebhookUtilities.Common.Docker;

namespace WebhookUtilities.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        public WebhookController(WebhookConfiguration webhookConfiguration, ILogger<WebhookController> logger)
        {
            WebhookConfiguration = webhookConfiguration;
            Logger = logger;
        }

        public WebhookConfiguration WebhookConfiguration { get; }
        public ILogger<WebhookController> Logger { get; }

        [HttpPost]
        public async Task<DockerResponse> DockerWebhookRunningScript(DockerRequest dockerRequest, [FromQuery] string token, [FromQuery] string scriptName)
        {
            Logger.LogDebug("Token: " + token + " Script Name:" + scriptName + " Request: " + JsonConvert.SerializeObject(dockerRequest));

            var response = await DockerProcesser.Process(dockerRequest, token, scriptName, WebhookConfiguration);
            Logger.LogDebug("Response: " + JsonConvert.SerializeObject(response));

            return response;
        }
    }
}
