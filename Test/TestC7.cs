using FluentAssertions;
using Raven.Bundles.Quotas.Documents.Triggers;
using Xunit;

namespace Test
{
    public class TestC7
    {
        [Fact]
        public void Tuple_should_work()
        {
            var names = LookupName(3);
            names.first.Should().Be("Jeremy");
            names.last.Should().Be("Holt");
        }

        (string first, string last) LookupName(int id)
        {
            var first = "Jeremy";
            var last = "Holt";

            return (first, last);
        }
        
    }
}