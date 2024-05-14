#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.IdGeneration;

/// <summary>
/// 定义 COMB 标识生成方式。
/// </summary>
public enum CombIdGeneration
{
    /// <summary>
    /// 作为二进制。适用于 Oracle 等数据库排序方式。当使用 Guid.ToByteArray() 方法进行格式化时连续。
    /// </summary>
    AsBinary = 1,

    /// <summary>
    /// 作为字符串。适用于 MySQL、PostgreSql、SQLite 等数据库排序方式。当使用 Guid.ToString() 方法进行格式化时连续。
    /// </summary>
    AsString = 2,

    /// <summary>
    /// 位于末尾。适用于 SQL Server 等数据库排序方式。连续性体现于 GUID 的第4块（Data4）。
    /// </summary>
    AtEnd = 3
}
