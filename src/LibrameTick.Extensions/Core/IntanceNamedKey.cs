#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 定义实例命名键。
    /// </summary>
    /// <param name="InstanceType">给定的实例类型。</param>
    /// <param name="InstanceName">给定的实例名称（可选）。</param>
    public record IntanceNamedKey(Type InstanceType, string? InstanceName = null)
    {
        
    }
}
