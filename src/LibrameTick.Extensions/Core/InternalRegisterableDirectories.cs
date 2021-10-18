#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core;

class InternalRegisterableDirectories : IRegisterableDirectories
{
    public InternalRegisterableDirectories()
    {
        BaseDirectory = PathExtensions.CurrentDirectoryWithoutDevelopmentRelativeSubpath;
    }


    public string BaseDirectory { get; init; }


    public string ConfigDirectory
        => BaseDirectory.CombinePath("_configs");

    public string ReportDirectory
        => BaseDirectory.CombinePath("_reports");

    public string ResourceDirectory
        => BaseDirectory.CombinePath("_resources");
}
