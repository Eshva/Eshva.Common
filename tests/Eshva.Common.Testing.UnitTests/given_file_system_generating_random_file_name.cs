#region Usings

using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using RandomStringCreator;
using Xunit;

#endregion

namespace Eshva.Common.Testing.UnitTests
{
  public class given_file_system_generating_random_file_name
  {
    [Fact]
    public void when_arguments_valid_it_should_generate_file_name_with_expected_format()
    {
      var prefix = StringCreator.Get(length: 10);
      var random1 = FileSystem.GenerateRandomFileName(prefix);
      var random2 = FileSystem.GenerateRandomFileName(prefix);
      random1.Should().NotBe(random2, "generated strings should be different");
      random1.Should().StartWith($"{prefix}-", "prefix should be placed in front of file name");
    }

    [Fact]
    [SuppressMessage("ReSharper", "AccessToModifiedClosure")]
    public void when_arguments_are_invalid_it_should_fail()
    {
      var prefix = StringCreator.Get(length: 10);
      Action sut = () => FileSystem.GenerateRandomFileName(prefix);
      prefix = null;
      sut.Should().Throw<ArgumentNullException>().Where(
        exception => exception.ParamName.Equals("prefix"),
        "null is not allowed");
      prefix = string.Empty;
      sut.Should().Throw<ArgumentNullException>().Where(
        exception => exception.ParamName.Equals("prefix"),
        "an empty string is not allowed");
      prefix = WhiteSpaceString;
      sut.Should().Throw<ArgumentNullException>().Where(
        exception => exception.ParamName.Equals("prefix"),
        "a whitespace string is not allowed");
    }

    private const string WhiteSpaceString = " \t\n\r";
    private static readonly StringCreator StringCreator = new StringCreator();
  }
}
