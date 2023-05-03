#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Crypto;

namespace Librame.Extensions.Bootstraps;

class InternalAutokeyBootstrap : AbstsractBootstrap, IAutokeyBootstrap
{
    public IAutokeyProvider Provider
        => new JsonFileAutokeyProvider();


    public Autokey Get()
    {
        // 如果不存在 JSON 文件格式的自动密钥
        if (!Provider.Exist())
            return Autokey.Fixed(); // 则直接使用固定的自动密钥

        // 支持加载独立配置的自动密钥
        return Provider.Load();
        //return Provider.LoadOrSaveAutokey();
    }

}
