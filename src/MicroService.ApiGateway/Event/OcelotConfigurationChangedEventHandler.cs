using DotNetCore.CAP;
using MicroService.ApiGatewayAdmin.Ocelot.Event;
using Microsoft.Extensions.Logging;
using Ocelot.Configuration.Creator;
using Ocelot.Configuration.Repository;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace MicroService.ApiGateway.Event
{
    public class OcelotConfigurationChangedEventHandler: IDistributedEventHandler<OcelotConfigChangeCommand>, ITransientDependency
    {
        private readonly IDistributedEventBus _distributedEventBus;
        private readonly ILogger<OcelotConfigurationChangedEventHandler> _logger;

        private readonly IFileConfigurationRepository _fileConfigRepo;
        private readonly IInternalConfigurationRepository _internalConfigRepo;
        private readonly IInternalConfigurationCreator _internalConfigCreator;
        public OcelotConfigurationChangedEventHandler(
            IFileConfigurationRepository fileConfigRepo,
            IInternalConfigurationRepository internalConfigRepo,
            IInternalConfigurationCreator internalConfigCreator,
            ILogger<OcelotConfigurationChangedEventHandler> logger)
        {
            _fileConfigRepo = fileConfigRepo;
            _internalConfigRepo = internalConfigRepo;
            _internalConfigCreator = internalConfigCreator;
            _logger = logger;
        }

        public OcelotConfigurationChangedEventHandler(IDistributedEventBus distributedEventBus, ILoggerFactory loggerFactory)
        {
            _distributedEventBus = distributedEventBus;
            _logger = loggerFactory.CreateLogger<OcelotConfigurationChangedEventHandler>();

        }

        public async Task HandleEventAsync(OcelotConfigChangeCommand eventData)
        {
            _logger.LogInformation("************************ INCOMING MESSAGE ****************************");
            _logger.LogInformation(JsonConvert.SerializeObject(eventData));
            _logger.LogInformation("**********************************************************************");

            var fileConfig = await _fileConfigRepo.Get();

            if (fileConfig.IsError)
            {
                _logger.LogWarning($"error geting file config, errors are {string.Join(",", fileConfig.Errors.Select(x => x.Message))}");
                return;
            }
            else
            {
                var config = await _internalConfigCreator.Create(fileConfig.Data);
                if (!config.IsError)
                {
                    _internalConfigRepo.AddOrReplace(config.Data);
                }
            }
            _logger.LogInformation("ocelot configuration changed on {0}", eventData.DateTime);
        }
    }
}
