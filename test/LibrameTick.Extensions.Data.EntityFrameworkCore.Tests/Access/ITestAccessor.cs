using Microsoft.EntityFrameworkCore;

namespace Librame.Extensions.Data.Access
{
    public interface ITestAccessor : IAccessor
    {
        DbSet<User> Users { get; set; }
    }
}
