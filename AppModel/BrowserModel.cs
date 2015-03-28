using System;
using System.Collections.Generic;
using ginger.Model;

namespace ginger.AppModel
{
  public delegate void ChangedEvent();

  public class BrowserModel
  {
    public event ChangedEvent Changed;

    public string ButtonLabel {
      get {
        if (_count > 0)
          return string.Format ("オシタナ{0}", _count);
        else
          return "オセ";
      }
    }
    public string Title {
      get {
        return "localhost:7144 - ginger";
      }
    }

    int _count;

    public bool IsOpen = true;
    ProgramModel _program;

    public BrowserModel (ProgramModel program)
    {
      _program = program;
    }

    public void Click()
    {
      _count++;
      Changed ();
      _program.OpenBrowser ();
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

    public Servent SelectedServent { get; set; }
  }
}

