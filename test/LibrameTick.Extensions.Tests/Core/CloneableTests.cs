using System;
using System.Drawing;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class ClonedInfo
    {
        private int _field1;

        internal int Field2;

        public int Property1 { get; set; }

        protected int Property2 { get; set; }

        public Rectangle Size { get; set; }

        public Version? Version { get; set; }


        public void SetField1(int value)
            => _field1 = value;

        public void SetField2(int value)
            => Field2 = value;

        public void SetProperty2(int value)
            => Property2 = value;


        public bool Field1Equals(ClonedInfo other)
            => _field1 == other._field1;

        public bool Property2Equals(ClonedInfo other)
            => Property2 == other.Property2;
    }


    public class CloneableTests
    {
        [Fact]
        public void AllTest()
        {
            var info = new ClonedInfo();
            info.SetField1(1);
            info.SetField2(2);
            info.SetProperty2(4);
            info.Size = new Rectangle(5, 5, 20, 10);
            info.Version = typeof(BaseCloneable<>).Assembly.GetName().Version;

            var cloneable = new BaseCloneable<ClonedInfo>(info);

            var info1 = (ClonedInfo)cloneable.Clone();
            Assert.True(info.Field1Equals(info1));
            Assert.True(info.Property2Equals(info1));
            Assert.Equal(info.Field2, info1.Field2);
            Assert.Equal(info.Property1, info1.Property1);

            var info2 = cloneable.CloneAs();
            Assert.True(info.Field1Equals(info2));
            Assert.True(info.Property2Equals(info2));
            Assert.Equal(info.Field2, info2.Field2);
            Assert.Equal(info.Property1, info2.Property1);
        }

    }

}
