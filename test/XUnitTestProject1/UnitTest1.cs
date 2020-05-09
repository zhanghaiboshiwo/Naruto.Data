using System;
using Xunit;

namespace XUnitTestProject1
{
    public class UnitTest1
    {

        [Theory]
        [InlineData(0,1)]
        [InlineData(1, 2)]
        [InlineData(2, 3)]
        public void Test1(int i, int j)
        {

        }
    }
}
