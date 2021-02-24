#region Usings

using System;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Serilog.Sinks.InMemory;
using SimpleInjector;
using Xunit;
using Xunit.Abstractions;

#endregion

namespace Eshva.Common.Testing.UnitTests
{
  public class given_logging_and_adding_serilog_logging_to_simple_injector_container
  {
    public given_logging_and_adding_serilog_logging_to_simple_injector_container(ITestOutputHelper testOutput)
    {
      _testOutput = testOutput;
    }

    [Fact]
    public void when_arguments_are_valid_it_should_be_possible_to_get_logger_for_any_class_context()
    {
      var container = new Container().AddLogging(_testOutput);
      var loggerForString = container.GetInstance<ILogger<string>>();
      loggerForString.Should().BeOfType<Logger<string>>("logger should be created");
      const string expectedLogMessage = "logger for string log message";
      loggerForString.LogInformation(expectedLogMessage);
      InMemorySink.Instance.LogEvents.Select(log => log.RenderMessage()).Should().Contain(expectedLogMessage);
    }

    [Fact]
    public void when_arguments_are_invalid_it_should_fail()
    {
      var container = new Container();
      Action sut = () => container.AddLogging(testOutput: null);
      sut.Should().Throw<ArgumentNullException>().Where(exception => exception.ParamName.Equals("testOutput"));
    }

    private readonly ITestOutputHelper _testOutput;
  }
}
