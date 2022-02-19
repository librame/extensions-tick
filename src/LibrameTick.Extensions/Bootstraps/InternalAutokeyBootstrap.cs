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

namespace Librame.Extensions.Bootstraps;

class InternalAutokeyBootstrap : AbstsractBootstrap, IAutokeyBootstrap
{
    public string? AutokeyFilePath
        => Autokey.GetDefaultFilePath();


    public Autokey GetAutokey()
    {
        if (string.IsNullOrEmpty(AutokeyFilePath) || !File.Exists(AutokeyFilePath))
            return GetDefaultAutokey();

        return Autokey.ReadJsonFile(AutokeyFilePath);
    }


    private Autokey GetDefaultAutokey()
    {
        return new Autokey
        {
            Id = Convert.FromBase64String("5JZ1OolMn7VkmvnDYExRwg=="),
            HmacMd5Key = Convert.FromBase64String("hl9V6MZP0Bw="),
            HmacSha256Key = Convert.FromBase64String("bXO8uwG8ysg="),
            HmacSha384Key = Convert.FromBase64String("s1Q5eN6jd+XK93rGCYce/Q=="),
            HmacSha512Key = Convert.FromBase64String("oRrbMJzkE1/3Rt7AGPeBQw=="),
            AesKey = Convert.FromBase64String("ojtZ/C8xb09aZaWZp03PaEq9XHBqosAxXEumFYuptHg="),
            AesIV = Convert.FromBase64String("5OS2V4WXI56HgYZAptOs7w==")
        };
    }

}
