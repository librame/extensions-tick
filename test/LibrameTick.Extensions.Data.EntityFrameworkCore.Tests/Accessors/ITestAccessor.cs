using Microsoft.EntityFrameworkCore;

namespace Librame.Extensions.Data.Accessors
{
    public interface ITestAccessor : IAccessor
    {
        /// <summary>
        /// 用户数据集。
        /// </summary>
        DbSet<User> Users { get; set; }
    }
}
