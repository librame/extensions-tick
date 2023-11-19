using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Librame.Extensions.Data.Accessing
{
    public class TestDataContext<TDbContext> : BaseDataContext
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

                b.Sharding(BaseDependencies.ShardingContext)
                    .HasProperty(p => p.CreatedTime)
                    .HasValue(CultureInfo.CurrentUICulture);
            });
        }

    }
}
