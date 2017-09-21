namespace Fabric.Realtime.Web.Controllers
{
    // ReSharper disable StyleCop.SA1650
    using System.Collections.Generic;

    using Fabric.Realtime.Domain;
    using Fabric.Realtime.Services;

    using Microsoft.AspNetCore.Mvc;

    // TODO Enhance this API to return messages within a specified time range and possibly with a max number option.
    
    /// <summary>
    /// The message controller.
    /// </summary>
    [Route("api/v1/[controller]")]
    public class MessageController : ControllerBase
    {
        /// <summary>
        /// The message store service.
        /// </summary>
        private readonly IMessageStoreService messageStoreService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageController"/> class.
        /// </summary>
        /// <param name="messageStoreService">
        /// The message store service.
        /// </param>
        public MessageController(IMessageStoreService messageStoreService)
        {
            this.messageStoreService = messageStoreService;
        }

        /// <summary>
        /// GET api/v1/message
        /// </summary>
        /// <returns>
        /// Enumerable of <see cref="HL7Message"/>.
        /// </returns>
        [HttpGet]
        public IEnumerable<HL7Message> Get()
        {
            // Return simple list of messages for demo purposes
            return this.messageStoreService.GetAll();
        }
    }
}