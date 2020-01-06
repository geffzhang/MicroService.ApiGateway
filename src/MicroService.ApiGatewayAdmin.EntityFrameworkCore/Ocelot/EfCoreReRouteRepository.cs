﻿using MicroService.ApiGateway.Entites.Ocelot;
using MicroService.ApiGateway.EntityFrameworkCore;
using MicroService.ApiGateway.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.EntityFrameworkCore;

namespace MicroService.ApiGateway.Ocelot
{
    public class EfCoreReRouteRepository : ApiGatewayEfCoreRepositoryBase<ApiGatewayDbContext, ReRoute, int>, IReRouteRepository
    {
        public EfCoreReRouteRepository(IDbContextProvider<ApiGatewayDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {
        }

        public async Task<ReRoute> GetByNameAsync(string routeName)
        {
            var reRoute = await WithDetails().Where(x => x.ReRouteName.Equals(routeName)).FirstOrDefaultAsync();
            return reRoute ?? throw new EntityNotFoundException(typeof(ReRoute), routeName);
        }

        public async Task<ReRoute> GetByReRouteIdAsync(long routeId)
        {
            var reRoute = await WithDetails().Where(x => x.ReRouteId.Equals(routeId)).FirstOrDefaultAsync();
            return reRoute ?? throw new EntityNotFoundException(typeof(ReRoute), routeId);
        }

        public async Task<(List<ReRoute> routes, long total)> GetPagedListAsync(int skipCount = 1, int maxResultCount = 100)
        {
            var resultReRoutes = await WithDetails()
                .EfPageBy(skipCount, maxResultCount)
                .ToListAsync();

            var total = await GetQueryable().LongCountAsync();

            return ValueTuple.Create(resultReRoutes, total);
        }

        public async Task RemoveAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            //var entityType = DbContext.Model.FindEntityType(typeof(ReRoute));
            //var tableName = entityType.().TableName;

            //var sqlText = $"DELETE FROM @tableName";
            //var sqlParam = new List<object> { new { tableName } };

            //// TODO: Test
            //await DbContext.Database.ExecuteSqlCommandAsync(sqlText, sqlParam, cancellationToken);
        }

        public override IQueryable<ReRoute> WithDetails()
        {
            return WithDetails(
                x => x.AuthenticationOptions,
                x => x.CacheOptions,
                x => x.HttpHandlerOptions,
                x => x.LoadBalancerOptions,
                x => x.QoSOptions,
                x => x.RateLimitOptions,
                x => x.SecurityOptions);
        }
    }
}
