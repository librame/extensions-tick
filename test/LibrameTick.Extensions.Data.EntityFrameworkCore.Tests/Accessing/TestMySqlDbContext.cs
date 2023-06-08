using Librame.Extensions.Core;
using Librame.Extensions.Data.Sharding;
using Librame.Extensions.Data.ValueConversion;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Librame.Extensions.Data.Accessing
{
    public class TestMySqlDbContext : TestDbContext<TestMySqlDbContext>
    {
        public TestMySqlDbContext(IShardingContext shardingContext,
            IEncryptionConverterFactory encryptionConverterFactory,
            IOptionsMonitor<DataExtensionOptions> dataOptions,
            IOptionsMonitor<CoreExtensionOptions> coreOptions,
            DbContextOptions<TestMySqlDbContext> options)
            : base(shardingContext, encryptionConverterFactory, dataOptions, coreOptions, options)
        {
        }

    }
}
