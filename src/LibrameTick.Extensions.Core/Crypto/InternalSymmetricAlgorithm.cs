﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Core;

namespace Librame.Extensions.Crypto;

class InternalSymmetricAlgorithm : AbstractSymmetricAlgorithm
{
    public InternalSymmetricAlgorithm(IAlgorithmParameterGenerator parameterGenerator,
        IOptionsMonitor<CoreExtensionOptions> options)
        : base(parameterGenerator, options.CurrentValue.Algorithm)
    {
    }

}