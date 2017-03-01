using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace ginger.Rpc
{
  public class Peercast
  {
    public JsonRpcClient RpcClient { get { return  _cli; } }

    JsonRpcClient _cli;

    public Peercast(string hostname, int port)
    {
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
      args ["channelId"] = channelId;
      return _cli.InvokeAsync<Connection[]>("getChannelConnections", args);
    }

    public Task<Status> GetStatus()
    {
      return _cli.InvokeAsync<Status>("getStatus");
    }

  }

  public class Status
  {
    public int uptime;
    public bool? isFirewalled;
    public Tuple<String, int> globalRelayEndPoint;
    public Tuple<String, int> globalDirectEndPoint;
    public Tuple<String, int> localRelayEndPoint;
    public Tuple<String, int> localDirectEndPoint;
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

