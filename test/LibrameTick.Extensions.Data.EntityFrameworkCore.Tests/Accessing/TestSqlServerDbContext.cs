using Librame.Extensions.Core;
using Librame.Extensions.Data.Sharding;
using Librame.Extensions.Data.ValueConversion;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Librame.Extensions.Data.Accessing
{
    public class TestSqlServerDbContext : TestDbContext<TestSqlServerDbContext>
    {
        public TestSqlServerDbContext(IShardingContext shardingContext,
            IEncryptionConverterFactory encryptionConverterFactory,
            IOptionsMonitor<DataExtensionOptions> dataOptionsMonitor,
            IOptionsMonitor<CoreExtensionOptions> coreOptionsMonitor,
            DbContextOptions<TestSqlServerDbContext> options)
            : base(shardingContext, encryptionConverterFactory, dataOptionsMonitor, coreOptionsMonitor, options)
        {
        }

    }
}
