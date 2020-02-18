using MicroService.ApiGateway.Settings;
using Volo.Abp.EntityFrameworkCore.Modeling;
using JetBrains.Annotations;

namespace MicroService.ApiGateway.EntityFrameworkCore
{
    public class ApiGatewayModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public ApiGatewayModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = ApiGatewaySettings.DefaultDbTablePrefix,
            [CanBeNull] string schema = ApiGatewaySettings.DefaultDbSchema)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}
