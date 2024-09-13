using System;
using Xunit;

namespace Librame.Extensions.Infrastructure
{
    public class FluentUrlTests
    {

        [Fact]
        public void BaseTest()
        {
            var testUrl = "http://user:pwd@www.contoso.com/index.htm?id=123#main";

            var url = testUrl.SetAbsoluteUrl();
            Assert.Equal("http", url.Scheme);
            Assert.Equal("user", url.UserName);
            Assert.Equal("pwd", url.Password);
            Assert.Equal("www.contoso.com", url.Host);
            Assert.Equal("/index.htm", url.Path);
            Assert.Equal("?id=123", url.QueryString);
            Assert.Equal("#main", url.Fragment);
            Assert.Equal("123", url.GetRequiredQueryValue("id"));

            var newUrl = url.Copy().EditQuery("id", "456").FlushQuery();
            Assert.Equal(newUrl.Scheme, url.Scheme);
            Assert.Equal(newUrl.UserName, url.UserName);
            Assert.Equal(newUrl.Password, url.Password);
            Assert.Equal(newUrl.Host, url.Host);
            Assert.Equal(newUrl.Path, url.Path);
            Assert.Equal(newUrl.Fragment, url.Fragment);
            Assert.Equal("?id=456", newUrl.QueryString);
            Assert.Equal("456", newUrl.GetRequiredQueryValue("id"));
        }

        [Fact]
        public void HandleFailTest()
        {
            var count = 0;
            var failRetries = 3;
            var failRetryInterval = TimeSpan.FromSeconds(1);

            var url = "http://www.contoso.com/index.htm?id=123#main"
                .SetAbsoluteUrl()
                .SetHandleFail((ex, retries, interval) => ++count);

            url.Handle(uri => throw new ArgumentException("test error."), failRetries, failRetryInterval);

            Assert.Equal(failRetries, count);
        }

    }
}
