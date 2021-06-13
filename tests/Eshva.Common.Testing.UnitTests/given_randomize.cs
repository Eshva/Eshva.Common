#region Usings

using System;
using FluentAssertions;
using Xunit;

#endregion

namespace Eshva.Common.Testing.UnitTests
{
  public class given_randomize
  {
    [Fact]
    public void when_get_random_string_it_should_produce_random_string_of_specified_length()
    {
      var value1 = Randomize.String(length: 10);
      var value2 = Randomize.String(length: 10);
      var value3 = Randomize.String(length: 10);

      value1.Should().NotBe(value2);
      value1.Should().NotBe(value3);
    }

    [Fact]
    public void when_get_random_string_with_zero_length_it_should_fail()
    {
      Action sut = () => Randomize.String(length: 0);
      sut.Should().ThrowExactly<ArgumentOutOfRangeException>();
    }
  }
}
