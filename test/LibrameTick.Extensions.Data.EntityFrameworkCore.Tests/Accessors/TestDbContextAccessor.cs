using Microsoft.EntityFrameworkCore;
using System;

namespace Librame.Extensions.Data.Accessors
{
    /// <summary>
    /// 定义 <see cref="DbContext"/> 数据访问器。
    /// </summary>
    public class TestDbContextAccessor : DbContext, IAccessor
    {
        /// <summary>
        /// 使用指定的数据库上下文选项构造一个 <see cref="TestDbContextAccessor"/> 实例。
        /// </summary>
        /// <remarks>
        /// 备注：如果需要注册多个 <see cref="DbContext"/> 扩展，参数必须使用泛型 <see cref="DbContextOptions{TContext}"/> 形式，
        /// 不能使用非泛型 <see cref="DbContextOptions"/> 形式，因为 <paramref name="options"/> 参数也会注册到容器中以供使用。
        /// </remarks>
        /// <param name="options">给定的 <see cref="DbContextOptions{TestDbContextAccessor}"/>。</param>
        public TestDbContextAccessor(DbContextOptions<TestDbContextAccessor> options)
            : base(options)
        {
        }


        /// <summary>
        /// 访问器类型。
        /// </summary>
        public virtual Type AccessorType
            => typeof(TestDbContextAccessor);


        #region ISortable

        public float Priority
            => 3;

        public int CompareTo(ISortable? other)
            => Priority.CompareTo(other?.Priority ?? 0);

        #endregion

    }
}
