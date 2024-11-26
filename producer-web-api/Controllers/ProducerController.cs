using Confluent.Kafka;
using MessageContract;
using Microsoft.AspNetCore.Mvc;
using producer_web_api.Services;


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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message">Request Body sample: {"key": "314", "value": { "id": "314", "description": "The PI number", "price": 314.159} }</param>
        /// <returns>value from message: type of MessageModel</returns>
        [HttpPost("sendmessage", Name = "sendmessage")] // request
        public async Task<IActionResult> SendMessageAsync([FromBody] Message<string, MessageContent> message)
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
