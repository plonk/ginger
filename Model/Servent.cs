using System;

namespace ginger.Model
{
  public class Servent
  {
    public string Hostname { get; set; }
    public int Port { get; set; }

    public Servent (string hostname, int port)
    {
      Hostname = hostname;
      Port = port;
    }

    public override string ToString () {
      return string.Format ("{0}:{1}", Hostname, Port);
    }
  }
}
