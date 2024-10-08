#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Cryptography.Internal;

internal sealed class AlgorithmDependency(AlgorithmEngine engine) : IAlgorithmDependency
{
    public AlgorithmEngine Engine => engine;


    public void Dispose()
        => Engine.Dispose();

}
