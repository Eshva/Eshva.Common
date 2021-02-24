#region Usings

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using FluentAssertions;
using RandomStringCreator;
using Xunit;

#endregion

namespace Eshva.Common.Testing.UnitTests
{
  public class given_file_system_deleting_if_possible_temp_files_with_pattern
  {
    [Fact]
    public void when_arguments_are_valid_it_should_delete_files_by_pattern()
    {
      var prefix = StringCreator.Get(length: 10);
      var extension = StringCreator.Get(length: 5);

      const int numberOfFiles = 11;
      var filePaths = GenerateFilePaths(
        prefix,
        extension,
        numberOfFiles);
      CreateFilesInTempFolder(filePaths);

      var fileNamePattern = $"{prefix}-*.{extension}";
      FileSystem.DeleteIfPossibleTempFilesWithPattern(fileNamePattern);

      // IMPORTANT: Deleting of a file isn't synchronous operation.
      Thread.Sleep(TimeSpan.FromMilliseconds(value: 500));
      filePaths.ForEach(filePath => File.Exists(filePath).Should().BeFalse("file {0} should be already deleted", filePath));
    }

    [Fact]
    public void when_some_files_are_blocked_it_should_not_delete_them_and_should_not_produce_any_errors()
    {
      var prefix = StringCreator.Get(length: 10);
      var extension = StringCreator.Get(length: 5);

      const int numberOfFiles = 11;
      var filePaths = GenerateFilePaths(
        prefix,
        extension,
        numberOfFiles);
      CreateFilesInTempFolder(filePaths);

      var blockedFilePath = filePaths.First();
      var blockedFileStream = File.Open(
        blockedFilePath,
        FileMode.Open,
        FileAccess.ReadWrite,
        FileShare.None);

      var fileNamePattern = $"{prefix}-*.{extension}";
      Action sut = () => FileSystem.DeleteIfPossibleTempFilesWithPattern(fileNamePattern);
      sut.Should().NotThrow("all errors for file deletions should be ignored");

      // IMPORTANT: Deleting of a file isn't synchronous operation.
      Thread.Sleep(TimeSpan.FromMilliseconds(value: 500));
      filePaths.Skip(count: 1).ToList()
        .ForEach(filePath => File.Exists(filePath).Should().BeFalse("file {0} should be already deleted", filePath));
      File.Exists(blockedFilePath).Should().BeTrue("blocked file should remain of disk");

      blockedFileStream.Dispose();
      File.Delete(blockedFilePath);
    }

    [Fact]
    [SuppressMessage("ReSharper", "AccessToModifiedClosure")]
    public void when_arguments_are_invalid_it_should_fail()
    {
      var fileNamePattern = StringCreator.Get(length: 10);
      Action sut = () => FileSystem.DeleteIfPossibleTempFilesWithPattern(fileNamePattern);

      fileNamePattern = null;
      sut.Should().Throw<ArgumentNullException>().Where(
        exception => exception.ParamName.Equals("fileNamePattern"),
        "null is not allowed");
      fileNamePattern = string.Empty;
      sut.Should().Throw<ArgumentNullException>().Where(
        exception => exception.ParamName.Equals("fileNamePattern"),
        "an empty string is not allowed");
      fileNamePattern = WhiteSpaceString;
      sut.Should().Throw<ArgumentNullException>().Where(
        exception => exception.ParamName.Equals("fileNamePattern"),
        "a whitespace string is not allowed");
    }

    private static void CreateFilesInTempFolder(IEnumerable<string> filePaths)
    {
      foreach (var filePath in filePaths)
      {
        File.Exists(filePath).Should().BeFalse($"file '{filePath}' should not exist");
        File.Create(filePath).Dispose();
        File.Exists(filePath).Should().BeTrue($"file '{filePath}' should be created");
      }
    }

    private static List<string> GenerateFilePaths(
      string prefix,
      string extension,
      int numberOfFiles) =>
      Enumerable.Range(start: 0, numberOfFiles)
        .Select(_ => Path.Combine(Path.GetTempPath(), $"{prefix}-{StringCreator.Get(length: 10)}.{extension}"))
        .ToList();

    private const string WhiteSpaceString = " \t\n\r";
    private static readonly StringCreator StringCreator = new StringCreator();
  }
}
