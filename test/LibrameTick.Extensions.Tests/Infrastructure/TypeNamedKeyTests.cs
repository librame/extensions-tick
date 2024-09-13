using Xunit;

namespace Librame.Extensions.Infrastructure
{
    public class TypeNamedKeyTests
    {

        [Fact]
        public void AllTest()
        {
            var type = typeof(TypedNamedKey);

            var original = new TypedNamedKey(type);
            var compare = original with { };

            Assert.Equal(original, compare);
            Assert.Equal(original.GetHashCode(), compare.GetHashCode());
            Assert.Equal(original.ToString(), compare.ToString());
        }

    }
}
