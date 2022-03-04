#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.IdGenerators;

/// <summary>
/// 定义 COMB 标识生成方式。
/// </summary>
public enum CombIdGeneration
{
    /// <summary>
    /// 作为二进制。适用于 Oracle 等数据库排序方式。
    /// </summary>
    AsBinary = 1,

    /// <summary>
    /// 作为字符串。适用于 MySQL、SQLite 等数据库排序方式。
    /// </summary>
    AsString = 2,

    /// <summary>
    /// 位于末尾。适用于 SQL Server 等数据库排序方式。
    /// </summary>
    AtEnd = 3
}
