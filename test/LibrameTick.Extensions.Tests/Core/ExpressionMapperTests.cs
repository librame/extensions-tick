using System;
using System.Drawing;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class CloneInfo
    {
        private int _field1;

        internal int Field2;

        public int Property1 { get; private set; }

        protected int Property2 { get; set; }

        public Rectangle Size { get; set; }

        public Version? Version { get; set; }

        public CloneInfo? Parent { get; set; }


        public void SetField1(int value)
            => _field1 = value;

        public void SetField2(int value)
            => Field2 = value;

        public void SetProperty1(int value)
            => Property1 = value;

        public void SetProperty2(int value)
            => Property2 = value;


        public bool Field1Equals(CloneInfo other)
            => _field1 == other._field1;

        public bool Property2Equals(CloneInfo other)
            => Property2 == other.Property2;
    }


    public class ExpressionMapperTests
    {
        [Fact]
        public void AllTest()
        {
            var info = new CloneInfo();
            info.SetField1(1);
            info.SetField2(2);
            info.SetProperty1(4);
            info.SetProperty2(8);
            info.Size = new Rectangle(5, 5, 20, 10);
            info.Version = typeof(CloneInfo).Assembly.GetName().Version;
            info.Parent = new CloneInfo();

            var mapPropsInfo = ExpressionMapper<CloneInfo, CloneInfo>.Map(info);
            Assert.Equal(info.Size, mapPropsInfo.Size);
            Assert.Equal(info.Version, mapPropsInfo.Version);
            Assert.NotEqual(info.Field2, mapPropsInfo.Field2);

            var mapAllFieldsInfo = AllExpressionMapper<CloneInfo, CloneInfo>.Map(info);
            Assert.Equal(info.Size, mapAllFieldsInfo.Size);
            Assert.Equal(info.Version, mapAllFieldsInfo.Version);
            Assert.Equal(info.Field2, mapAllFieldsInfo.Field2);
        }

        [Fact]
        public void BenchmarkTest()
        {
            var info = new CloneInfo();
            info.SetField1(1);
            info.SetField2(2);
            info.SetProperty2(4);
            info.Size = new Rectangle(5, 5, 20, 10);
            info.Version = typeof(CloneInfo).Assembly.GetName().Version;

            var ms = StopwatchExtensions.Run(s =>
            {
                for (int i = 0; i < 1000_000; i++)
                {
                    var _ = ExpressionMapper<CloneInfo, CloneInfo>.Map(info);
                }

                return s.ElapsedMilliseconds;
            });
            Assert.True(ms <= 100);

            var msAll = StopwatchExtensions.Run(s =>
            {
                for (int i = 0; i < 1000_000; i++)
                {
                    var _ = AllExpressionMapper<CloneInfo, CloneInfo>.Map(info);
                }

                return s.ElapsedMilliseconds;
            });
            Assert.True(msAll <= 100);
        }

    }
}
