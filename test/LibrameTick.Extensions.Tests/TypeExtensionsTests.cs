﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Xunit;

namespace Librame.Extensions
{
    public class TypeExtensionsTests
    {

        [Fact]
        public void GetFriendlyNameTest()
        {
            var type = typeof(Action<Dictionary<Guid, KeyValuePair<int, string>>>);

            var name = type.GetFriendlyName();
            Assert.NotEmpty(name);

            var assemblyName = type.GetAssemblyFriendlyName();
            Assert.NotEmpty(assemblyName);
        }

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
            Assert.True(typeof(bool?).IsNullableOrTypeDefinition());
            Assert.False(typeof(bool).IsNullableOrTypeDefinition());
        }


        [Fact]
        public void GetBaseTypesTest()
        {
            var baseTypes = typeof(HMACMD5).GetBaseTypes();
            Assert.NotEmpty(baseTypes);
        }


        #region IsAssignableType

        [Fact]
        public void IsAssignableFromTargetTypeTest()
        {
            var baseType = typeof(IList<string>);
            var testType = typeof(List<string>);

            Assert.True(baseType.IsAssignableFromTargetType(testType));
            Assert.True(testType.IsAssignableToBaseType(baseType));

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

            Assert.True(listType.IsImplementedType<IList<string>>());
            Assert.True(typeof(IList<string>).IsImplementedType<IEnumerable<string>>());

            Assert.True(listType.IsImplementedType(typeof(ICollection<>), out Type? resultType));
            Assert.True(resultType?.GetGenericArguments().Single() == typeof(string));
        }

        [Fact]
        public void IsImplementedBaseTypeTest()
        {
            var type = typeof(MD5);

            Assert.True(type.IsImplementedType<HashAlgorithm>());
            Assert.True(type.IsImplementedType<ICryptoTransform>());

            Assert.True(type.IsImplementedType<HashAlgorithm>(out Type? resultType));
            Assert.Equal(type, resultType);
        }

        #endregion

    }
}
