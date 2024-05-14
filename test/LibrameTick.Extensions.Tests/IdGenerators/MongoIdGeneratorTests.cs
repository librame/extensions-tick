using System;
using System.Collections.Generic;
using Xunit;

namespace Librame.Extensions.IdGeneration
{
    public class MongoIdGeneratorTests
    {
        [Fact]
        public void ParseTest()
        {
            var generator = new MongoIdGenerator(new(), new());

            var testId = generator.GenerateId();

            var ticks1 = generator.ParseTicks(testId);
            var lastTicks = generator.GetLastTicks();

            Assert.Equal(ticks1, lastTicks);
        }

        [Fact]
        public void AllTest()
        {
            var generator = new MongoIdGenerator(new(), new());

            var capacity = 1000000; // 835ms
            var ids = new HashSet<string>(capacity);

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
