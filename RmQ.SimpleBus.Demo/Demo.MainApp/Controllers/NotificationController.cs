using Demo.MainApp.PriorityQueue;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Demo.MainApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public sealed class NotificationController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly PriorityQueueRmQ<Notifaction> _priorityQueueRmQ;

        public NotificationController(ILogger<WeatherForecastController> logger, PriorityQueueRmQ<Notifaction> priorityQueueRmQ)
        {
            _logger = logger;
            _priorityQueueRmQ = priorityQueueRmQ;
        }


        [HttpPost]
        public void Notify(Notifaction notifaction)
        {
            _priorityQueueRmQ.Enqueue(notifaction);
        }
    }
}
