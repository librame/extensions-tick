using Librame.Extensions.Core;
using Librame.Extensions.Data.Sharding;
using Librame.Extensions.Data.ValueConversion;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Librame.Extensions.Data.Accessing
{
    public class TestDbContext<TDbContext> : BaseDbContext
        where TDbContext : DbContext
    {
        public TestDbContext(IEncryptionConverterFactory encryptionConverterFactory,
            IShardingManager shardingManager,
            IOptionsMonitor<DataExtensionOptions> dataOptionsMonitor,
            IOptionsMonitor<CoreExtensionOptions> coreOptionsMonitor,
            DbContextOptions<TDbContext> options)
            : base(encryptionConverterFactory, shardingManager, dataOptionsMonitor, coreOptionsMonitor, options)
        {
        }


        protected override void ModelCreatingCore(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(b =>
            {
                b.ToTableWithSharding(ShardingManager);

                b.HasKey(k => k.Id);

                b.HasIndex(i => i.Name);

                b.Property(p => p.Id).ValueGeneratedNever();

                b.Property(p => p.Name).HasMaxLength(50);
                b.Property(p => p.Passwd).HasMaxLength(50);
                b.Property(p => p.CreatedTime).HasMaxLength(50)
                    .Sharding<DateTimeOffsetShardingStrategy>(ShardingManager);
            });
        }

    }
}
