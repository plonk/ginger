using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using ginger.Rpc;

namespace JsonRpcTest
{


  class MainClass
  {
    static Peercast pc = new Peercast ();

    static void GetVersion()
    {
      Console.WriteLine (pc.GetVersionInfoAsync().Result.AgentName);
    }

    static void GetChannels()
    {
      foreach (var ch in pc.GetChannelsAsync ().Result) {
        Console.WriteLine ("{0} {1} {2} {3}", ch.ChannelId.Substring (0, 3), ch.info.Name, ch.info.Desc, ch.info.Comment);
        foreach (var conn in pc.GetChannelConnectionsAsync(ch.ChannelId).Result) {
          Console.WriteLine ("{0}, {1}", conn.AgentName, conn.RemoteEndPoint);
        }
      }
    }

    public static void Main (string[] args)
    {
      GetVersion ();
      GetChannels ();
    }
  }
}
