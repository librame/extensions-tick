using Librame.Extensions.Data;
using Xunit;

namespace Librame.Extensions.Collections
{
    public class PagingListTests
    {
        class TestPaging : AbstractIdentifier<int>
        {
            public string? Name { get; set; }
        }


        [Fact]
        public void AllTest()
        {
            var list = new List<TestPaging>();

            for (var i = 0; i < 99; i++)
            {
                list.Add(new TestPaging
                {
                    Id = i + 1,
                    Name = nameof(PagingListTests) + (i + 1)
                });
            }

            var paging = list.AsPaging(page => page.PageByIndex(index: 2, size: 20));
            Assert.Equal(20, paging.Info.Size);
            Assert.Equal(21, paging.First().Id);
        }

    }
}
