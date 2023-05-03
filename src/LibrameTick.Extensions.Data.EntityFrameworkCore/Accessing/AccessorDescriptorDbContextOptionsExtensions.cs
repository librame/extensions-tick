#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Accessing;

/// <summary>
/// <see cref="AccessorDescriptor"/> 静态扩展。
/// </summary>
public static class AccessorDescriptorDbContextOptionsExtensions
{

    /// <summary>
    /// 转为存取器描述符。
    /// </summary>
    /// <param name="extension">给定的 <see cref="AccessorDbContextOptionsExtension"/>。</param>
    /// <param name="accessor">给定的 <see cref="AbstractAccessor"/>。</param>
    /// <returns>返回 <see cref="AccessorDescriptor"/>。</returns>
    public static AccessorDescriptor ToDescriptor(this AccessorDbContextOptionsExtension extension,
        AbstractAccessor accessor)
    {
        // 默认使用存取器定义的优先级属性值
        var priority = extension.Priority < 0 ? accessor.Priority : extension.Priority;
        var algorithms = extension.Algorithm ?? accessor.CoreOptions.Algorithm;

        var name = string.IsNullOrEmpty(extension.Name)
            ? accessor.CurrentContext.GetType().Name.TrimEnd(nameof(DbContext))
            : extension.Name;

        return new AccessorDescriptor(accessor, extension.ServiceType!, name, extension.Group,
            extension.Access, extension.Dispatching, priority, algorithms, extension.Sharded,
            extension.LoaderHost);
    }

}
