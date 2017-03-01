using System;
using ginger.Rpc;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ginger
{
  public class Server
  {
    public JsonRpcClient RpcClient { get { return _cli; } }

    JsonRpcClient _cli;

    public string Hostname;
    public int Port;

    public Server(string hostname, int port)
    {
      Hostname = hostname;
      Port = port;
      _cli = new JsonRpcClient($"http://{hostname}:{port}/api/1");
    }

    public Task<Channel[]> GetChannelsAsync()
    {
      return _cli.InvokeAsync<Channel[]>("getChannels");
    }

    public Task<VersionInfo> GetVersionInfoAsync()
    {
      return _cli.InvokeAsync<VersionInfo>("getVersionInfo");
    }

    public Task<Connection[]> GetChannelConnectionsAsync(string channelId)
    {
      JObject args = new JObject();
      args["channelId"] = channelId;
      return _cli.InvokeAsync<Connection[]>("getChannelConnections", args);
    }

    public Task<Status> GetStatus()
    {
      return _cli.InvokeAsync<Status>("getStatus");
    }

  }

  public class Status
  {
    public int Uptime;
    public bool? IsFirewalled;
    public Tuple<String, int> GlobalRelayEndPoint;
    public Tuple<String, int> GlobalDirectEndPoint;
    public Tuple<String, int> LocalRelayEndPoint;
    public Tuple<String, int> LocalDirectEndPoint;
  }

  public class VersionInfo
  {
    public string AgentName;
  }

  public class Channel
  {
    public string ChannelId;
    public ChannelStatus Status;
    public ChannelInfo Info;
    public Track Track;
    // YellowPage YellowPages;
  }

  public class ChannelStatus
  {
    public string Status;
    public string Source;
    public int Uptime;
    public int LocalRelays;
    public int LocalDirects;
    public int TotalRelays;
    public int TotalDirects;
    public bool IsBroadcasting;
    public bool IsRelayFull;
    public bool IsReceiving;
  }

  public class ChannelInfo
  {
    public string Name;
    public string Url;
    public string Genre;
    public string Desc;
    public string Comment;
    public int Bitrate;
    public string ContentType;
    public string MimeType;
  }

  public class Track
  {
    public string Name;
    public string Genre;
    public string Album;
    public string Creator;
    public string Url;
  }

  public class Connection
  {
    public int ConnectionId;
    public string Type;
    public string Status;
    public float SendRate;
    public float RecvRate;
    public string ProtocolName;
    public int? LocalRelays;
    public int? LocalDirects;
    public int ContentPosition;
    public string AgentName;
    public string RemoteEndPoint;
    public string[] RemoteHostStatus;
    public string RemoteName;
  }
}
