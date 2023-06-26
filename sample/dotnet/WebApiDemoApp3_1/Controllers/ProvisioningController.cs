using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealPage.Webhooks.Models;
using System.Text.Json;

namespace WebApiDemoApp3_1.Controllers
{
    [Route("api/provisioning")]
    [ApiController]
    public class ProvisioningController : ControllerBase
    {

        [HttpPost]
        [AllowAnonymous]
        [WebhookSubscription]
        public IActionResult Post([FromBody] WebhookEvent<JsonElement> webhookEvent)
        {
            // Process the provision event.
            return Ok();
        }

        [HttpPut]
        public IActionResult Put()
        {
            return Ok();
        }
    }
}
