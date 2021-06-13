#region Usings

using System;
using RandomStringCreator;

#endregion

namespace Eshva.Common.Testing
{
  /// <summary>
  /// Random value generator.
  /// </summary>
  public static class Randomize
  {
    /// <summary>
    /// Gets a random string of specified length.
    /// </summary>
    /// <param name="length">
    /// The length of random string to generate.
    /// </param>
    /// <returns>
    /// A random string of the specified length.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The length of random string is 0.
    /// </exception>
    public static string String(uint length)
    {
      if (length == 0) throw new ArgumentOutOfRangeException(nameof(length), "The length of random string should be greater than 0.");

      return StringCreator.Get((int) length);
    }

    private static readonly StringCreator StringCreator = new StringCreator();
  }
}
