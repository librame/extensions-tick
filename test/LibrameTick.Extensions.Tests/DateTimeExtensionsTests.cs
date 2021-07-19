#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using Xunit;

namespace Librame.Extensions
{
    public class DateTimeExtensionsTests
    {

        [Fact]
        public void ToUnixTicksTest()
        {
            var unixTicks = DateTimeOffset.UtcNow.ToUnixTicks();
            Assert.True(unixTicks > 0);
        }


        [Fact]
        public void AsWeekOfYearTest()
        {
            var weekOfYear = DateTime.Now.AsWeekOfYear();
            Assert.True(weekOfYear > 0 && weekOfYear < 54);
        }

        [Fact]
        public void AsQuarterOfYearTest()
        {
            var quarterOfYear = DateTime.Now.AsQuarterOfYear();
            Assert.True(quarterOfYear > 0 && quarterOfYear < 5);
        }

    }
}
