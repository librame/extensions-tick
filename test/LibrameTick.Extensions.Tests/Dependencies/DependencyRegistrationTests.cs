using Librame.Extensions.Cryptography;
using Librame.Extensions.Dependency;
using System;
using Xunit;

namespace Librame.Extensions.Infrastructure
{
    public class DependencyRegistrationTests
    {

        [Fact]
        public void CurrentContextTest()
        {
            var context = DependencyRegistration.CurrentContext;
            Assert.NotNull(context);

            Assert.Equal(context, DependencyRegistration.CurrentContext);
        }

        [Fact]
        public void CharacteristicTest()
        {
            var dependency = AlgorithmExtensions.AlgorithmDependency;
            Assert.NotNull(dependency.Value);

            DependencyRegistration.DisableCharacteristic<IAlgorithmDependency>();

            Assert.Throws<InvalidOperationException>(() =>
            {
                return dependency.Value;
            });
        }

    }
}
