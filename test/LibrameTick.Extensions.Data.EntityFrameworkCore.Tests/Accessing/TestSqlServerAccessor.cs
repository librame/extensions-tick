using Microsoft.EntityFrameworkCore;

namespace Librame.Extensions.Data.Accessing
{
    public class TestSqlServerAccessor : BaseTestAccessor<TestSqlServerAccessor>
    {

        public TestSqlServerAccessor(DbContextOptions<TestSqlServerAccessor> options)
            : base(options)
        {
        }

    }
}
