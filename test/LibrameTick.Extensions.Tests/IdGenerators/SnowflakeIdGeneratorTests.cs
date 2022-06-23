using System;
using System.Collections.Generic;
using Xunit;

namespace Librame.Extensions.IdGenerators
{
    public class SnowflakeIdGeneratorTests
    {
        [Fact]
        public void AllTest()
        {
            var generator = new SnowflakeIdGenerator(new(), new());

            var capacity = 100; // 1000000 [149ms]
            var ids = new HashSet<long>(capacity);

            for (var i = 0; i < capacity; i++)
            {
                // 1770184990360355840
                var id = generator.GenerateId();

                if (ids.Contains(id))
                    throw new ArgumentException($"The id '{id}' is repeat.");

                ids.Add(id);
            }

            Assert.Equal(capacity, ids.Count);
        }

    }
}
