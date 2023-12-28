using Librame.Extensions.IdGenerators;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace Librame.Extensions.Data
{
    public class InternalIdGeneratorFactoryTests
    {

        [Fact]
        public void AllTest()
        {
            var factory = DataExtensionBuilderHelper.CurrentServices
                .GetRequiredService<IIdGeneratorFactory>();

            var combId = factory.GetCombIdGeneratorForMySql().GenerateId();
            Assert.NotEqual(Guid.Empty, combId);

            combId = factory.GetCombIdGeneratorForOracle().GenerateId();
            Assert.NotEqual(Guid.Empty, combId);

            combId = factory.GetCombIdGeneratorForSqlite().GenerateId();
            Assert.NotEqual(Guid.Empty, combId);

            combId = factory.GetCombIdGeneratorForSqlServer().GenerateId();
            Assert.NotEqual(Guid.Empty, combId);

            combId = factory.GetCombSnowflakeIdGenerator().GenerateId();
            Assert.NotEqual(Guid.Empty, combId);

            var snowflakeId = factory.GetSnowflakeIdGenerator().GenerateId();
            Assert.True(snowflakeId > 0);

            var mongoId = factory.GetMongoIdGenerator().GenerateId();
            Assert.NotEmpty(mongoId);
        }

    }
}
