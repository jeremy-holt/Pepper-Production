using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Test
{
    public class Class1
    {
        [Fact]
        public void Test1()
        {
            (1 == 1).Should().BeTrue();
        }

    /*    [Fact]
        public void TestAutofacRegistration()
        {
            var container = Configure();
        } */
    }
}
