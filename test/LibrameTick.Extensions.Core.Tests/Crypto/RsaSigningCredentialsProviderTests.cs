using System.Security.Cryptography;
using Xunit;

namespace Librame.Extensions.Infrastructure
{
    public class RsaSigningCredentialsProviderTests
    {
        [Fact]
        public void AllTest()
        {
            var provider = new RsaSigningCredentialsProvider(new JsonFileRsaKeyProvider());
            var rsa = provider.LoadRsa();
            Assert.NotNull(rsa);

            var str = nameof(RsaSigningCredentialsProviderTests);

            var buffer = EncodingExtensions.UTF8Encoding.GetBytes(str);
            buffer = rsa.Encrypt(buffer, RSAEncryptionPadding.Pkcs1);
            buffer = rsa.Decrypt(buffer, RSAEncryptionPadding.Pkcs1);

            Assert.Equal(str, EncodingExtensions.UTF8Encoding.GetString(buffer));
        }

    }
}
