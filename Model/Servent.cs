using System;
using ginger.Rpc;

namespace ginger.Model
{
  public class Servent
  {
    public string Hostname { get; set; }
    public int Port { get; set; }

    Peercast Api { get; set; }

    bool _cacheInvalidated = true;

    public Servent (string hostname, int port)
    {
      Hostname = hostname;
      Port = port;

      Api = new Peercast ();
    }

    Channel[] _Channels;
    public Channel[] Channels {
      get {
        if (_Channels == null || _cacheInvalidated)
          _Channels = Api.GetChannelsAsync ().Result;
        return _Channels;
      }
    }

    public override string ToString () {
      return string.Format ("{0}:{1}", Hostname, Port);
    }
  }
}
