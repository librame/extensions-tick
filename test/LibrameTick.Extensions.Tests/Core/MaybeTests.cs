using System;
using Librame.Extensions.Infrastructure;
using Xunit;

namespace Librame.Extensions.Core
{
    public class MaybeTests
    {

        [Fact]
        public void AllTest()
        {
            var apple = Maybe.From("apple");
            var none = Maybe<string>.None;

            Assert.Equal("apple", apple);
            Assert.Throws<InvalidOperationException>(() => none.GetValueOrThrow());

            // ToList
            var apples = apple.ToList();
            Assert.Single(apples);

            // Bind
            var applesauce = apple.Bind(MakeApplesauce);
            Assert.Equal("applesauce", applesauce);

            // Map
            var fruit = apple.Map(v => v == "apple" ? "fruit" : string.Empty);
            Assert.Equal("fruit", fruit);

            // Where
            Func<string, bool> likeApple = v => v == "apple";
            Assert.Equal("apple", apple.Where(likeApple));
        }

        private Maybe<string> MakeApplesauce(Maybe<string> fruit)
        {
            if (fruit == "apple")
                return "applesauce";

            return Maybe<string>.None;
        }

    }

}
