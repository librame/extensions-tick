using System.Linq;
using System.Security.Cryptography;
using Xunit;

namespace Librame.Extensions
{
    public class AlgorithmExtensionsTests
    {
        private readonly RandomNumberGenerator _generator
            = RandomNumberGenerator.Create();


        #region Base and Hex

        [Fact]
        public void Base32StringTest()
        {
            var bytes = new byte[32];
            _generator.GetBytes(bytes);

            var base32 = bytes.AsBase32String();
            Assert.True(bytes.SequenceEqual(base32.FromBase32String()));
        }

        [Fact]
        public void Base64StringTest()
        {
            var bytes = new byte[32];
            _generator.GetBytes(bytes);

            var base64 = bytes.AsBase64String();
            Assert.True(bytes.SequenceEqual(base64.FromBase64String()));
        }

        [Fact]
        public void HexStringTest()
        {
            var bytes = new byte[32];
            _generator.GetBytes(bytes);

            var hex = bytes.AsHexString();
            Assert.True(bytes.SequenceEqual(hex.FromHexString()));
        }

        #endregion


        #region Hash

        [Fact]
        public void AsHashBase64StringTest()
        {
            var sha256 = nameof(AlgorithmExtensionsTests).AsSHA256Base64String();
            Assert.NotEmpty(sha256);

            var sha384 = nameof(AlgorithmExtensionsTests).AsSHA384Base64String();
            Assert.True(sha384.Length > sha256.Length);

            var sha512 = nameof(AlgorithmExtensionsTests).AsSHA512Base64String();
            Assert.True(sha512.Length > sha384.Length);
        }

        #endregion


        #region HMAC Hash

        [Fact]
        public void AsHmacHashBase64StringTest()
        {
            var hmacSha256 = nameof(AlgorithmExtensionsTests).AsHMACSHA256Base64String();
            Assert.NotEmpty(hmacSha256);

            var hmacSha384 = nameof(AlgorithmExtensionsTests).AsHMACSHA384Base64String();
            Assert.True(hmacSha384.Length > hmacSha256.Length);

            var hmacSha512 = nameof(AlgorithmExtensionsTests).AsHMACSHA512Base64String();
            Assert.True(hmacSha512.Length > hmacSha384.Length);
        }

        #endregion


        #region AES

        [Fact]
        public void AsAesAndFromAesTest()
        {
            var str = nameof(AlgorithmExtensionsTests);
            (byte[] key, byte[] iv) = AlgorithmExtensions.GetAesKeyAndIV();

            var ciphertext = str.AsAesWithBase64String(key, iv);
            Assert.NotEmpty(ciphertext);

            var plaintext = ciphertext.FromAesWithBase64String(key, iv);
            Assert.Equal(str, plaintext);
        }

        #endregion

    }
}
