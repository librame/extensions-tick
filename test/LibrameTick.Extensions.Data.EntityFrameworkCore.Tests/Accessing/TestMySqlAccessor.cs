using Microsoft.EntityFrameworkCore;

namespace Librame.Extensions.Data.Accessing
{
    public class TestMySqlAccessor : BaseTestAccessor<TestMySqlAccessor>
    {
        public TestMySqlAccessor(DbContextOptions<TestMySqlAccessor> options)
            : base(options)
        {
        }

    }
}
