using Librame.Extensions.Core;
using Librame.Extensions.Data.Sharding;
using Librame.Extensions.Data.ValueConversion;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace Librame.Extensions.Data.Accessing
{
    public class TestDbContext<TDbContext> : BaseDbContext
        where TDbContext : DbContext
    {
        public TestDbContext(IShardingContext shardingContext,
            IEncryptionConverterFactory encryptionConverterFactory,
            IOptionsMonitor<DataExtensionOptions> dataOptions,
            IOptionsMonitor<CoreExtensionOptions> coreOptions,
            DbContextOptions<TDbContext> options)
            : base(shardingContext, encryptionConverterFactory, dataOptions, coreOptions, options)
        {
        }


        protected override void OnModelCreatingCore(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(b =>
            {
                b.ToTableWithSharding(ShardingContext);

                b.HasKey(k => k.Id);

                b.HasIndex(i => i.Name);

                b.Property(p => p.Id).ValueGeneratedNever();

                b.Property(p => p.Name).HasMaxLength(50);
                b.Property(p => p.Passwd).HasMaxLength(50);
                b.Property(p => p.CreatedTime).HasMaxLength(50);

                b.Sharding(ShardingContext)
                    .HasProperty(p => p.CreatedTime)
                    .HasValue(CultureInfo.CurrentUICulture);
            });
        }

    }
}
