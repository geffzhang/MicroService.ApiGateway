using System;
using MicroService.ApiGateway;
using Volo.Abp.EventBus;

namespace MicroService.ApiGatewayAdmin.Ocelot.Event
{
    [EventName(ApiGatewayDomainConsts.Events_OcelotConfigChanged)]
    public class OcelotConfigChangeCommand
    {
        public DateTime DateTime { get; set; }
        public string Method { get; set; }
        public string Object { get; set; }
        public OcelotConfigChangeCommand()
        {

        }

        public OcelotConfigChangeCommand(string @object, string @method)
        {
            DateTime = DateTime.Now;
            Object = @object;
            Method = @method;
        }
    }
}
