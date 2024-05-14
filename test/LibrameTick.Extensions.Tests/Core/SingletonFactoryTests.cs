using System;
using System.Threading;
using Librame.Extensions.Infrastructure;
using Xunit;

namespace Librame.Extensions.Core
{
    public class SingletonFactoryTests
    {
        public class TestNow
        {
            private TestNow()
            {
            }

            public DateTime Now { get; set; } = DateTime.Now;

            public DateTime Now2 => DateTime.Now;
        }


        [Fact]
        public void AllTest()
        {
            var instance = SingletonFactory<TestNow>.Instance;

            var now = instance.Now;
            var now2 = instance.Now2;

            Thread.Sleep(100);
            Assert.Equal(now, instance.Now);
            Assert.NotEqual(now2, instance.Now2);

            Thread.Sleep(100);
            var instance2 = SingletonFactory<TestNow>.Instance;

            Assert.Equal(now, instance2.Now);
            Assert.NotEqual(now2, instance2.Now2);
            Assert.True(ReferenceEquals(instance, instance2));

            Thread.Sleep(100);
            instance2 = SingletonFactory<TestNow>.Instance;

            Assert.Equal(now, instance2.Now);
            Assert.NotEqual(now2, instance2.Now2);
            Assert.True(ReferenceEquals(instance, instance2));
        }

    }
}
