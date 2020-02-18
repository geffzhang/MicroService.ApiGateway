using System.Collections.Generic;
using System.Threading.Tasks;
using MicroService.ApiGateway.Entites.Ocelot;
using MicroService.ApiGateway.Ocelot.Dto;
using MicroService.ApiGateway.Repositories;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.EventBus.Distributed;

namespace MicroService.ApiGateway.Ocelot
{
    [Route("AggregateReRoute")]
    public class AggregateReRouteAppService : ApiGatewayApplicationServiceBase, IAggregateReRouteAppService
    {
        private readonly IAggregateReRouteRepository _aggregateReRouteRepository;
        private readonly IDistributedEventBus _distributedEventBus;

        public AggregateReRouteAppService(IAggregateReRouteRepository aggregateReRouteRepository,
            IDistributedEventBus distributedEventBus
            )
        {
            _aggregateReRouteRepository = aggregateReRouteRepository;
            _distributedEventBus = distributedEventBus;
        }

        [HttpGet]
        [Route("GetList")]
        public async Task<ListResultDto<AggregateReRouteDto>> GetListAsync()
        {
            var reroutes = await _aggregateReRouteRepository.GetListAsync(true);

            return new ListResultDto<AggregateReRouteDto>(ObjectMapper.Map<List<AggregateReRoute>, List<AggregateReRouteDto>>(reroutes));
        }

        [HttpGet]
        [Route("GetPagedList")]
        public async Task<PagedResultDto<AggregateReRouteDto>> GetPagedListAsync(PagedResultRequestDto requestDto)
        {
            var reroutesTuple = await _aggregateReRouteRepository.GetPagedListAsync(requestDto.SkipCount, requestDto.MaxResultCount);

            return new PagedResultDto<AggregateReRouteDto>(reroutesTuple.total,
                ObjectMapper.Map<List<AggregateReRoute>, List<AggregateReRouteDto>>(reroutesTuple.routes));
        }
    }
}
