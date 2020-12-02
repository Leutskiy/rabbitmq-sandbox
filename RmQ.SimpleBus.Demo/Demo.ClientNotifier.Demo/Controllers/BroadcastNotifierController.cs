using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Demo.ClientNotifier.Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BroadcastNotifierController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private int _messageCount = 1000;

        public BroadcastNotifierController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("start")]
        public async Task StartBroadcasting()
        {

            var client = _httpClientFactory.CreateClient("broadcast_service");
            

            while (_messageCount > 0)
            {
                var notifaction = new Notifaction($"test message № {_messageCount - (_messageCount--) + 1}");

                var notifactionJson = new StringContent(
                    JsonSerializer.Serialize(notifaction),
                    Encoding.UTF8,
                    "application/json");

                await client.PostAsync("notification", notifactionJson);
            }
        }
    }
}
