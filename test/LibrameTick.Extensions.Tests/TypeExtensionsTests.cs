using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Xunit;

namespace Librame.Extensions.Tests
{
    public class TypeExtensionsTests
    {

        [Fact]
        public void GetBaseTypesTest()
        {
            var baseTypes = typeof(HMACMD5).GetBaseTypes();
            Assert.NotEmpty(baseTypes);
        }


        #region InvokeTypes and ExportedTypes

        [Fact]
        public void InvokeTypesTest()
        {
            var count = typeof(TypeExtensionsTests).Assembly.InvokeTypes(type =>
            {
                Assert.NotNull(type);
            });
            Assert.True(count > 0);
        }

        #endregion


        #region IsAssignableType

        [Fact]
        public void IsAssignableFromTargetTypeTest()
        {
            var baseType = typeof(IList<string>);
            var testType = typeof(List<string>);

            Assert.True(baseType.IsAssignableFromTargetType(testType));
            Assert.True(testType.IsAssignableFromTargetType(baseType));

            baseType = typeof(KeyedHashAlgorithm);
            testType = typeof(HMACMD5);

            Assert.True(testType.IsAssignableToBaseType(baseType));
        }

        #endregion


        #region IsImplementedType

        [Fact]
        public void IsImplementedInterfaceTypeTest()
        {
            var listType = typeof(List<string>);

            Assert.True(listType.IsImplementedInterfaceType<IList<string>>());
            Assert.True(typeof(IList<string>).IsImplementedInterfaceType<IEnumerable<string>>());

            Assert.True(listType.IsImplementedInterfaceType(typeof(ICollection<>), out Type? resultType));
            Assert.True(resultType?.GetGenericArguments().Single() == typeof(string));
        }

        [Fact]
        public void IsImplementedBaseTypeTest()
        {
            var type = typeof(MD5);

            Assert.True(type.IsImplementedBaseType<HashAlgorithm>());
            Assert.ThrowsAny<NotSupportedException>(() =>
            {
                type.IsImplementedBaseType<ICryptoTransform>();
            });

            Assert.True(type.IsImplementedBaseType<HashAlgorithm>(out Type? resultType));
            Assert.Equal(typeof(HashAlgorithm), resultType);
        }

        #endregion


        #region IsType

        [Fact]
        public void IsConcreteTypeTest()
        {
            Assert.True(typeof(HMACMD5).IsConcreteType());
            Assert.False(typeof(MD5).IsConcreteType());
            Assert.False(typeof(ICryptoTransform).IsConcreteType());
        }

        [Fact]
        public void IsNullableTypeTest()
        {
            Assert.True(typeof(bool?).IsNullableType());
            Assert.False(typeof(bool).IsNullableType());
        }

        #endregion

    }
}
