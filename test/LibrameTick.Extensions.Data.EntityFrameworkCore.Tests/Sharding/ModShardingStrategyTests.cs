using Xunit;

namespace Librame.Extensions.Data.Sharding
{
    public class ModShardingStrategyTests
    {

        [Fact]
        public void AllTest()
        {
            var strategry = new ModShardingStrategy();

            foreach (var key in strategry.AllKeys)
            {
                var mod = strategry.Format(key, new SingleShardingValue<long>(() => 10));
                Assert.True(mod.IsDigit());
            }
        }

    }
}
