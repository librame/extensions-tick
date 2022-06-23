using Xunit;

namespace Librame.Extensions.Core
{
    public interface ITestInterceptable
    {
        int Count { get; set; }

        string GetName(string name);
    }

    public class TestInterceptable : ITestInterceptable
    {
        public TestInterceptable()
            => Count = 1;

        public int Count { get; set; }

        public string GetName(string name)
            => $"Hello {name}";
    }


    public class InterceptableTests
    {
        [Fact]
        public void AllTest()
        {
            var countDescriptor = InterceptionDescriptor<ITestInterceptable>.InterceptPropertyGetter(p => p.Count);
            countDescriptor.PreAction = (s, v) => s.As<ITestInterceptable>().Count += 1; // 执行后返回
            countDescriptor.PostAction = (s, v) => s.As<ITestInterceptable>().Count -= 1; // 返回后执行

            var nameDescriptor = InterceptionDescriptor.InterceptMethod(nameof(ITestInterceptable.GetName));
            nameDescriptor.ParameterTypes = new[] { typeof(string) };
            nameDescriptor.Parameters = new[] { "Stranger" }; // 限定拦截 Stranger
            nameDescriptor.PostAction = (s, v) => v.InvokeValue += ", Good Luck!";

            var source = new TestInterceptable();
            var interceptor = Interceptor.CreateInterface<ITestInterceptable>(source, intercept =>
            {
                intercept.AddOrUpdate(countDescriptor);
                intercept.AddOrUpdate(nameDescriptor);
            });

            var count = interceptor.Count; // 成功拦截 GET（获取Count属性不会执行PostAction）
            Assert.Equal(2, count);

            var name = interceptor.GetName("Stranger"); // 成功拦截 Stranger
            Assert.Equal("Hello Stranger, Good Luck!", name);

            name = interceptor.GetName("Friend"); // 未拦截 Friend
            Assert.Equal("Hello Friend", name);
        }

    }
}
