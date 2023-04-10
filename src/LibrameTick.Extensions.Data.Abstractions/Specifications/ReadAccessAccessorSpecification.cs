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
/// 定义一个实现 <see cref="AccessAccessorSpecification"/> 且支持读取与读写模式的数据访问存取器规约。
/// </summary>
public class ReadAccessAccessorSpecification : AccessAccessorSpecification
{
    /// <summary>
    /// 构造一个 <see cref="ReadAccessAccessorSpecification"/>。
    /// </summary>
    public ReadAccessAccessorSpecification(int group = 0)
        : base(group, AccessMode.Read | AccessMode.ReadWrite)
    {
    }

}
