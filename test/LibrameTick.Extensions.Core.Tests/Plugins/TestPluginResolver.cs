﻿using System;

namespace Librame.Extensions.Plugins
{
    internal class TestPluginResolver : AbstractPluginResolver
    {
        // 使用默认的插件加载选项设置
        public TestPluginResolver()
            : base(new())
        {
        }

    }
}
