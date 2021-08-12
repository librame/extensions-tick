using Librame.Extensions.Data;
using Xunit;

namespace Librame.Extensions.Collections
{
    public class TreeingListTests
    {
        class TestTreeing : AbstractParentIdentifier<int>
        {
            public string? Name { get; set; }
        }


        [Fact]
        public void AllTest()
        {
            var list = new List<TestTreeing>();

            for (var i = 0; i < 99; i++)
            {
                var test = new TestTreeing
                {
                    Id = i + 1,
                    Name = nameof(TreeingListTests) + (i + 1)
                };

                if (i < 10) test.ParentId = 0;
                if (i >= 10 && i < 50) test.ParentId = 3;
                if (i >= 50 && i < 99) test.ParentId = 7;

                list.Add(test);
            }

            var treeing = list.AsTreeing<TestTreeing, int>();
            Assert.Equal(10, treeing.Count);
            Assert.Equal(40, treeing.FindNode(3)!.Children.Count);
            Assert.Equal(49, treeing.FindNode(7)!.Children.Count);
        }

    }
}
