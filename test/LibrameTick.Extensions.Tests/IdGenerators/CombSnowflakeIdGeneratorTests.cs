using System;
using System.Collections.Generic;
using Xunit;

namespace Librame.Extensions.IdGenerators
{
    public class CombSnowflakeIdGeneratorTests
    {
        [Fact]
        public void AllTest()
        {
            var generator = new CombSnowflakeIdGenerator(new());

            var capacity = 1000000; // 196ms
            var ids = new HashSet<Guid>(capacity);

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
