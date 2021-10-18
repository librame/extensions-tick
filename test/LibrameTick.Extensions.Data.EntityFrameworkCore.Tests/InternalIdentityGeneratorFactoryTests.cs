using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace Librame.Extensions.Data
{
    public class InternalIdentityGeneratorFactoryTests
    {

        [Fact]
        public void AllTest()
        {
            var factory = DataExtensionBuilderHelper.CurrentServices
                .GetRequiredService<IIdentificationGeneratorFactory>();

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
