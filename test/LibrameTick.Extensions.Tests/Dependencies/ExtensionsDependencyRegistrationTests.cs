using Librame.Extensions.Infrastructure.Dependency;
using Xunit;

namespace Librame.Extensions.Infrastructure
{
    public class ExtensionsDependencyRegistrationTests
    {

        [Fact]
        public void CurrentDependencyTest()
        {
            var dependency = DependencyRegistration.CurrentContext;
            Assert.NotNull(dependency);

            //ExtensionsDependencyRegistration.DisableCharacteristic<IAlgorithmManager>();
            //Assert.Throws<InvalidOperationException>(() =>
            //{
            //    return dependency.LazyAlgorithmManager.Value;
            //});
        }

    }
}
