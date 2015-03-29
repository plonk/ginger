using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace ginger.Rpc
{
  public class Peercast
  {
    public JsonRpcClient RpcClient { get{ return  _cli; } }

    JsonRpcClient _cli;

    public Peercast ()
    {
      _cli = new JsonRpcClient ("http://windows:7144/api/1");
      _cli.Authorization = new Tuple<string,string> ("hk'_uK9qv-td0ag.", "3ihvZAo.s$Jp'eZh");
    }

    public Task<Channel[]> GetChannelsAsync() { return _cli.InvokeAsync<Channel[]> ("getChannels"); }
    public Task<VersionInfo> GetVersionInfoAsync() { return _cli.InvokeAsync<VersionInfo> ("getVersionInfo"); }

    public Task<Connection[]> GetChannelConnectionsAsync(string channelId)
    {
      JObject args = new JObject ();
      args ["channelId"] = channelId;
      return _cli.InvokeAsync<Connection[]> ("getChannelConnections", args);
    }

  }
  public class VersionInfo
  {
    public string AgentName { get; set; }
  }

  public class Channel
  {
    public string ChannelId;
    public ChannelStatus status;

    public ChannelInfo info {get; set; }


    // YellowPage YellowPages { get; set; }
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
    public string Name { get; set; }
    public string Url { get; set; }
    public string Genre { get; set; }
    public string Desc { get; set; }
    public string Comment {get; set; }
    public int Bitrate { get; set; }
    public string ContentType { get; set; }
    public string MimeType { get; set; }
    public Track Track { get; set; }
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

