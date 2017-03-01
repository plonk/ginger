using System;
using ginger.Rpc;
using System.Threading;
using System.Threading.Tasks;

namespace ginger.Model
{
  public class Server
  {
    public string Hostname { get; set; }

    public int Port { get; set; }

    Peercast Api { get; set; }

    public Server(string hostname, int port)
    {
      Hostname = hostname;
      Port = port;

      Api = new Peercast(Hostname, Port);
    }

    public async Task LoadAsync()
    {
      Channels = await Api.GetChannelsAsync();
      VersionInfo = await Api.GetVersionInfoAsync();
      Status = await Api.GetStatus();
    }

    public Channel[] Channels;
    public VersionInfo VersionInfo = new VersionInfo { AgentName = "" };
    public Status Status;

    public override string ToString()
    {
      return string.Format("{0}:{1}", Hostname, Port);
    }
  }
}
