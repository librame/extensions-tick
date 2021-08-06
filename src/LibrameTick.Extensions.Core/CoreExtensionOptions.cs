#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Core.Cryptography;
using Librame.Extensions.Core.Serialization;
using System.Text;
using System.Text.Json.Serialization;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 核心扩展选项。
    /// </summary>
    public class CoreExtensionOptions : AbstractExtensionOptions<CoreExtensionOptions>
    {
        /// <summary>
        /// 构造一个 <see cref="CoreExtensionOptions"/>。
        /// </summary>
        public CoreExtensionOptions()
            : base(parentOptions: null)
        {
            Encoding = Encoding.UTF8;
            Clock = LocalClock.Default;

            Algorithms = new AlgorithmOptions(this);
            AssemblyLoading = new AssemblyLoadingOptions();

            // Cryptography
            ServiceCharacteristics.AddSingleton<IAlgorithmParameterGenerator>();
            ServiceCharacteristics.AddSingleton<IAsymmetricAlgorithm>();
            ServiceCharacteristics.AddSingleton<ISymmetricAlgorithm>();
        }


        /// <summary>
        /// 字符编码。
        /// </summary>
        [JsonConverter(typeof(JsonStringEncodingConverter))]
        public Encoding Encoding { get; set; }
        
        /// <summary>
        /// 时钟。
        /// </summary>
        [JsonIgnore]
        public IClock Clock { get; set; }

        /// <summary>
        /// 机器中心标识。
        /// </summary>
        public long DataCenterId { get; set; }

        /// <summary>
        /// 机器标识。
        /// </summary>
        public long MachineId { get; set; }

        /// <summary>
        /// 启用 <see cref="IAutoloader"/> 激活器。
        /// </summary>
        public bool EnableAutoloaderActivator { get; set; }


        /// <summary>
        /// 算法选项。
        /// </summary>
        public AlgorithmOptions Algorithms { get; init; }

        /// <summary>
        /// 程序集加载选项。
        /// </summary>
        public AssemblyLoadingOptions AssemblyLoading { get; init; }
    }
}
