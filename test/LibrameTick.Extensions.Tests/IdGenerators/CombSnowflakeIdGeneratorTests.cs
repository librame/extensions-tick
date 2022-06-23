using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Librame.Extensions.IdGenerators
{
    public class CombSnowflakeIdGeneratorTests
    {
        [Fact]
        public void AllTest()
        {
            var generator = new CombSnowflakeIdGenerator(new());

            var capacity = 100; // 1000000 [187ms]
            var ids = new HashSet<Guid>(capacity);

            var startTime = DateTimeOffset.UtcNow;

            for (var i = 0; i < capacity; i++)
            {
                // 9ce00500-a49d-31f6-0000-000100000000
                var id = generator.GenerateId();

                if (ids.Contains(id))
                    throw new ArgumentException($"The id '{id}' is repeat.");

                ids.Add(id);
            }

            Assert.Equal(capacity, ids.Count);

            // ToDateTime
            var dateTime = generator.ToDateTime(ids.First());
            Assert.True(dateTime > startTime);
        }

    }

}
