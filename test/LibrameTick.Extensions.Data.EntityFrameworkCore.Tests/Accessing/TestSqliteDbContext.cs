using Microsoft.EntityFrameworkCore;

namespace Librame.Extensions.Data.Accessing
{
    public class TestSqliteDbContext : TestDataContext<TestSqliteDbContext>
    {
        public TestSqliteDbContext(DbContextOptions<TestSqliteDbContext> options)
            : base(options)
        {
        }

    }
}
