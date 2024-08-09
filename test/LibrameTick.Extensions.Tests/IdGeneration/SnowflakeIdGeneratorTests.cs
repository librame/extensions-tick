using Librame.Extensions.Dependency;
using System;
using System.Collections.Generic;
using Xunit;

namespace Librame.Extensions.IdGeneration
{
    public class SnowflakeIdGeneratorTests
    {
        private IClockDependency _clocks;


        public SnowflakeIdGeneratorTests()
        {
            _clocks = DependencyRegistration.CurrentContext.Clocks;
        }


        [Fact]
        public void ParseTest()
        {
            var generator = new SnowflakeIdGenerator(new(), new(), _clocks);

            var testId = generator.GenerateId();

            var ticks1 = generator.ParseTicks(testId);
            var ticks2 = generator.ParseTicks(testId, isOther: true);

            var lastTicks = generator.GetLastTicks();

            Assert.Equal(ticks1, lastTicks);
            Assert.Equal(ticks2, lastTicks);
        }

        [Fact]
        public void AllTest()
        {
            var generator = new SnowflakeIdGenerator(new(), new(), _clocks);

            var capacity = 1000000; // 253ms
            var ids = new HashSet<long>(capacity);

            for (var i = 0; i < capacity; i++)
            {
                var id = generator.GenerateId();

                if (ids.Contains(id))
                    throw new ArgumentException($"The id '{id}' is repeat.");

                ids.Add(id);
            }

            Assert.Equal(capacity, ids.Count);
        }

    }
}
