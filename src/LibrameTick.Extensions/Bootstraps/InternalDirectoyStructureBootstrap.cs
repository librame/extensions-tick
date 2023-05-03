#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Bootstraps;

class InternalDirectoyStructureBootstrap : AbstsractBootstrap, IDirectoryStructureBootstrap
{
    public InternalDirectoyStructureBootstrap()
    {
        BaseDirectory = PathExtensions.CurrentDirectoryWithoutDevelopmentRelativeSubpath;
    }


    public string BaseDirectory { get; init; }


    public string ConfigDirectory
        => BaseDirectory.CombineDirectory("_configs");

    public string ReportDirectory
        => BaseDirectory.CombineDirectory("_reports");

    public string ResourceDirectory
        => BaseDirectory.CombineDirectory("_resources");
}
