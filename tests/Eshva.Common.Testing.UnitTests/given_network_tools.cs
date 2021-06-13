#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using FluentAssertions;
using Xunit;

#endregion

namespace Eshva.Common.Testing.UnitTests
{
  public class given_network_tools
  {
    [Fact]
    public void when_get_free_port_it_should_find_same_port_few_times()
    {
      var freePort = NetworkTools.GetFreeTcpPort();
      NetworkTools.GetFreeTcpPort().Should().Be(freePort);
      NetworkTools.GetFreeTcpPort().Should().Be(freePort);
    }

    [Fact]
    public void when_there_is_no_free_ports_in_range_it_should_fail()
    {
      const ushort rangeStart = 50000;
      const ushort rangeFinish = 50002;
      var occupiedPorts = OccupyPortRange(rangeStart, rangeFinish).ToArray();

      Action sut = () => NetworkTools.GetFreeTcpPort(rangeStart, rangeFinish);
      sut.Should().ThrowExactly<ArgumentException>().Which.Message.Should().Contain("range");

      FreePortRange(occupiedPorts);
    }

    private IEnumerable<TcpListener> OccupyPortRange(ushort rangeStart, ushort rangeFinish)
    {
      for (var port = rangeStart; port <= rangeFinish; port++)
      {
        var listener = new TcpListener(IPAddress.Loopback, port);
        try
        {
          listener.Start();
        }
        catch (SocketException)
        {
          continue;
        }

        yield return listener;
      }
    }

    private void FreePortRange(TcpListener[] occupiedPorts)
    {
      foreach (var listener in occupiedPorts)
      {
        listener.Stop();
      }
    }
  }
}
