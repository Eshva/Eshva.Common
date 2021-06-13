#region Usings

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

#endregion

namespace Eshva.Common.Tpl.UnitTests
{
  public class given_cancellation
  {
    [Fact]
    public async Task when_create_timeout_token_it_should_produce_cancellation_token_cancels_task_within_expected_timeout_length()
    {
      var timeout = Cancellation.TimeoutToken(TimeSpan.FromSeconds(value: 1));
      Func<Task> sut = async () => await Task.Delay(TimeSpan.FromSeconds(value: 3), timeout);
      await sut.Should().ThrowExactlyAsync<TaskCanceledException>();
    }
  }
}
