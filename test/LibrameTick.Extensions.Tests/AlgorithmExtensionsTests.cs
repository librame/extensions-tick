using Xunit;

namespace Librame.Extensions
{
    public class AlgorithmExtensionsTests
    {

        #region Hash

        [Fact]
        public void AsHashBase64StringTest()
        {
            var sha256 = nameof(AlgorithmExtensionsTests).AsSha256Base64String();
            Assert.NotEmpty(sha256);

            var sha384 = nameof(AlgorithmExtensionsTests).AsSha384Base64String();
            Assert.True(sha384.Length > sha256.Length);

            var sha512 = nameof(AlgorithmExtensionsTests).AsSha512Base64String();
            Assert.True(sha512.Length > sha384.Length);
        }

        #endregion


        #region HMAC Hash

        [Fact]
        public void AsHmacHashBase64StringTest()
        {
            var hmacMd5 = nameof(AlgorithmExtensionsTests).AsHmacMd5Base64String();
            Assert.NotEmpty(hmacMd5);

            var hmacSha256 = nameof(AlgorithmExtensionsTests).AsHmacSha256Base64String();
            Assert.NotEmpty(hmacSha256);

            var hmacSha384 = nameof(AlgorithmExtensionsTests).AsHmacSha384Base64String();
            Assert.True(hmacSha384.Length > hmacSha256.Length);

            var hmacSha512 = nameof(AlgorithmExtensionsTests).AsHmacSha512Base64String();
            Assert.True(hmacSha512.Length > hmacSha384.Length);
        }

        #endregion


        #region AES

        [Fact]
        public void AsAesAndFromAesTest()
        {
            var str = nameof(AlgorithmExtensionsTests);

            var ciphertext = str.AsAesWithBase64String();
            Assert.NotEmpty(ciphertext);

            var plaintext = ciphertext.FromAesWithBase64String();
            Assert.Equal(str, plaintext);

            (byte[] key, byte[] iv) = AlgorithmExtensions.GetAesKeyAndIV();
            Assert.NotEmpty(key);
            Assert.NotEmpty(iv);
        }

        #endregion

    }
}
