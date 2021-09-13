using Microsoft.EntityFrameworkCore;

namespace Librame.Extensions.Data.Accessing
{
    public interface ITestAccessor : IDataAccessor
    {
        DbSet<User> Users { get; set; }
    }
}
