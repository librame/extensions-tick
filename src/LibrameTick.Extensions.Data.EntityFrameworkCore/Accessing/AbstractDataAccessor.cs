#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data.Sharding;
using Librame.Extensions.Data.Storing;
using Librame.Extensions.Data.ValueConversion;
using Microsoft.EntityFrameworkCore;

namespace Librame.Extensions.Data.Accessing
{
    /// <summary>
    /// 定义抽象 <see cref="AbstractDataAccessor"/> 的泛型实现。
    /// </summary>
    /// <typeparam name="TAccessor">指定实现 <see cref="AbstractDataAccessor"/> 的访问器类型。</typeparam>
    public abstract class AbstractDataAccessor<TAccessor> : AbstractDataAccessor
        where TAccessor : AbstractDataAccessor
    {
        /// <summary>
        /// 使用指定的数据库上下文选项构造一个 <see cref="AbstractDataAccessor{TAccessor}"/> 实例。
        /// </summary>
        /// <remarks>
        /// 备注：如果需要注册多个 <see cref="DbContext"/> 扩展，参数必须使用泛型 <see cref="DbContextOptions{TAccessor}"/> 形式，
        /// 不能使用非泛型 <see cref="DbContextOptions"/> 形式，因为 <paramref name="options"/> 参数也会注册到容器中以供使用。
        /// </remarks>
        /// <param name="options">给定的 <see cref="DbContextOptions{TAccessor}"/>。</param>
        protected AbstractDataAccessor(DbContextOptions<TAccessor> options)
            : base(options)
        {
        }


        /// <summary>
        /// 访问器类型。
        /// </summary>
        public override Type AccessorType
            => typeof(TAccessor);
    }


    /// <summary>
    /// 定义抽象 <see cref="AbstractAccessor"/> 与 <see cref="IDataAccessor"/> 的实现。
    /// </summary>
    public abstract class AbstractDataAccessor : AbstractAccessor, IDataAccessor
    {

#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

        /// <summary>
        /// 使用指定的数据库上下文选项构造一个 <see cref="AbstractDataAccessor"/> 实例。
        /// </summary>
        /// <param name="options">给定的 <see cref="DbContextOptions"/>。</param>
        protected AbstractDataAccessor(DbContextOptions options)
            : base(options)
        {
        }

#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。


        /// <summary>
        /// 审计数据集。
        /// </summary>
        public DbSet<Audit> Audits { get; set; }

        /// <summary>
        /// 表格数据集。
        /// </summary>
        public DbSet<Tabulation> Tabulations { get; set; }


        /// <summary>
        /// 开始模型创建。
        /// </summary>
        /// <param name="modelBuilder">给定的 <see cref="ModelBuilder"/>。</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var shardingManager = GetRequiredScopeService<IShardingManager>();
            OnDataModelCreating(modelBuilder, shardingManager);

            var converterFactory = GetRequiredScopeService<IEncryptionConverterFactory>();
            modelBuilder.UseEncryption(converterFactory, AccessorType);
        }

        /// <summary>
        /// 开始数据模型创建。
        /// </summary>
        /// <param name="modelBuilder">给定的 <see cref="ModelBuilder"/>。</param>
        /// <param name="shardingManager">给定的 <see cref="IShardingManager"/>。</param>
        protected virtual void OnDataModelCreating(ModelBuilder modelBuilder,
            IShardingManager shardingManager)
        {
            modelBuilder.CreateDataModel(shardingManager);
        }

    }
}
