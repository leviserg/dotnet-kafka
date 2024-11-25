using MessageContract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using producer_web_api.Services;
using System.Text.Json;

namespace producer_web_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProducerController : ControllerBase
    {
        private readonly ILogger<ProducerController> _logger;
        private readonly IProducer _producer;

        public ProducerController(ILogger<ProducerController> logger, IProducer producer)
        {
            _logger = logger;
            _producer = producer;
        }

        [HttpPost("sendmessage", Name = "sendmessage")]
        public async Task<IActionResult> SendMessageAsync([FromBody] MessageModel message)
        {
            try
            {
                var result = await _producer.SendMessageAsync(message);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
