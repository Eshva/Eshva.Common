#region Usings

using System;
using System.IO;
using JetBrains.Annotations;
using RandomStringCreator;

#endregion

namespace Eshva.Common.Testing
{
  [UsedImplicitly]
  public class FileSystem
  {
    /// <summary>
    /// Creates file path of a file placed in the system's temporary folder.
    /// </summary>
    /// <param name="fileName">
    /// File name.
    /// </param>
    /// <param name="extension">
    /// File extension.
    /// </param>
    /// <returns>
    /// Full file path placed in the system's temporary folder.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// One of arguments is <c>null</c>, an empty or a whitespace string.
    /// </exception>
    [NotNull]
    public static string CreateTempFilePath([NotNull] string fileName, [NotNull] string extension)
    {
      if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentNullException(nameof(fileName));
      if (string.IsNullOrWhiteSpace(extension)) throw new ArgumentNullException(nameof(extension));

      return Path.Combine(Path.GetTempPath(), $"{fileName}.{extension}");
    }

    /// <summary>
    /// Generates a random file name.
    /// </summary>
    /// <param name="prefix">
    /// File name prefix.
    /// </param>
    /// <returns>
    /// A random file name.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="prefix" /> is <c>null</c>, an empty or a whitespace string.
    /// </exception>
    [NotNull]
    public static string GenerateRandomFileName([NotNull] string prefix)
    {
      if (string.IsNullOrWhiteSpace(prefix)) throw new ArgumentNullException(nameof(prefix));

      return $"{prefix}-{StringCreator.Get(length: 10)}";
    }

    /// <summary>
    /// Deletes files from the system's temporary folder by file name pattern. If it is not possible to delete file it will be
    /// skipped.
    /// </summary>
    /// <param name="fileNamePattern">
    /// File name pattern. You can use * and ?.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="fileNamePattern" /> is <c>null</c>, an empty or a whitespace string.
    /// </exception>
    public static void DeleteIfPossibleTempFilesWithPattern([NotNull] string fileNamePattern)
    {
      if (string.IsNullOrWhiteSpace(fileNamePattern)) throw new ArgumentNullException(nameof(fileNamePattern));

      var foundFiles = Directory.GetFiles(Path.GetTempPath(), fileNamePattern);
      foreach (var filePath in foundFiles)
      {
        try
        {
          File.Delete(filePath);
        }
        catch (Exception)
        {
          // Intentionally ignore any errors. We did our best.
        }
      }
    }

    private static readonly StringCreator StringCreator = new();
  }
}
