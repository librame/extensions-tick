using Xunit;

namespace Librame.Extensions.Data
{
    public class DatabaseDescriptorTests
    {

        [Fact]
        public void AllTest()
        {
            var now = DateTime.Now;
            var defaultDatabase = $"{nameof(Librame)}_{nameof(Extensions)}";

            // Database: Librame.Extensions
            var database = new DatabaseDescriptor(defaultDatabase)
                .AppendShardingNamedSuffix(now.Year.ToString());

            var expected = $"{database.Key}{database.Separator}{database.ShardingName}";
            Assert.Equal(expected, database);

            var copyDatabase = DatabaseDescriptor.Parse(expected);
            Assert.Equal(expected, copyDatabase);

            var newDatabase = database.WithShardingNamedSuffix(s => now.Month.ToString());
            expected = $"{database}{database.ShardingName.NamedConnector}{now.Month}";
            Assert.Equal(expected, newDatabase);

            // SQLServer Connection String
            var connString = $"Server=.;Database={defaultDatabase};Integrated Security=True";
            var connDatabase = DatabaseDescriptor.ParseFromConnectionString(connString)
                .WithShardingNamedSuffix(s => now.Month.ToString());

            expected = $"Database={connDatabase.ShardingName}";
            Assert.Equal(expected, connDatabase);

            // SQLite Connection String
            connString = $"Data Source=/test/{defaultDatabase}.db";
            connDatabase = DatabaseDescriptor.ParseFromConnectionString(connString)
                .WithShardingNamedSuffix(s => now.Month.ToString());

            expected = $"Data Source={connDatabase.ShardingName}.db";
            Assert.Equal(expected, connDatabase);
        }

    }
}
