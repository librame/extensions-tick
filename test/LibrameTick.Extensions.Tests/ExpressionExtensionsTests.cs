using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using Xunit;

namespace Librame.Extensions
{
    class TestGeneric
    {
        private static TValue GetValue<TValue>(TValue value)
            => value;

    }


    public class ExpressionExtensionsTests
    {
        private readonly Encoding _defaultEncoding = Encoding.UTF8;


        [Fact]
        public void PropertyTest()
        {
            var bodyName = nameof(Encoding.BodyName);

            Expression<Func<Encoding, string>> expression = p => p.BodyName;

            var propertyName = expression.AsPropertyName();
            Assert.Equal(bodyName, propertyName);

            var utf8Name = expression.Compile()(_defaultEncoding);
            Assert.Equal("utf-8", utf8Name);

            // p => p.BodyName
            var createExpression = bodyName.CreatePropertyExpression<Encoding>();
            Assert.Equal(expression.ToString(), createExpression.ToString());

            var decoder = nameof(Encoding.GetDecoder).GetMethodValueByExpression<Encoding, Decoder>(_defaultEncoding);
            Assert.NotNull(decoder);

            var maxByteCount = nameof(Encoding.GetMaxByteCount).GetMethodValueByExpression<Encoding, int, int>(_defaultEncoding, 3);
            Assert.NotEqual(0, maxByteCount);

            // 静态属性
            var utf8 = nameof(Encoding.UTF8).GetPropertyValueByExpression<Encoding, Encoding>(source: null);
            Assert.NotNull(utf8);

            var getValueFunc = "GetValue".GetMethodFuncByExpression<TestGeneric?, int, int>(genTypes => [typeof(int)]);
            var value = getValueFunc(default, 2);
            Assert.Equal(2, value);

            var directValue = "GetValue".GetMethodValueByExpression<TestGeneric?, int, int>(default, 2, genTypes => [typeof(int)]);
            Assert.Equal(directValue, value);
        }

        [Fact]
        public void CreatePropertyExpressionTest()
        {
            Expression<Func<Encoding, int>> expression = p => p.CodePage;

            var codePageName = nameof(Encoding.CodePage);

            var codePage = expression.Compile()(_defaultEncoding);
            var orginalCodePage = 65001;

            // p => p.CodePage > orginalCodePage
            var greaterThanExpression = codePageName.CreateGreaterThanPropertyExpression<Encoding>(orginalCodePage);
            Assert.False(greaterThanExpression.Compile()(_defaultEncoding));

            // p => p.CodePage >= orginalCodePage
            var greaterThanOrEqualExpression = codePageName.CreateGreaterThanOrEqualPropertyExpression<Encoding>(orginalCodePage);
            Assert.True(greaterThanOrEqualExpression.Compile()(_defaultEncoding));

            // p => p.CodePage < orginalCodePage
            var lessThanExpression = codePageName.CreateLessThanPropertyExpression<Encoding>(orginalCodePage);
            Assert.False(lessThanExpression.Compile()(_defaultEncoding));

            // p => p.CodePage <= orginalCodePage
            var lessThanOrEqualExpression = codePageName.CreateLessThanOrEqualPropertyExpression<Encoding>(orginalCodePage);
            Assert.True(lessThanOrEqualExpression.Compile()(_defaultEncoding));

            // p => p.CodePage != orginalCodePage
            var notEqualExpression = codePageName.CreateNotEqualPropertyExpression<Encoding>(orginalCodePage);
            Assert.False(notEqualExpression.Compile()(_defaultEncoding));

            // p => p.CodePage is orginalCodePage
            var equalExpression = codePageName.CreateEqualPropertyExpression<Encoding>(orginalCodePage);
            Assert.True(equalExpression.Compile()(_defaultEncoding));
        }

        [Fact]
        public void NewTest()
        {
            var list = ExpressionExtensions.New<List<string>>();
            Assert.NotNull(list);

            var fileName = @"D:\test.txt";
            var expressionFileInfo = ExpressionExtensions.New<FileInfo>(fileName);
            Assert.Equal(fileName, expressionFileInfo.FullName);
        }

    }
}
