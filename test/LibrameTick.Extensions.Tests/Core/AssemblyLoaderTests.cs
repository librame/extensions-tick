using Xunit;

namespace Librame.Extensions.Core
{
    public class AssemblyLoaderTests
    {
        [Fact]
        public void LoadAssembliesTest()
        {
            var options = new AssemblyLoadingOptions(nameof(AssemblyLoaderTests));
            //options.AssemblyLoadPath = PathExtensions.CurrentDirectory;

            // 设定筛选器描述符
            var librameFiltering = new AssemblyFilteringDescriptor("LibrameFiltering");

            // 包含 Librame
            librameFiltering.Filters.Add(new FilteringRegex("Librame", FilteringMode.Inclusive));
            // 排除 Tests
            librameFiltering.Filters.Add(new FilteringRegex("Tests", FilteringMode.Exclusive));

            options.FilteringDescriptors.Add(librameFiltering);

            var assemblies = AssemblyLoader.LoadAssemblies(options);
            Assert.NotEmpty(assemblies!);

            var keyTypes = AssemblyLoader.LoadConcreteTypes(typeof(TypeNamedKey), assemblies!);
            Assert.NotEmpty(keyTypes!);
        }

    }
}
