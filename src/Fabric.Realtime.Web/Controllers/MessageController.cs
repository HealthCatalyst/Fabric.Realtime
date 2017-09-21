namespace Fabric.Realtime.Controllers
{
    using System.Collections.Generic;
    using System.Linq;

    using Fabric.Realtime.Data.Models;
    using Fabric.Realtime.Data.Stores;
    using Fabric.Realtime.Engine.EventBus.Models;
    using Fabric.Realtime.Engine.Transformers;

    using Microsoft.AspNetCore.Mvc;

    [Route("api/v1/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly RealtimeContext _realtimeContext;

        private readonly IInterfaceEngineMessageTransformer _transformer;

        public MessageController(RealtimeContext context, IInterfaceEngineMessageTransformer transformer)
        {
            this._realtimeContext = context;
            this._transformer = transformer;
        }

        // GET api/v1/message
        [HttpGet]
        public IEnumerable<IMessage> Get()
        {
            // Return simple list of messages for demo purposes
            var setOfMessages = this._realtimeContext.HL7Messages;
            var simpleListOfMessages = setOfMessages.ToList();
            return simpleListOfMessages.ToArray();
        }

        [HttpPost]
        public void Post([FromBody] InterfaceEngineMessage interfaceEngineMessage)
        {
            var message = this._transformer.Transform(interfaceEngineMessage);
            if (message.Protocol.Equals(MessageProtocol.HL7))
            {
                this._realtimeContext.HL7Messages.Add((HL7Message)message);
                this._realtimeContext.SaveChanges();
            }
        }
    }
}