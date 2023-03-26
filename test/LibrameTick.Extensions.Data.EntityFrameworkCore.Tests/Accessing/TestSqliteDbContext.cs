using Librame.Extensions.Core;
using Librame.Extensions.Data.Sharding;
using Librame.Extensions.Data.ValueConversion;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Librame.Extensions.Data.Accessing
{
    public class TestSqliteDbContext : TestDbContext<TestSqliteDbContext>
    {
        public TestSqliteDbContext(IEncryptionConverterFactory encryptionConverterFactory,
            IShardingManager shardingManager,
            IOptionsMonitor<DataExtensionOptions> dataOptionsMonitor,
            IOptionsMonitor<CoreExtensionOptions> coreOptionsMonitor,
            DbContextOptions<TestSqliteDbContext> options)
            : base(encryptionConverterFactory, shardingManager, dataOptionsMonitor, coreOptionsMonitor, options)
        {
        }

    }
}
