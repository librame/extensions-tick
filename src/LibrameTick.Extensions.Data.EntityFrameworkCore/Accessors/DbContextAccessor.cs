#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.EntityFrameworkCore;
using System;

namespace Librame.Extensions.Data.Accessors
{
    /// <summary>
    /// 定义 <see cref="DbContext"/> 的 <see cref="IAccessor"/>。
    /// </summary>
    public class DbContextAccessor : DbContext, IAccessor
    {
        /// <summary>
        /// 使用指定的数据库上下文选项构造一个 <see cref="DbContextAccessor"/> 实例。
        /// </summary>
        /// <remarks>
        /// 备注：如果需要注册多个 <see cref="DbContext"/> 扩展，参数必须使用泛型 <see cref="DbContextOptions{TContext}"/> 形式，
        /// 不能使用非泛型 <see cref="DbContextOptions"/> 形式，因为 <paramref name="options"/> 参数也会注册到容器中以供使用。
        /// </remarks>
        /// <param name="options">给定的 <see cref="DbContextOptions{DbContextAccessor}"/>。</param>
        public DbContextAccessor(DbContextOptions<DbContextAccessor> options)
            : base(options)
        {
        }


        /// <summary>
        /// 访问器类型。
        /// </summary>
        public virtual Type AccessorType
            => typeof(DbContextAccessor);


        #region ISortable

        /// <summary>
        /// 排序优先级（数值越小越优先）。
        /// </summary>
        public float Priority
            => 1;


        /// <summary>
        /// 与指定的 <see cref="ISortable"/> 比较大小。
        /// </summary>
        /// <param name="other">给定的 <see cref="ISortable"/>。</param>
        /// <returns>返回整数。</returns>
        public int CompareTo(ISortable? other)
            => Priority.CompareTo(other?.Priority ?? 0);

        #endregion

    }
}
