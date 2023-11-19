using Microsoft.EntityFrameworkCore;

namespace Librame.Extensions.Data.Accessing
{
    public class TestMySqlDbContext : TestDataContext<TestMySqlDbContext>
    {
        public TestMySqlDbContext(DbContextOptions<TestMySqlDbContext> options)
            : base(options)
        {
        }

    }
}
