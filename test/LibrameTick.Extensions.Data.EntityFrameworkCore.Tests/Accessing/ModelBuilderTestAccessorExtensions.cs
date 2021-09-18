using Microsoft.EntityFrameworkCore;

namespace Librame.Extensions.Data.Accessing
{
    static class ModelBuilderTestAccessorExtensions
    {

        public static ModelBuilder CreateUserModel(this ModelBuilder modelBuilder,
            ITestAccessor testAccessor)
        {
            modelBuilder.Entity<User>(b =>
            {
                b.ToTableWithSharding(testAccessor.ShardingManager);

                b.HasKey(k => k.Id);

                b.HasIndex(i => i.Name);

                b.Property(p => p.Id).ValueGeneratedNever();

                b.Property(p => p.Name).HasMaxLength(50);
                b.Property(p => p.Passwd).HasMaxLength(50);
                b.Property(p => p.CreatedTime).HasMaxLength(50);
            });

            return modelBuilder;
        }

    }
}
