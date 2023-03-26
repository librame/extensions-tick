#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data.Accessing;

namespace Librame.Extensions.Specifications;

/// <summary>
/// 定义一个实现 <see cref="AccessorSpec"/> 且支持写入与读写模式的数据访问存取器规约。
/// </summary>
public class WriteAccessorSpec : AccessorSpec
{
    /// <summary>
    /// 构造一个 <see cref="WriteAccessorSpec"/>。
    /// </summary>
    public WriteAccessorSpec(int group = 0)
        : base(group, AccessMode.Write | AccessMode.ReadWrite)
    {
    }

}
