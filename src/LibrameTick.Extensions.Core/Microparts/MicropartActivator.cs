#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Infrastructure;

namespace Librame.Extensions.Microparts;

///// <summary>
///// 定义一个创建微构件的工厂接口。
///// </summary>
//public static class MicropartActivator
//{

//    /// <summary>
//    /// 创建程序集加载微构件实例。
//    /// </summary>
//    /// <param name="options">给定的 <see cref="AssemblyOptions"/>。</param>
//    /// <returns>返回 <see cref="AssemblyMicropart"/>。</returns>
//    public static AssemblyMicropart CreateAssembly(AssemblyOptions options)
//        => CreateInstance<AssemblyMicropart, AssemblyOptions>(options);

//    /// <summary>
//    /// 创建 <see cref="HttpClient"/> 微构件实例。
//    /// </summary>
//    /// <param name="options">给定的 <see cref="HttpClientOptions"/>。</param>
//    /// <returns>返回 <see cref="HttpClientMicropart"/>。</returns>
//    public static HttpClientMicropart CreateHttpClient(HttpClientOptions options)
//        => CreateInstance<HttpClientMicropart, HttpClientOptions>(options);


//    /// <summary>
//    /// 创建微构件实例。
//    /// </summary>
//    /// <typeparam name="TMicropart">指定的微构件类型。</typeparam>
//    /// <typeparam name="TOptions">指定的微构件选项类型。</typeparam>
//    /// <param name="options">给定的 <typeparamref name="TOptions"/>。</param>
//    /// <returns>返回 <typeparamref name="TMicropart"/>。</returns>
//    public static TMicropart CreateInstance<TMicropart, TOptions>(TOptions options)
//        where TOptions : IOptions
//        => (TMicropart)typeof(TMicropart).NewByExpression(options);

//}