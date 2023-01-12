#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Accessing;

/// <summary>
/// 定义实现 <see cref="BaseAccessorInitializer{TAccessor, TSeeder}"/> 的基础数据库上下文存取器初始化器。
/// </summary>
/// <typeparam name="TDbContext">指定实现 <see cref="BaseDbContext"/> 的数据库上下文存取器类型。</typeparam>
/// <typeparam name="TSeeder">指定实现 <see cref="AbstractAccessorSeeder"/> 的存取器种子机类型。</typeparam>
public class BaseDbContextAccessorInitializer<TDbContext, TSeeder> : BaseAccessorInitializer<BaseAccessor<TDbContext>, TSeeder>
    where TDbContext : BaseDbContext
    where TSeeder : AbstractAccessorSeeder
{
    /// <summary>
    /// 构造一个 <see cref="BaseDbContextAccessorInitializer{TDbContext, TSeeder}"/>。
    /// </summary>
    /// <param name="accessor">给定的 <see cref="BaseAccessor{TDbContext}"/>。</param>
    /// <param name="seeder">给定的 <typeparamref name="TSeeder"/>。</param>
    public BaseDbContextAccessorInitializer(BaseAccessor<TDbContext> accessor, TSeeder seeder)
        : base(accessor, seeder)
    {
    }

}
