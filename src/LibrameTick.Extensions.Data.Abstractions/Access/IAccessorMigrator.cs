#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Access
{
    /// <summary>
    /// 定义 <see cref="IAccessor"/> 移植器。
    /// </summary>
    public interface IAccessorMigrator
    {
        /// <summary>
        /// 迁移数据库。
        /// </summary>
        /// <param name="descriptors">给定的 <see cref="IReadOnlyList{AccessorDescriptor}"/>。</param>
        void Migrate(IReadOnlyList<AccessorDescriptor> descriptors);

        /// <summary>
        /// 异步迁移数据库。
        /// </summary>
        /// <param name="descriptors">给定的 <see cref="IReadOnlyList{AccessorDescriptor}"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个异步操作。</returns>
        Task MigrateAsync(IReadOnlyList<AccessorDescriptor> descriptors, CancellationToken cancellationToken = default);
    }
}
