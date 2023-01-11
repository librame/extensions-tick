using Librame.Extensions.Core;
using Librame.Extensions.Data.Sharding;
using Librame.Extensions.Data.ValueConversion;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Librame.Extensions.Data.Accessing
{
    public class TestDbContext : BaseDbContext
    {
        public TestDbContext(IEncryptionConverterFactory encryptionConverterFactory,
            IShardingManager shardingManager,
            IOptionsMonitor<DataExtensionOptions> dataOptionsMonitor,
            IOptionsMonitor<CoreExtensionOptions> coreOptionsMonitor,
            DbContextOptions options)
            : base(encryptionConverterFactory, shardingManager, dataOptionsMonitor, coreOptionsMonitor, options)
        {
        }


        protected override void CustomModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(b =>
            {
                b.ToTableWithSharding(ShardingManager, this);

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
