using System;
using Xunit;

namespace Librame.Extensions
{
    public class DateTimeExtensionsTests
    {

        [Fact]
        public void BaseTest()
        {
            Assert.Equal(DateTimeExtensions.BaseTime, DateTimeExtensions.BaseTimeOffset.DateTime);
            Assert.Equal(DateTimeExtensions.BaseOffset, TimeSpan.Zero);
            Assert.Equal(DateTimeExtensions.LocalOffset, new TimeSpan(8, 0, 0));
        }

        [Fact]
        public void AsOffsetTest()
        {
            var now = DateTime.Now;
            var utcNow = now.AsOffset();
            Assert.Equal(now, utcNow.DateTime);

            now = DateTime.UtcNow;
            utcNow = now.AsOffset();
            Assert.Equal(now, utcNow.DateTime);
        }

        [Fact]
        public void AsDateTimeStringTest()
        {
            var now = DateTime.Now;
            Assert.Equal(now.ToString("yyyy-MM-dd HH:mm:ss"), now.AsDateTimeString());
            Assert.Equal(now.ToString("yyyy-MM-dd HH:mm:ss.fff"), now.AsDateTimeString(withMilliseconds: true));

            var utcNow = DateTimeOffset.UtcNow;
            Assert.Equal(utcNow.ToString("yyyy-MM-dd HH:mm:ss zzz"), utcNow.AsDateTimeString());
            Assert.Equal(utcNow.ToString("yyyy-MM-dd HH:mm:ss.fff zzz"), utcNow.AsDateTimeString(withMilliseconds: true));
        }

        [Fact]
        public void WithTest()
        {
            var newDay = 3;

            var now = DateTime.Now;
            Assert.Equal(newDay, now.With(newDay: newDay).Day);
            Assert.Equal(0, now.WithoutMillisecond().Microsecond);

            var utcNow = DateTimeOffset.UtcNow;
            Assert.Equal(newDay, utcNow.With(newDay: newDay).Day);
            Assert.Equal(0, utcNow.WithoutMillisecond().Microsecond);
        }

        [Fact]
        public void DateOfYearTest()
        {
            var weekOfYear = DateTime.Now.AsWeekOfYear();
            Assert.True(weekOfYear > 0 && weekOfYear < 54);

            var quarterOfYear = DateTime.Now.AsQuarterOfYear();
            Assert.True(quarterOfYear > 0 && quarterOfYear < 5);
        }

    }
}
