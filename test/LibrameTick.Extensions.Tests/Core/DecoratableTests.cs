using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class DecoratedInfo
    {
        public int Value { get; set; }
            = 1;
    }

    public class DecoratedInfoExtension : DecoratedInfo
    {
        public DecoratedInfoExtension(IDecoratable<DecoratedInfo> decoratable)
        {
            Value += decoratable.Source.Value;
        }
    }

    public class Decoratable : AbstractDecoratable<DecoratedInfo>
    {
        public Decoratable(DecoratedInfo source)
            : base(source)
        {
            Source.Value += 3;
        }
    }


    public class DecoratableTests
    {
        [Fact]
        public void AllTest()
        {
            var info = new DecoratedInfo();
            Assert.Equal(1, info.Value);

            var decoratable = new Decoratable(info);
            Assert.Equal(4, decoratable.Source.Value);

            var infoExtension = new DecoratedInfoExtension(decoratable);
            Assert.Equal(5, infoExtension.Value);
        }

    }

}
