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

    /// <summary>
    /// Produces a random integer value in the range specified.
    /// </summary>
    /// <remarks>
    /// It tolerates switching the range start and finish values.
    /// </remarks>
    /// <param name="rangeStart">
    /// The range start. Inclusive.
    /// </param>
    /// <param name="rangeFinish">
    /// The range finish. Inclusive.
    /// </param>
    /// <returns>
    /// A random integer between <paramref name="rangeStart" /> and <paramref name="rangeFinish" />.
    /// </returns>
    public static int Integer(int rangeStart, int rangeFinish)
    {
      var fromValue = Math.Min(rangeStart, rangeFinish);
      var toValue = Math.Max(rangeStart, rangeFinish);
      return Random.Next(fromValue, toValue + 1);
    }

    private static readonly StringCreator StringCreator = new StringCreator();
    private static readonly Random Random = new Random();
  }
}
