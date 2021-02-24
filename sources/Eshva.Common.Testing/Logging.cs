#region Usings

using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.InMemory;
using SimpleInjector;
using Xunit.Abstractions;

#endregion

namespace Eshva.Common.Testing
{
  public static class Logging
  {
    public static Container AddLogging(this Container container, [NotNull] ITestOutputHelper testOutput)
    {
      if (testOutput == null) throw new ArgumentNullException(nameof(testOutput));

      container.RegisterInstance(GetLoggerFactory(testOutput));
      container.Register(
        typeof(ILogger<>),
        typeof(Logger<>),
        Lifestyle.Singleton);
      return container;
    }

    private static ILoggerFactory GetLoggerFactory(ITestOutputHelper testOutput) =>
      new LoggerFactory().AddSerilog(
        new LoggerConfiguration()
          .WriteTo.InMemory()
          .WriteTo.TestOutput(testOutput)
          .MinimumLevel.Verbose()
          .CreateLogger());
  }
}
