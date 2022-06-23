using System;
using System.Collections.Generic;
using Xunit;

namespace Librame.Extensions.IdGenerators
{
    public class CombIdGeneratorTests
    {
        [Fact]
        public void AllTest()
        {
            var generator = CombIdGenerators.ForSqlServer(new());

            var capacity = 100; // 1000000 [374ms]
            var ids = new HashSet<Guid>(capacity);

            for (var i = 0; i < capacity; i++)
            {
                // 1370a73b-a0c4-6a6b-049f-01812e2d479f
                var id = generator.GenerateId();

                if (ids.Contains(id))
                    throw new ArgumentException($"The id '{id}' is repeat.");

                ids.Add(id);
            }

            Assert.Equal(capacity, ids.Count);
        }

    }
}
