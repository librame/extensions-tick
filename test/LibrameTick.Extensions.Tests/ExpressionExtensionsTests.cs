using System;
using System.Linq.Expressions;
using System.Text;
using Xunit;

namespace Librame.Extensions
{
    public class ExpressionExtensionsTests
    {

        [Fact]
        public void PropertyExpressionTest()
        {
            var orignalName = nameof(Encoding.BodyName);
            var orignalPage = nameof(Encoding.CodePage);

            Expression<Func<Encoding, string>> expression = p => p.BodyName;

            var name = expression.AsPropertyName();
            Assert.Equal(orignalName, name);

            var value = EncodingExtensions.UTF8Encoding.AsPropertyValue(expression);
            Assert.Equal("utf-8", value);

            // p => p.BodyName
            var autoExpression = orignalName.AsPropertyExpression<Encoding, string>();
            Assert.Equal(expression.ToString(), autoExpression.ToString());

            // p => p.PropertyName > 3
            var greaterThanExpression = orignalPage.AsGreaterThanPropertyExpression<Encoding, int>(3);
            var greaterThanExpression1 = orignalPage.AsGreaterThanPropertyExpression<Encoding>(typeof(int), 3);
            Assert.Equal(greaterThanExpression.ToString(), greaterThanExpression1.ToString());

            // p => p.PropertyName >= 3
            var greaterThanOrEqualExpression = orignalPage.AsGreaterThanOrEqualPropertyExpression<Encoding, int>(3);
            var greaterThanOrEqualExpression1 = orignalPage.AsGreaterThanOrEqualPropertyExpression<Encoding>(typeof(int), 3);
            Assert.Equal(greaterThanOrEqualExpression.ToString(), greaterThanOrEqualExpression1.ToString());

            // p => p.PropertyName < 3
            var lessThanExpression = orignalPage.AsGreaterThanPropertyExpression<Encoding, int>(3);
            var lessThanExpression1 = orignalPage.AsGreaterThanPropertyExpression<Encoding>(typeof(int), 3);
            Assert.Equal(lessThanExpression.ToString(), lessThanExpression1.ToString());

            // p => p.PropertyName <= 3
            var lessThanOrEqualExpression = orignalPage.AsGreaterThanOrEqualPropertyExpression<Encoding, int>(3);
            var lessThanOrEqualExpression1 = orignalPage.AsGreaterThanOrEqualPropertyExpression<Encoding>(typeof(int), 3);
            Assert.Equal(lessThanOrEqualExpression.ToString(), lessThanOrEqualExpression1.ToString());

            // p => p.PropertyName != 3
            var notEqualExpression = orignalPage.AsNotEqualPropertyExpression<Encoding, int>(3);
            var notEqualExpression1 = orignalPage.AsNotEqualPropertyExpression<Encoding>(typeof(int), 3);
            Assert.Equal(notEqualExpression.ToString(), notEqualExpression1.ToString());
        }

    }
}
