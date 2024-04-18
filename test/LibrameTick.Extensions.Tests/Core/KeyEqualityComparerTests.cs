using System;
using System.Collections.Generic;
using Xunit;

namespace Librame.Extensions.Core
{
    public class KeyEqualityComparerTests
    {
        class TestComparer
        {
            public Guid Id { get; set; } = Guid.Empty;

            public int Count { get; set; } = 0;

            public string Name { get; set; } = string.Empty;
        }


        [Fact]
        public void AllTest()
        {
            var nameComparer = EqualityComparer<TestComparer>.Create((x, y) => x?.Name.Equals(y?.Name) == true, o => o.Name.GetHashCode());
            var countComparer = KeyEqualityComparer<TestComparer>.CreateBy(s => s.Count);

            var initial = new TestComparer();

            var compare = new TestComparer()
            {
                Name = nameof(TestComparer)
            };

            Assert.False(nameComparer.Equals(compare, initial));
            Assert.True(countComparer.Equals(compare, initial));
        }

    }
}
