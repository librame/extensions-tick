using Microsoft.EntityFrameworkCore;

namespace Librame.Extensions.Data.Accessing
{
    public class TestSqlServerDbContext : TestDataContext<TestSqlServerDbContext>
    {
        public TestSqlServerDbContext(DbContextOptions<TestSqlServerDbContext> options)
            : base(options)
        {
        }

    }
}
