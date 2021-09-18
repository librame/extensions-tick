#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core.Cryptography;

class InternalAsymmetricAlgorithm : AbstractAsymmetricAlgorithm
{
    private CoreExtensionOptions _options;


    public InternalAsymmetricAlgorithm(IAlgorithmParameterGenerator parameterGenerator,
        CoreExtensionOptions options)
        : base(parameterGenerator, options)
    {
        _options = options;
    }


    protected override SigningCredentialsOptions DefaultRsaOptions
        => _options.Algorithm.Rsa;

}
