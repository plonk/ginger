using System;
using System.Collections.Generic;
using ginger.Model;
using System.Diagnostics;
using ginger.Rpc;

namespace ginger.AppModel
{
  public delegate void ChangedEvent();

  public class BrowserModel
  {
    public event ChangedEvent Changed;

    public string Title {
      get {
        if (SelectedServent != null) {
          return string.Format ("{0} - ginger", SelectedServent.ToString());
        }
        else {
          return "ginger";
        }
      }
    }

    public bool IsOpen = true;
    ProgramModel _program;

    public BrowserModel (ProgramModel program)
    {
      _program = program;
    }

    public void Close()
    {
      IsOpen = false;
      Changed ();
    }

    public List<Servent> Servents {
      get {
        return _program.Servents;
      }
    }

    Servent _selectedServent;

    public Servent SelectedServent {
      get {
        return _selectedServent;
      }
      set {
        Debug.Assert(Servents.Contains (value));
        _selectedServent = value;
        Changed ();
      }
    }

    public Channel[] Channels {
      get {
        if (SelectedServent != null) {
          return SelectedServent.Channels;
        }
        else {
          return new Channel[] { };
        }
      }
    }

    public void OpenInBrowser()
    {
      System.Diagnostics.Process.Start ("http://www.google.com/");
    }

    public string VersionString { get { return "ginger version 0.0.1"; } }
  }
}

