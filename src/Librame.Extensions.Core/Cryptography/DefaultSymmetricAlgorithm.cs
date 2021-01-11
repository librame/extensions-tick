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
    class DefaultSymmetricAlgorithm : AbstractSymmetricAlgorithm
    {
        public DefaultSymmetricAlgorithm(IAlgorithmParameterGenerator parameterGenerator,
            CoreExtensionBuilder extensionBuilder)
            : base(parameterGenerator, extensionBuilder)
        {
        }

    }
}
