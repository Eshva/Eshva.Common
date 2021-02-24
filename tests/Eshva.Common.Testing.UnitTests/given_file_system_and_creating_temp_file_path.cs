#region Usings

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using FluentAssertions;
using RandomStringCreator;
using Xunit;

#endregion

namespace Eshva.Common.Testing.UnitTests
{
  public class given_file_system_and_creating_temp_file_path
  {
    [Fact]
    public void when_arguments_are_valid_file_path_should_be_placed_in_system_temporary_folder()
    {
      var expectedFileName = StringCreator.Get(length: 10);
      var expectedExtension = StringCreator.Get(length: 5);
      FileSystem.CreateTempFilePath(expectedFileName, expectedExtension).Should()
        .Be(Path.Combine(Path.GetTempPath(), $"{expectedFileName}.{expectedExtension}"), "it is the expected path");
    }

    [Fact]
    [SuppressMessage("ReSharper", "AccessToModifiedClosure")]
    public void when_arguments_are_invalid_it_should_fail()
    {
      var fileName = StringCreator.Get(length: 5);
      var extension = StringCreator.Get(length: 5);
      Action sut = () => FileSystem.CreateTempFilePath(fileName, extension);

      fileName = null;
      sut.Should().Throw<ArgumentNullException>().Where(exception => exception.ParamName.Equals("fileName"), "null is not allowed");
      fileName = string.Empty;
      sut.Should().Throw<ArgumentNullException>().Where(exception => exception.ParamName.Equals("fileName"), "an empty string is not allowed");
      fileName = WhiteSpaceString;
      sut.Should().Throw<ArgumentNullException>().Where(exception => exception.ParamName.Equals("fileName"), "a whitespace string is not allowed");

      fileName = StringCreator.Get(length: 5);
      extension = null;
      sut.Should().Throw<ArgumentNullException>().Where(exception => exception.ParamName.Equals("extension"), "null is not allowed");
      extension = string.Empty;
      sut.Should().Throw<ArgumentNullException>().Where(exception => exception.ParamName.Equals("extension"), "an empty string is not allowed");
      extension = WhiteSpaceString;
      sut.Should().Throw<ArgumentNullException>().Where(exception => exception.ParamName.Equals("extension"), "a whitespace string is not allowed");
    }

    private const string WhiteSpaceString = " \t\n\r";
    private static readonly StringCreator StringCreator = new StringCreator();
  }
}
