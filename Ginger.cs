using System;
using Xwt;
using System.Diagnostics;
using System.Collections.Generic;

namespace ginger
{
  // プログラム全体の挙動。
  // ブラウザウィンドウが 0 個になったら終了する。
  public class Ginger
  {
    // エントリーポイント
    [STAThread]
    static void Main()
    {
      Application.Initialize(ToolkitType.Gtk);
      var ginger = new Ginger();
      ginger.LoadSettings();
      ginger.AddBrowser(new Browser(ginger));
      Application.Run();
    }

    public List<Browser> Browsers = new List<Browser>();
    public List<Server> KnownServers = new List<Server>();
    public Server DefaultServer;

    void AddBrowser(Browser browser)
    {
      Browsers.Add(browser);
      browser.Closed += (sender, e) => {
        Browsers.Remove(browser);
        if (Browsers.Count == 0)
          Application.Exit();
      };
    }

    void LoadSettings()
    {
      KnownServers.Add(new Server("localhost", 7144));
      KnownServers.Add(new Server("windows", 7144));
      DefaultServer = new Server("localhost", 7144);
    }

    void SaveSettings()
    {
    }

    public void OnStart()
    {
      LoadSettings();
    }

    public string VersionString {
      get { return "ginger version 0.0.1"; }
    }
  }
}
