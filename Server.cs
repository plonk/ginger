using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Diagnostics;

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

    public Task<GetChannelInfoResult> GetChannelInfoAsync(string channelId)
    {
      var args = new JObject();
      args["channelId"] = channelId;
      return _cli.InvokeAsync<GetChannelInfoResult>("getChannelInfo", args);
    }

    public Task<Status> GetStatusAsync()
    {
      return _cli.InvokeAsync<Status>("getStatus");
    }

    public Task<ChannelStatus> GetChannelStatusAsync(string channelId)
    {
      var args = new JObject();
      args["channelId"] = channelId;
      return _cli.InvokeAsync<ChannelStatus>("getChannelStatus", args);
    }

    public Task<RelayNode[]> GetChannelRelayTreeAsync(string channelId)
    {
      var args = new JObject();
      args["channelId"] = channelId;
      return _cli.InvokeAsync<RelayNode[]>("getChannelRelayTree", args);
    }

    public Task<Settings> GetSettingsAsync()
    {
      return _cli.InvokeAsync<Settings>("getSettings");
    }

    public Task SetChannelInfoAsync(string channelId, ChannelInfo info, Track track)
    {
      var json_info = new JObject();
      json_info["name"] = info.Name;
      json_info["url"] = info.Url;
      json_info["genre"] = info.Genre;
      json_info["desc"] = info.Desc;
      json_info["comment"] = info.Comment;

      var json_track = new JObject();
      json_track["name"] = track.Name;
      json_track["genre"] = track.Genre;
      json_track["album"] = track.Album;
      json_track["creator"] = track.Creator;
      json_track["url"] = track.Url;

      var args = new JObject();
      args["channelId"] = channelId;
      args["info"] = json_info;
      args["track"] = json_track;

      return _cli.InvokeAsync("setChannelInfo", args);
    }

    async public Task SetSettingsAsync(Settings settings)
    {
      var args = new JObject();
      args["settings"] = JObject.Parse(JsonConvert.SerializeObject(settings));
      _cli.InvokeAsync("setSettings", args);
    }
  }

  public class Settings
  {
    public int maxRelays;
    public int maxRelaysPerChannel;
    public int maxDirects;
    public int maxDirectsPerChannel;
    public int maxUpstreamRate;
    public int maxUpstreamRatePerChannel;
    public ChannelCleanerSettings channelClearner;
  }

  public class ChannelCleanerSettings
  {
    public int mode;
    public int inactiveLimit;
  }

  public class RelayNode
  {
    public string SessionId;
    public string Address;
    public int Port;
    public bool? IsFirewalled;
    public int LocalRelays;
    public int LocalDirects;
    public bool? IsTracker;
    public bool? IsRelayFull;
    public bool? IsDirectFull;
    public bool? IsReceiving;
    public bool? IsControlFull;
    public string Version;
    public string VersionVP; // ?;
    public RelayNode[] Children;
  }

  public class GetChannelInfoResult
  {
    public ChannelInfo Info;
    public Track Track;
    public YellowPage[] YellowPages;

  }

  public class YellowPage
  {
    public int YellowPageId; // non-negative signed int32
    public string Name;
    public string Uri;
    public string Protocol;
    public ChannelInfoBrief[] Channels;
  }

  public class ChannelInfoBrief
  {
    public string ChannelId;
    public string Status;
  }

  public class Status
  {
    public int Uptime;
    public bool? IsFirewalled;
    public object[] GlobalRelayEndPoint;
    public object[] GlobalDirectEndPoint;
    public object[] LocalRelayEndPoint;
    public object[] LocalDirectEndPoint;
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
    public double? SendRate;
    public double? RecvRate;
    public string ProtocolName;
    public int? LocalRelays;
    public int? LocalDirects;
    public UInt32? ContentPosition;
    public string AgentName;
    public string RemoteEndPoint;
    public string[] RemoteHostStatus;
    public string RemoteName;
  }

}
