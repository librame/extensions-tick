using Xunit;

namespace Librame.Extensions.Infrastructure
{
    public class JsonFileRsaKeyProviderTests
    {
        [Fact]
        public void AllTest()
        {
            var provider = new JsonFileRsaKeyProvider();
            var rsaKey = provider.LoadOrSave();
            Assert.NotNull(rsaKey);

            Assert.True(provider.Exist());

            //provider.FilePath.FileDelete();
            //Assert.False(provider.Exist());
        }

    }
}
