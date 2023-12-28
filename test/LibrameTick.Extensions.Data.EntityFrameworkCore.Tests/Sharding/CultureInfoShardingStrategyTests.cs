using System.Globalization;
using Xunit;

namespace Librame.Extensions.Data.Sharding
{
    public class CultureInfoShardingStrategyTests
    {

        [Fact]
        public void AllTest()
        {
            var strategry = new CultureInfoShardingStrategy(() => CultureInfo.CurrentCulture);

            foreach (var key in strategry.AllKeys)
            {
                var name = strategry.Format(key, shardingValue: null); // Use Default CultureInfo
                Assert.NotEqual(key, name);
            }
        }

    }
}
