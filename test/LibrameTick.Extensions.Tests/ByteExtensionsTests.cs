﻿using System.Linq;
using Xunit;

namespace Librame.Extensions
{
    public class ByteExtensionsTests
    {
        private readonly byte[] _data = RandomExtensions.GenerateByteArray(32);


        [Fact]
        public void Base32StringTest()
        {
            var base32 = _data.AsBase32String();
            Assert.True(_data.SequenceEqual(base32.FromBase32String()));
        }

        [Fact]
        public void Base64StringTest()
        {
            var base64 = _data.AsBase64String();
            Assert.True(_data.SequenceEqual(base64.FromBase64String()));
        }

        [Fact]
        public void HexStringTest()
        {
            var hex = _data.AsHexString();
            Assert.True(_data.SequenceEqual(hex.FromHexString()));
        }

    }
}
