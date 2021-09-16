using Librame.Extensions.Data.Sharding;
using Microsoft.EntityFrameworkCore;

namespace Librame.Extensions.Data.Accessing
{
    public class TestMySqlAccessor : AbstractDataAccessor<TestMySqlAccessor>, ITestAccessor
    {

#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

        public TestMySqlAccessor(DbContextOptions<TestMySqlAccessor> options)
            : base(options)
        {
        }

#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。


        public DbSet<User> Users { get; set; }


        protected override void OnDataModelCreating(ModelBuilder modelBuilder,
            IShardingManager shardingManager, DataExtensionOptions dataOptions)
        {
            base.OnDataModelCreating(modelBuilder, shardingManager, dataOptions);

            modelBuilder.CreateUserModel(shardingManager);
        }

    }
}
