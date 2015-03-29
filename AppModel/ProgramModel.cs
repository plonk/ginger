using System;
using System.Collections.Generic;
using ginger.Model;

namespace ginger.AppModel
{
  public delegate void BrowserAddedEvent(BrowserModel browser);

  // プログラム全体の挙動。
  // ブラウザウィンドウが 0 個になったら終了する。
  public class ProgramModel
  {
    // Changed の替わりに個別のイベントを持つ。
    public event BrowserAddedEvent BrowserAdded;
    public event Action Exit;

    List<BrowserModel> _browsers = new List<BrowserModel>(); 
    List<Servent> _servents = new List<Servent>(); 

    public List<Servent> Servents {
      get { return _servents; }
    }

    public ProgramModel ()
    {
      LoadSettings ();
    }

    void LoadSettings()
    {
      _servents.Add (new Servent ("localhost", 7144));
      _servents.Add (new Servent ("xubuntu-14", 7145));
    }

    void AddBrowser (BrowserModel browser)
    {
      _browsers.Add (browser);
      BrowserAdded (browser);
    }

    public void Start()
    {
      var firstBrowser = new BrowserModel (this);
      firstBrowser.Changed += () => {
        if (!firstBrowser.IsOpen) {
          _browsers.Remove (firstBrowser);
        }
        Update();
      };
      AddBrowser (firstBrowser);
    }

    public void Update()
    {
      if (_browsers.Count == 0)
        Exit();
    }

    public void OpenBrowser()
    {
      var browser = new BrowserModel (this);
      browser.Changed += () => {
        if (!browser.IsOpen)
          _browsers.Remove (browser);
        Update ();
      };
      AddBrowser (browser);
    }
  }
}

