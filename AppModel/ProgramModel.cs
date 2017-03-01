using System;
using System.Collections.Generic;
using ginger.Model;
using Xwt;

namespace ginger.AppModel
{
  public delegate void BrowserAddedEvent(BrowserModel browser);

  // プログラム全体の挙動。
  // ブラウザウィンドウが 0 個になったら終了する。
  public class ProgramModel
  {
    // Changed の替わりに個別のイベントを持つ。
    public event BrowserAddedEvent BrowserAdded;

    List<BrowserModel> _browsers = new List<BrowserModel>();
    List<Server> _servers = new List<Server>();

    public void Exit()
    {
      Application.Exit();
    }

    public List<Server> Servers {
      get { return _servers; }
    }

    public ProgramModel()
    {
    }

    void LoadSettings()
    {
      //_servents.Add (new Servent ("localhost", 7144));
    }

    void SaveSettings()
    {
    }

    void AddBrowser(BrowserModel browser)
    {
      _browsers.Add(browser);
      BrowserAdded(browser);
    }

    public void OnStart()
    {
      LoadSettings();

      if (_servers.Count == 0) {
        var dialog = new ServerAddDialog();
        Command response = dialog.Run();
        dialog.Close();
        if (response == Command.Cancel)
          System.Environment.Exit(0);

        _servers.Add(new Server(dialog.Hostname, dialog.Port));
        SaveSettings();
      }

      var firstBrowser = new BrowserModel(this);
      firstBrowser.Changed += () => {
        if (!firstBrowser.IsOpen) {
          _browsers.Remove(firstBrowser);
        }
        Update();
      };
      AddBrowser(firstBrowser);
    }

    public void Update()
    {
      if (_browsers.Count == 0)
        Exit();
    }

    public void OpenBrowser()
    {
      var browser = new BrowserModel(this);
      browser.Changed += () => {
        if (!browser.IsOpen)
          _browsers.Remove(browser);
        Update();
      };
      AddBrowser(browser);
    }
  }
}

