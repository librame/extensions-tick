using Xunit;

namespace Librame.Extensions.Cryptography
{
    public class JsonFileAutokeyProviderTests
    {
        [Fact]
        public void AllTest()
        {
            var provider = new JsonFileAutokeyProvider();
            Assert.False(provider.Exist());

            //var autokey = provider.LoadOrSave();
            //Assert.NotNull(autokey);
            //Assert.NotEmpty(autokey.Id!);

            // Test AlgorithmExtensions.PopulateDefaultCki
            var cki = new CkiOptions().PopulateDefaultCki();
            Assert.NotNull(cki);
        }

    }
}
