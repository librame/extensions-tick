#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Accessors
{
    /// <summary>
    /// 定义 <see cref="IAccessor"/> 种子机。
    /// </summary>
    public interface IAccessorSeeder
    {
        /// <summary>
        /// 标识生成器工厂。
        /// </summary>
        IIdentificationGeneratorFactory IdGeneratorFactory { get; }
    }
}
