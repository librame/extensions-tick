using System.Threading;
using Xunit;

namespace Librame.Extensions.Core
{
    public class LazySingletonTests
    {

        [Fact]
        public void AllTest()
        {
            var instance = LazySingleton<SingletonFactoryTests.TestNow>.Instance;

            var now = instance.Now;
            var now2 = instance.Now2;

            Thread.Sleep(100);
            Assert.Equal(now, instance.Now);
            Assert.NotEqual(now2, instance.Now2);

            Thread.Sleep(100);
            var instance2 = LazySingleton<SingletonFactoryTests.TestNow>.Instance;

            Assert.Equal(now, instance2.Now);
            Assert.NotEqual(now2, instance2.Now2);
            Assert.True(ReferenceEquals(instance, instance2));

            Thread.Sleep(100);
            instance2 = LazySingleton<SingletonFactoryTests.TestNow>.Instance;

            Assert.Equal(now, instance2.Now);
            Assert.NotEqual(now2, instance2.Now2);
            Assert.True(ReferenceEquals(instance, instance2));
        }

    }
}
