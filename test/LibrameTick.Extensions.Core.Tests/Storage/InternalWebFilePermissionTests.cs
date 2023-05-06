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

            var accessToken = await permission.GetAccessTokenAsync().DisableAwaitContext();
            Assert.NotNull(accessToken);

            var basicCode = await permission.GetBasicCodeAsync().DisableAwaitContext();
            Assert.NotNull(basicCode);

            var bearerToken = await permission.GetBearerTokenAsync().DisableAwaitContext();
            Assert.NotNull(bearerToken);

            var cookieValue = await permission.GetCookieValueAsync().DisableAwaitContext();
            Assert.NotNull(cookieValue);
        }

    }
}
