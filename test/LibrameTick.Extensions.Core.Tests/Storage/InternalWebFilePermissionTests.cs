using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Core.Storage
{
    public class InternalWebFilePermissionTests
    {

        [Fact]
        public async void AllTest()
        {
            var permission = CoreExtensionBuilderHelper.CurrentServices.GetRequiredService<IWebFilePermission>();

            var accessToken = await permission.GetAccessTokenAsync().ConfigureAwait();
            Assert.NotEmpty(accessToken);

            var basicCode = await permission.GetBasicCodeAsync().ConfigureAwait();
            Assert.NotEmpty(basicCode);

            var bearerToken = await permission.GetBearerTokenAsync().ConfigureAwait();
            Assert.NotEmpty(bearerToken);

            var cookieValue = await permission.GetCookieValueAsync().ConfigureAwait();
            Assert.NotEmpty(cookieValue);
        }

    }
}
