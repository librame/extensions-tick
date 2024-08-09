#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Storing;

/// <summary>
/// 定义实现 <see cref="IOptions"/> 的存储选项。
/// </summary>
public class StoreOptions : IOptions
{
    /// <summary>
    /// 启用分表（功能未实现，默认禁用）。
    /// </summary>
    public bool EnablingSharding { get; private set; } = true;

    /// <summary>
    /// 映射关系（默认启用）。
    /// </summary>
    public bool MapRelationship { get; set; } = true;

    /// <summary>
    /// 可限制的最大属性长度（默认为 250）。
    /// </summary>
    /// <remarks>
    /// 在 MySQL 中如果长度超出 255 会被转换为不能作为主键或唯一性约束 的 BLOB/TEXT 类型。
    /// </remarks>
    public int LimitableMaxLengthOfProperty { get; set; } = 250;
}
