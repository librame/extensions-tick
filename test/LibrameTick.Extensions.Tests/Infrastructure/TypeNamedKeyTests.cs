using Xunit;

namespace Librame.Extensions.Infrastructure
{
    public class TypeNamedKeyTests
    {

        [Fact]
        public void AllTest()
        {
            var type = typeof(TypeNamedKey);

            var original = new TypeNamedKey(type);
            var compare = original with { };

            Assert.Equal(original, compare);
            Assert.Equal(original.GetHashCode(), compare.GetHashCode());
            Assert.Equal(original.ToString(), compare.ToString());
        }

    }
}
