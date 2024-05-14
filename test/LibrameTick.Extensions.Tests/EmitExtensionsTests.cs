using Librame.Extensions.Infrastructure.Mapping;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Xunit;

namespace Librame.Extensions
{
    public class TestEmit
    {
        public string Name { get; set; } = nameof(TestEmit);
    }

    public class TestEmitDerived : TestEmit
    {
    }


    public class EmitExtensionsTests
    {
        private readonly Type _sourceType = typeof(TestEmit);

        private readonly ModuleBuilder _moduleBuilder;


        public EmitExtensionsTests()
        {
            var newAssemblyName = typeof(EmitExtensionsTests).Assembly.GetName().Name;
            newAssemblyName = $"{newAssemblyName}_Sharding_{DateTimeOffset.UtcNow.UtcTicks}";

            _moduleBuilder = newAssemblyName.DefineDynamicModule();
        }


        [Fact]
        public void CopyTypeTest()
        {
            var source = new TestEmit();

            var copyTypeNames = new List<string>
            {
                "TestEmit_01",
                "TestEmit_02"
            };

            foreach (var newTypeName in copyTypeNames)
            {
                var copyType = _moduleBuilder.CopyType(_sourceType, newTypeName);
                var copyValue = ObjectMapper.NewByMapAllPublicProperties(source, copyType);

                var copyName = copyType.GetProperty(nameof(TestEmit.Name))?.GetValue(copyValue);
                Assert.Equal(source.Name, copyName);
            }
        }

        [Fact]
        public void DeriveTypeTest()
        {
            var autoDeriveType = _moduleBuilder.DeriveType(_sourceType, "TestEmitDerived_01");
            var dfltDeriveType = typeof(TestEmitDerived);

            var props1 = autoDeriveType.GetProperties();
            var props2 = dfltDeriveType.GetProperties();

            Assert.Equal(props1.Length, props2.Length);
        }

    }
}
