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
            Assert.NotNull(accessToken);

            var basicCode = await permission.GetBasicCodeAsync().ConfigureAwait();
            Assert.NotNull(basicCode);

            var bearerToken = await permission.GetBearerTokenAsync().ConfigureAwait();
            Assert.NotNull(bearerToken);

            var cookieValue = await permission.GetCookieValueAsync().ConfigureAwait();
            Assert.NotNull(cookieValue);
        }

    }
}
