using Librame.Extensions.Data.Sharding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;

namespace Librame.Extensions.Data.Accessing
{

    [ShardingDatabase("%dto:wk", typeof(DateTimeOffsetShardingStrategy))]
    public class TestDataContext<TDbContext> : DataContext, IShardingValue<DateTimeOffset>
        where TDbContext : DbContext
    {
        public TestDataContext(DbContextOptions<TDbContext> options)
            : base(options)
        {
        }


        protected override void OnModelCreatingCore(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(b =>
            {
                b.HasKey(k => k.Id);

                b.HasIndex(i => i.Name);

                b.Property(p => p.Id).ValueGeneratedNever();

                b.Property(p => p.Name).HasMaxLength(50);
                b.Property(p => p.Passwd).HasMaxLength(50);
                b.Property(p => p.CreatedTime).HasMaxLength(50);

                b.Sharding().HasProperty(p => p.CreatedTime).HasValue(() => CultureInfo.CurrentUICulture);
            });
        }


        /// <summary>
        /// 获取分片值。
        /// </summary>
        /// <returns>返回 <see cref="DateTimeOffset"/>。</returns>
        public virtual DateTimeOffset GetShardedValue(DateTimeOffset defaultValue)
            => DateTimeOffset.Now;

    }
}
