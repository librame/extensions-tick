using Microsoft.EntityFrameworkCore;

namespace Librame.Extensions.Data.Accessing
{
    public interface ITestAccessor : IAccessor
    {
        DbSet<User> Users { get; set; }
    }
}
