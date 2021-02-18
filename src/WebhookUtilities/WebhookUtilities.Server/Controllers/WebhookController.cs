using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebhookUtilities.Common;
using WebhookUtilities.Common.Docker;

namespace WebhookUtilities.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        public WebhookController(WebhookConfiguration webhookConfiguration)
        {
            WebhookConfiguration = webhookConfiguration;
        }

        public WebhookConfiguration WebhookConfiguration { get; }

        [HttpPost]
        public async Task<DockerResponse> DockerWebhookRunningScript(DockerRequest dockerRequest, [FromQuery] string token, [FromQuery] string scriptName)
        {
            return await DockerProcesser.Process(dockerRequest, token, scriptName, WebhookConfiguration);
        }
    }
}
