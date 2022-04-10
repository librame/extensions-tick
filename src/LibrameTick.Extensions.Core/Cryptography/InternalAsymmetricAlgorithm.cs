#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Core;

namespace Librame.Extensions.Cryptography;

class InternalAsymmetricAlgorithm : AbstractAsymmetricAlgorithm
{
    public InternalAsymmetricAlgorithm(IAlgorithmParameterGenerator parameterGenerator,
        IOptionsMonitor<CoreExtensionOptions> options)
        : base(parameterGenerator, options.CurrentValue.Algorithm)
    {
    }

}
