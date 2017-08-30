namespace Fabric.Realtime.Controllers
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;

    using Fabric.Realtime.Domain.Models;
    using Fabric.Realtime.Domain.Stores;
    using Fabric.Realtime.EventBus.Models;
    using Newtonsoft.Json;
    using Fabric.Realtime.Transformers;
    using Microsoft.EntityFrameworkCore;

    [Route("api/v1/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly RealtimeContext _realtimeContext;

        private readonly IInterfaceEngineMessageTransformer _transformer;

        //private readonly RealtimeSettings _settings;

        //private readonly IRealtimeIntegrationEventService _realtimeIntegrationEventService;

        public MessageController(RealtimeContext context, IInterfaceEngineMessageTransformer transformer)
        {
            _realtimeContext = context;
            _transformer = transformer;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<Message> Get()
        {
            // Return simple list of messages for demo purposes
            DbSet<HL7Message> setOfMessages = this._realtimeContext.HL7Messages;
            var simpleListOfMessages = setOfMessages.ToList<HL7Message>();
            return simpleListOfMessages.ToArray();
        }

        [HttpPost]
        public void Post([FromBody] InterfaceEngineMessage interfaceEngineMessage)
        {
            Message message = this._transformer.Transform(interfaceEngineMessage);
            if (message.Protocol.Equals(MessageProtocol.HL7))
            {
                _realtimeContext.HL7Messages.Add((HL7Message)message);
                _realtimeContext.SaveChanges();
            }
        }
    }
}
