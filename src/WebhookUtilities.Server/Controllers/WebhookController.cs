using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebhookUtilities.Business;
using WebhookUtilities.Common;
using WebhookUtilities.Common.Docker;

namespace WebhookUtilities.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        public WebhookController(WebhookProcesser webhookProcesser, ILogger<WebhookController> logger)
        {
            WebhookProcesser = webhookProcesser;
            Logger = logger;
        }

        public WebhookProcesser WebhookProcesser { get; }
        public ILogger<WebhookController> Logger { get; }

        [HttpPost]
        public async Task<DockerResponse> DockerWebhookRunningScript(DockerRequest dockerRequest, [FromQuery] string token, [FromQuery] string scriptName)
        {
            Logger.LogDebug("Token: " + token + " Script Name:" + scriptName + " Request: " + JsonConvert.SerializeObject(dockerRequest));

            var response = await WebhookProcesser.Process(new WebhookRequest<DockerRequestWrapper, DockerResponse>
            {
                Request = new DockerRequestWrapper(dockerRequest, scriptName),
                Token = token
            });

            Logger.LogDebug("Response: " + JsonConvert.SerializeObject(response));

            return response;
        }
    }
}
