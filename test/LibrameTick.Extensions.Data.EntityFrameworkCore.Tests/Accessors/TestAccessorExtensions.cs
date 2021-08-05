using Microsoft.EntityFrameworkCore;

namespace Librame.Extensions.Data.Accessors
{
    static class TestAccessorExtensions
    {

        public static ModelBuilder CreateUserModel(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(b =>
            {
                b.ToTableByPluralize();

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
