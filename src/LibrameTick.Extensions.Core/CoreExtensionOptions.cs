#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Text;
using System.Text.Json.Serialization;

namespace Librame.Extensions.Core
{
    using Cryptography;
    using Serialization;

    /// <summary>
    /// 核心扩展选项。
    /// </summary>
    public class CoreExtensionOptions : AbstractExtensionOptions<CoreExtensionOptions>
    {
        /// <summary>
        /// 构造一个 <see cref="CoreExtensionOptions"/>。
        /// </summary>
        public CoreExtensionOptions()
            : base(parent: null)
        {
            Encoding = Encoding.UTF8;

            Algorithms = new AlgorithmOptions(this);

            ServiceCharacteristics.AddSingleton<IAlgorithmParameterGenerator>();
            ServiceCharacteristics.AddSingleton<ISymmetricAlgorithm>();
        }


        /// <summary>
        /// 字符编码。
        /// </summary>
        [JsonConverter(typeof(EncodingJsonConverter))]
        public Encoding Encoding { get; set; }


        /// <summary>
        /// 算法选项。
        /// </summary>
        public AlgorithmOptions Algorithms { get; private set; }
    }
}
