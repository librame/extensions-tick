#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core.Cryptography
{
    class InternalSymmetricAlgorithm : AbstractSymmetricAlgorithm
    {
        private CoreExtensionOptions _options;


        public InternalSymmetricAlgorithm(IAlgorithmParameterGenerator parameterGenerator,
            CoreExtensionOptions options)
            : base(parameterGenerator, options)
        {
            _options = options;
        }


        protected override KeyNonceOptions DefaultAesOptions
            => _options.Algorithms.Aes;

        protected override KeyNonceTagOptions DefaultAesCcmOptions
            => _options.Algorithms.AesCcm;

        protected override KeyNonceTagOptions DefaultAesGcmOptions
            => _options.Algorithms.AesGcm;

    }
}
