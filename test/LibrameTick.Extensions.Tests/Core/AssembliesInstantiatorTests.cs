using System.ComponentModel.DataAnnotations.Schema;
using Xunit;

namespace Librame.Extensions.Core
{
    public class AssembliesInstantiatorTests
    {
        [Fact]
        public void AllTest()
        {
            var options = new AssembliesOptions();
            //options.AssemblyLoadPath = PathExtensions.CurrentDirectory;

            // 设定筛选器描述符
            var librameFiltering = new AssembliesFilteringDescriptor("LibrameFiltering");

            // 包含 Librame
            librameFiltering.Filters.Add(new FilteringRegex("Librame", FilteringMode.Inclusive));
            // 排除 Tests
            librameFiltering.Filters.Add(new FilteringRegex("Tests", FilteringMode.Exclusive));

            options.FilteringDescriptors.Add(librameFiltering);

            var instantiator = new AssembliesInstantiator(options);
            var assemblies = instantiator.Create();
            Assert.NotEmpty(assemblies);

            var keyTypes = assemblies.ExportedConcreteTypes<TypeNamedKey>();
            Assert.NotEmpty(keyTypes);

            var attribTypes = assemblies.ExportedConcreteTypesByAttribute<NotMappedAttribute>();
            Assert.NotEmpty(attribTypes);
        }

    }
}
