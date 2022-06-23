using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Librame.Extensions.IdGenerators
{
    public class MongoIdGeneratorTests
    {
        [Fact]
        public void AllTest()
        {
            var generator = new MongoIdGenerator(new(), new());

            var capacity = 100; // 1000000 [921ms]
            var ids = new HashSet<string>(capacity);

            //var startTime = DateTimeOffset.UtcNow;

            for (var i = 0; i < capacity; i++)
            {
                // 629B69108628AB55040D4B85
                var id = generator.GenerateId();

                if (ids.Contains(id))
                    throw new ArgumentException($"The id '{id}' is repeat.");

                ids.Add(id);
            }

            Assert.Equal(capacity, ids.Count);

            // ToDateTime
            //var dateTime = generator.ToDateTime(ids.First());
            //Assert.True(dateTime > startTime);
        }

    }
}
