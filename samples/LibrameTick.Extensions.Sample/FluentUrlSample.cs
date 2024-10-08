using Librame.Extensions.Infrastructure;

namespace LibrameTick.Extensions.Sample
{
    internal class FluentUrlSample
    {
        private static readonly int _failRetries = 3;
        private static readonly TimeSpan _failRetryInterval = TimeSpan.FromSeconds(1);
        private static readonly string _testUrl = "http://www.contoso.com/index.htm?id=1#main";


        public static void Run()
        {
            Console.WriteLine(nameof(Run));

            var url = _testUrl
                .SetAbsoluteUrl()
                .SetHandleFail((ex, retries, interval) =>
                {
                    Console.WriteLine($"Error: {ex.Message}, CurrentRetries: {retries}, CurrentInterval: {interval}");
                });

            url.Handle(uri => throw new ArgumentException("Test error."), _failRetries, _failRetryInterval);
        }

        public static async Task RunAsync()
        {
            Console.WriteLine(nameof(RunAsync));

            var url = _testUrl
                .SetAbsoluteUrl()
                .SetHandleFail((ex, retries, interval) =>
                {
                    Console.WriteLine($"Error: {ex.Message}, CurrentRetries: {retries}, CurrentInterval: {interval}");
                });

            await url.HandleAsync(uri => throw new ArgumentException("Test error."), _failRetries, _failRetryInterval);
        }

    }
}
