#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

//namespace Librame.Extensions.Core;

///// <summary>
///// 定义实现 <see cref="IExtensionBuilder"/> 并自动注册当前扩展构建器实例与选项配置的基础扩展构建器。
///// </summary>
///// <typeparam name="TBuilder">指定的扩展构建器类型。</typeparam>
///// <typeparam name="TOptions">指定的扩展选项类型。</typeparam>
//public class BaseExtensionBuilder<TBuilder, TOptions> : AbstractExtensionBuilder<TBuilder>
//    where TBuilder : IExtensionBuilder
//    where TOptions : class, IExtensionOptions
//{
//    /// <summary>
//    /// 构造一个父级 <see cref="BaseExtensionBuilder{TBuilder, TOptions}"/>。
//    /// </summary>
//    /// <exception cref="ArgumentNullException">
//    /// <paramref name="services"/> 为空。
//    /// </exception>
//    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
//    /// <param name="setupOptions">给定用于设置选项的动作（可选；为空则不设置）。</param>
//    /// <param name="configOptions">给定使用 <see cref="IConfiguration"/> 的选项配置（可选；为空则不配置）。</param>
//    public BaseExtensionBuilder(IServiceCollection services,
//        Action<TOptions>? setupOptions = null, IConfiguration? configOptions = null)
//        : base(services)
//    {
//        if (configOptions is not null)
//            Services.Configure<TOptions>(configOptions);

//        if (setupOptions is not null)
//            Services.Configure(setupOptions);
//    }

//    /// <summary>
//    /// 构造一个子级 <see cref="BaseExtensionBuilder{TBuilder, TOptions}"/>。
//    /// </summary>
//    /// <exception cref="ArgumentNullException">
//    /// <paramref name="parentBuilder"/> 为空。
//    /// </exception>
//    /// <param name="parentBuilder">给定的父级 <see cref="IExtensionBuilder"/>。</param>
//    /// <param name="setupOptions">给定用于设置选项的动作（可选；为空则不设置）。</param>
//    /// <param name="configOptions">给定使用 <see cref="IConfiguration"/> 的选项配置（可选；为空则不配置）。</param>
//    public BaseExtensionBuilder(IExtensionBuilder parentBuilder,
//        Action<TOptions>? setupOptions = null, IConfiguration? configOptions = null)
//        : base(parentBuilder)
//    {
//    }


//    /// <summary>
//    /// 扩展选项类型。
//    /// </summary>
//    public override Type ExtensionOptionsType
//        => typeof(TOptions);


//    ///// <summary>
//    ///// 将扩展选项保存为 JSON 文件。
//    ///// </summary>
//    ///// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
//    ///// <returns>返回保存路径。</returns>
//    //public override string SaveOptionsAsJson(IServiceProvider services)
//    //    => SaveOptionsAsJson(services, out _);

//    ///// <summary>
//    ///// 将扩展选项保存为 JSON 文件。
//    ///// </summary>
//    ///// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
//    ///// <param name="options">输出 <typeparamref name="TOptions"/>。</param>
//    ///// <returns>返回保存路径。</returns>
//    //public virtual string SaveOptionsAsJson(IServiceProvider services, out TOptions options)
//    //{
//    //    options = services.GetRequiredService<IOptions<TOptions>>().Value;
//    //    return options.ExportAsJson();
//    //}

//    ///// <summary>
//    ///// 将扩展选项保存为 JSON 文件。
//    ///// </summary>
//    ///// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
//    ///// <param name="options">输出 <see cref="IExtensionOptions"/>。</param>
//    ///// <returns>返回保存路径。</returns>
//    //public override string SaveOptionsAsJson(IServiceProvider services, out IExtensionOptions options)
//    //{
//    //    options = services.GetRequiredService<IOptions<TOptions>>().Value;
//    //    return options.ExportAsJson();
//    //}

//}
