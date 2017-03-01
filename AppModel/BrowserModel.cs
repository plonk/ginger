using System;
using System.Collections.Generic;
using ginger.Model;
using System.Diagnostics;
using ginger.Rpc;
using System.Linq;
using Xwt;

namespace ginger.AppModel
{
  public delegate void ChangedEvent();

  public class BrowserModel
  {
    public event ChangedEvent Changed;

    public string Title {
      get {
        if (SelectedServer != null) {
          return string.Format("{0}で稼働中の{1} - ginger", SelectedServer.ToString(), SelectedServer.VersionInfo.AgentName);
        }
        else {
          return "ginger";
        }
      }
    }

    public bool IsOpen = true;
    ProgramModel _program;

    public ProgramModel Program {
      get {
        return _program;
      }
    }

    public BrowserModel(ProgramModel program)
    {
      _program = program;
    }

    public void Close()
    {
      IsOpen = false;
      Changed();
    }

    public List<Server> Servers {
      get {
        return _program.Servers;
      }
    }

    Server _selectedServer;

    public Server SelectedServer {
      get {
        return _selectedServer;
      }
      set {
        Debug.Assert(Servers.Contains(value));
        _selectedServer = value;
        _selectedServer.LoadAsync().ContinueWith((prev) => Application.Invoke(() => Changed()));
      }
    }

    public Channel[] Channels {
      get {
        if (SelectedServer != null) {
          return SelectedServer.Channels;
        }
        else {
          return new Channel[] { };
        }
      }
    }

    Channel _selectedChannel;

    public Channel SelectedChannel {
      get {
        return _selectedChannel;
      }
      set {
        Debug.Assert(Channels.Contains(value));
        _selectedChannel = value;
        Console.WriteLine("{0} selected", value);
        Changed();
      }
    }

    public void OpenInBrowser()
    {
      if (_selectedServer != null) {
        Process.Start($"http://{_selectedServer.Hostname}:{_selectedServer.Port}/");
      }
    }

    public string VersionString { get { return "ginger version 0.0.1"; } }
  }
}

