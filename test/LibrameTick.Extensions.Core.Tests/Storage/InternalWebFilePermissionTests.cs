using Librame.Extensions.Core;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Storage
{
    public class InternalWebFilePermissionTests
    {

        [Fact]
        public async void AllTest()
        {
            var permission = CoreExtensionBuilderHelper.CurrentServices.GetRequiredService<IWebFilePermission>();

            var accessToken = await permission.GetAccessTokenAsync().DiscontinueCapturedContext();
            Assert.NotNull(accessToken);

            var basicCode = await permission.GetBasicCodeAsync().DiscontinueCapturedContext();
            Assert.NotNull(basicCode);

            var bearerToken = await permission.GetBearerTokenAsync().DiscontinueCapturedContext();
            Assert.NotNull(bearerToken);

            var cookieValue = await permission.GetCookieValueAsync().DiscontinueCapturedContext();
            Assert.NotNull(cookieValue);
        }

    }
}
