using Librame.Extensions.Cryptography;
using Xunit;

namespace Librame.Extensions
{
    public class AlgorithmExtensionsTests
    {
        private const string _plaintext = nameof(AlgorithmExtensionsTests);


        [Fact]
        public void FormatStringTest()
        {
            // Encoding
            var buffer = _plaintext.FromEncodingString();
            Assert.Equal(_plaintext, buffer.AsEncodingString());

            // Base16
            var hex = buffer.AsHexString();
            Assert.Equal(buffer, hex.FromHexString());

            // Base32
            var base32 = buffer.AsBase32String();
            Assert.Equal(buffer, base32.FromBase32String());

            // Base64
            var base64 = buffer.AsBase64String();
            Assert.Equal(buffer, base64.FromBase64String());

            // Base64 Sortable
            var base64Sort = base64.AsSortableBase64String();
            Assert.Equal(base64, base64Sort.FromSortableBase64String());

            // Image Base64
            var imgBase64 = buffer.AsImageBase64String("image/jpeg");
            Assert.Equal(buffer, imgBase64.FromImageBase64String().bytes);
        }


        #region Hash

        [Fact]
        public void HashBase64StringTest()
        {
            var md5 = _plaintext.AsMd5Base64String();
            Assert.NotEmpty(md5);

            var sha1 = _plaintext.AsSha1Base64String();
            Assert.NotEmpty(sha1);

            var sha256 = _plaintext.AsSha256Base64String();
            Assert.NotEmpty(sha256);

            var sha384 = _plaintext.AsSha384Base64String();
            Assert.True(sha384.Length > sha256.Length);

            var sha512 = _plaintext.AsSha512Base64String();
            Assert.True(sha512.Length > sha384.Length);
        }

        #endregion


        #region HMAC Hash

        [Fact]
        public void HmacHashBase64StringTest()
        {
            var hmacMd5 = _plaintext.AsHmacMd5Base64String();
            Assert.NotEmpty(hmacMd5);

            var hmacSha1 = _plaintext.AsHmacSha1Base64String();
            Assert.NotEmpty(hmacSha1);

            var hmacSha256 = _plaintext.AsHmacSha256Base64String();
            Assert.NotEmpty(hmacSha256);

            var hmacSha384 = _plaintext.AsHmacSha384Base64String();
            Assert.True(hmacSha384.Length > hmacSha256.Length);

            var hmacSha512 = _plaintext.AsHmacSha512Base64String();
            Assert.True(hmacSha512.Length > hmacSha384.Length);
        }

        #endregion


        #region DES

        [Fact]
        public void DesTest()
        {
            var ciphertext = _plaintext.As3DesWithBase64String();
            Assert.NotEmpty(ciphertext);

            var plaintext = ciphertext.From3DesWithBase64String();
            Assert.Equal(_plaintext, plaintext);
        }

        #endregion


        #region AES

        [Fact]
        public void AesTest()
        {
            var ciphertext = _plaintext.AsAesWithBase64String();
            Assert.NotEmpty(ciphertext);

            var plaintext = ciphertext.FromAesWithBase64String();
            Assert.Equal(_plaintext, plaintext);
        }

        #endregion


        #region AES-CCM

        [Fact]
        public void AesCcmTest()
        {
            var ciphertext = _plaintext.AsAesCcmWithBase64String();
            Assert.NotEmpty(ciphertext);

            var plaintext = ciphertext.FromAesCcmWithBase64String();
            Assert.Equal(_plaintext, plaintext);
        }

        #endregion


        #region AES-GCM

        [Fact]
        public void AesGcmTest()
        {
            var ciphertext = _plaintext.AsAesGcmWithBase64String();
            Assert.NotEmpty(ciphertext);

            var plaintext = ciphertext.FromAesGcmWithBase64String();
            Assert.Equal(_plaintext, plaintext);
        }

        #endregion


        #region RSA

        [Fact]
        public void RsaTest()
        {
            // Encrypt & Decrypt Bytes
            var buffer = 256.GenerateByteArray();

            var pubEncrypt = buffer.AsPublicRsa();
            var pvtDecrypt = pubEncrypt.FromPrivateRsa();
            Assert.Equal(buffer, pvtDecrypt);

            // Encrypt & Decrypt
            var ciphertext = _plaintext.AsPublicRsaWithBase64String();
            Assert.NotEmpty(ciphertext);

            var plaintext = ciphertext.FromPrivateRsaWithBase64String();
            Assert.Equal(_plaintext, plaintext);

            // Sign
            var sig = _plaintext.SignDataPrivateRsaWithBase64String();
            var result = sig.VerifyDataPublicRsaWithBase64String(_plaintext);
            Assert.True(result);
        }

        #endregion


        #region ECDSA

        [Fact]
        public void EcdsaTest()
        {
            var ciphertext = _plaintext.SignDataPrivateEcdsaWithBase64String();
            Assert.NotEmpty(ciphertext);
            
            var result = ciphertext.VerifyDataPublicEcdsaWithBase64String(_plaintext);
            Assert.True(result);
        }

        #endregion


        #region Password

        [Fact]
        public void PasswordTest()
        {
            var ciphertext = _plaintext.AsPassword();
            Assert.NotEmpty(ciphertext);

            var result = ciphertext.VerifyPassword(_plaintext);
            Assert.True(result);
        }

        #endregion

    }
}
