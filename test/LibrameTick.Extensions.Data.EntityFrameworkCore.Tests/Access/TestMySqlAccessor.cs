using Librame.Extensions.Data.ValueConversion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Librame.Extensions.Data.Access
{
    public class TestMySqlAccessor : AbstractAccessor<TestMySqlAccessor>, ITestAccessor
    {

#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        public TestMySqlAccessor(DbContextOptions<TestMySqlAccessor> options)
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
            : base(options)
        {
        }


        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.CreateUserModel();

            var converterFactory = this.GetService<IEncryptionConverterFactory>();
            modelBuilder.UseEncryption(converterFactory, AccessorType);
        }

    }
}
