using Xunit;

namespace Librame.Extensions.Data
{
    public class TableDescriptorTests
    {

        [Fact]
        public void AllTest()
        {
            var now = DateTime.Now;

            var table = new TableDescriptor("Tables", "dbo")
                .AppendShardingNamedSuffix(now.Year.ToString());

            var expected = $"{table.Schema}{table.Separator}{table.ShardingName}";
            Assert.Equal(expected, table);

            var copyTable = TableDescriptor.Parse(expected);
            Assert.Equal(expected, copyTable);

            var newTable = table.WithShardingNamedSuffix(s => now.Month.ToString());
            expected = $"{table}{table.ShardingName.NamedConnector}{now.Month}";
            Assert.Equal(expected, newTable);
        }

    }
}
