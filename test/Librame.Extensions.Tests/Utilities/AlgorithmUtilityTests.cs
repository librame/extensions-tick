using System.Text;
using Xunit;

namespace Librame.Extensions.Tests
{
    public class AlgorithmUtilityTests
    {

        [Fact]
        public void RunAesTest()
        {
            var encoding = Encoding.UTF8;
            var str = nameof(AlgorithmUtilityTests);

            var ciphertext = AlgorithmUtility.RunAes(aes =>
            {
                var plaintext = encoding.GetBytes(str);
                var transform = aes.CreateEncryptor();
                return transform.TransformFinalBlock(plaintext, 0, plaintext.Length);
            });

            Assert.NotEmpty(ciphertext);

            var orgin = AlgorithmUtility.RunAes(aes =>
            {
                var transform = aes.CreateDecryptor();
                var plaintext = transform.TransformFinalBlock(ciphertext, 0, ciphertext.Length);
                return encoding.GetString(plaintext);
            });

            Assert.Equal(str, orgin);
        }

    }
}
