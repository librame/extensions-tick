using System.ComponentModel.DataAnnotations.Schema;
using Librame.Extensions.Filtration;
using Xunit;

namespace Librame.Extensions.Microparts
{
    //public class AssemblyMicropartTests
    //{
    //    [Fact]
    //    public void AllTest()
    //    {
    //        var options = new AssemblyFiltrationOptions();
    //        //options.AssemblyLoadPath = PathExtensions.CurrentDirectory;

    //        // 设定筛选器
    //        var librameFiltering = new AssemblyFiltrationDescriptor("LibrameFiltering");

    //        // 包含 Librame
    //        librameFiltering.Filters.Add(new FiltrationRegex("Librame", FiltrationMode.Inclusive));
    //        // 排除 Tests
    //        librameFiltering.Filters.Add(new FiltrationRegex("Tests", FiltrationMode.Exclusive));

    //        options.FilteringDescriptors.Add(librameFiltering);

    //        var micropart = new AssemblyMicropart(options);
    //        var assemblies = micropart.Unwrap();
    //        Assert.NotEmpty(assemblies);

    //        var keyTypes = assemblies.ExportedConcreteTypes<TypeNamedKey>();
    //        Assert.NotEmpty(keyTypes);

    //        var attribTypes = assemblies.ExportedConcreteTypesByAttribute<NotMappedAttribute>();
    //        Assert.NotEmpty(attribTypes);
    //    }

    //}
}
