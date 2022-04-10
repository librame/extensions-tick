using Microsoft.EntityFrameworkCore;

namespace Librame.Extensions.Data.Accessing
{
    public class BaseTestAccessor<TAccessor> : DbContextAccessorWithAuditing<TAccessor>
        where TAccessor : DbContextAccessorWithAudit
    {
        public BaseTestAccessor(DbContextOptions<TAccessor> options)
            : base(options)
        {
        }


        protected override void OnModelCreatingCore(ModelBuilder modelBuilder)
        {
            base.OnModelCreatingCore(modelBuilder);

            modelBuilder.CreateUserModel(this);
        }

    }
}
