using Librame.Extensions.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Librame.Extensions
{
    public class TestEmit
    {
        public string Name { get; set; } = nameof(TestEmit);
    }


    public class EmitExtensionsTests
    {
        [Fact]
        public void BuildCopyFromTypesTest()
        {
            var sourceTypes = new Dictionary<Type, IEnumerable<string>>
            {
                { typeof(TestEmit), new List<string> { "TestEmit_01", "TestEmit_02" } }
            };

            var testEmit = new TestEmit();

            var newAssemblyName = typeof(EmitExtensionsTests).Assembly.GetName().Name;
            newAssemblyName = $"{newAssemblyName}_Sharding_{DateTimeOffset.UtcNow.UtcTicks}";

            var copyTypes = sourceTypes.BuildCopyFromTypes(newAssemblyName);
            for (var i = 0; i < copyTypes.Count; i++)
            {
                var copyType = copyTypes.ElementAt(i);
                var copyValue = ObjectMapper.NewByMapAllPublicProperties(testEmit, copyType);

                var copyName = copyType.GetProperty(nameof(TestEmit.Name))?.GetValue(copyValue);
                Assert.NotNull(copyName);
                Assert.Equal(testEmit.Name, copyName);
            }
        }

    }
}
