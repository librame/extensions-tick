using System;

namespace Librame.Extensions.Plugins
{
    class TestPluginResource : AbstractPluginResource
    {
        public TestPluginResource()
            : base("TestPlugin")
        {
        }


        public string Message
        {
            get => GetString(nameof(Message));
            set => Add(nameof(Message), value);
        }

    }
}
