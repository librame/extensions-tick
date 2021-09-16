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
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// <see cref="IndexBuilder"/> 静态扩展。
    /// </summary>
    public static class IndexBuilderExtensions
    {
        /// <summary>
        /// 有指定数据库索引的名称（格式：EntityName+PropertyNames+Index）。
        /// </summary>
        /// <param name="indexBuilder">给定的 <see cref="IndexBuilder"/>。</param>
        /// <param name="nameFunc">给定的名称方法（可选）。</param>
        /// <returns>返回 <see cref="IndexBuilder"/>。</returns>
        public static IndexBuilder HasDatabaseName(this IndexBuilder indexBuilder,
            Func<string, string>? nameFunc = null)
        {
            var sb = new StringBuilder();

            // Prefix: EntityBodyName
            sb.Append(indexBuilder.Metadata.DeclaringEntityType.ClrType.Name);

            foreach (var property in indexBuilder.Metadata.Properties)
                sb.Append(property.Name);

            // Suffix: Index
            sb.Append("Index");

            var name = sb.ToString();
            return indexBuilder.HasDatabaseName(nameFunc?.Invoke(name) ?? name);
        }

    }
}
