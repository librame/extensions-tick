using Microsoft.EntityFrameworkCore;

namespace Librame.Extensions.Data.Accessors
{
    public class TestWriteAccessor : AbstractAccessor<TestWriteAccessor>, ITestAccessor
    {
        public TestWriteAccessor(DbContextOptions<TestWriteAccessor> options)
            : base(options)
        {
        }


        public DbSet<User>? Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(b =>
            {
                //b.ToTable();

                b.HasKey(k => k.Id);

                b.HasIndex(i => i.Name);

                b.Property(p => p.Id).ValueGeneratedNever();

                b.Property(p => p.Name).HasMaxLength(50);
                b.Property(p => p.Passwd).HasMaxLength(50);
                b.Property(p => p.CreatedTime).HasMaxLength(50);
            });
        }

    }
}
