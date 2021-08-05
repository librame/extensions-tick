#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// <see cref="EntityTypeBuilder"/> 静态扩展。
    /// </summary>
    public static class EntityTypeBuilderExtensions
    {
        // Microsoft.EntityFrameworkCore.Metadata.RelationalAnnotationNames
        private const string RelationalAnnotationTableName = "Relational:TableName";
        private const string RelationalAnnotationScheme = "Relational:Schema";


        /// <summary>
        /// 通过复数化实体类型名称映射表名（此方法仅支持关系型数据库）。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="builder">给定的 <see cref="EntityTypeBuilder{TEntity}"/>。</param>
        /// <returns>返回 <see cref="EntityTypeBuilder{TEntity}"/>。</returns>
        public static EntityTypeBuilder<TEntity> ToTableByPluralize<TEntity>(this EntityTypeBuilder<TEntity> builder)
            where TEntity : class
            => builder.ToTableByPluralize(schema: null);

        /// <summary>
        /// 通过复数化实体类型名称映射表名（此方法仅支持关系型数据库）。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="builder">给定的 <see cref="EntityTypeBuilder{TEntity}"/>。</param>
        /// <param name="schema">给定的架构。</param>
        /// <returns>返回 <see cref="EntityTypeBuilder{TEntity}"/>。</returns>
        public static EntityTypeBuilder<TEntity> ToTableByPluralize<TEntity>(this EntityTypeBuilder<TEntity> builder, string? schema)
            where TEntity : class
        {
            var names = typeof(TEntity).Name.AsPluralize();
            builder.Metadata.SetAnnotation(RelationalAnnotationTableName, names);

            if (!string.IsNullOrEmpty(schema))
                builder.Metadata.SetAnnotation(RelationalAnnotationScheme, schema);

            return builder;
        }

    }
}
