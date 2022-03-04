using Microsoft.EntityFrameworkCore;

namespace Librame.Extensions.Data.Accessing
{
    public class TestSqliteAccessor : BaseTestAccessor<TestSqliteAccessor>
    {

        public TestSqliteAccessor(DbContextOptions<TestSqliteAccessor> options)
            : base(options)
        {
        }

    }
}
