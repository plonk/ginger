using System;
using Xwt;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Reflection;
using Xwt.Drawing;

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
      InitializeApplication();

      var ginger = new Ginger();
      ginger.LoadSettings();
      ginger.AddBrowser(new Browser(ginger));

      Application.Run();
    }

    Ginger()
    {
      var image_path = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
        "ginger_icon.png");
      Icon = Image.FromFile(image_path);
    }

    static void InitializeApplication()
    {
      //      if (System.Environment.OSVersion.Platform == PlatformID.Unix)
      //        Application.Initialize(ToolkitType.Gtk);
      //      else if (System.Environment.OSVersion.Platform == PlatformID.MacOSX)
      //        Application.Initialize(ToolkitType.XamMac);
      //      else if (System.Environment.OSVersion.Platform == PlatformID.Win32NT)
      //        Application.Initialize(ToolkitType.Wpf);
      //      else
      //        Application.Initialize(ToolkitType.Gtk);
      Application.Initialize(ToolkitType.Gtk);
    }

    public List<Browser> Browsers = new List<Browser>();
    public List<Server> Servers = new List<Server>();
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
      Servers.Add(new Server("localhost", 7144));
      Servers.Add(new Server("windows", 7144));
      DefaultServer = new Server("localhost", 7144);
    }

    void SaveSettings()
    {
    }

    public void RequestExit()
    {
      foreach (var browser in Browsers) {
        browser.Dispose();
      }
      Application.Exit();
    }

    public void OnStart()
    {
      LoadSettings();
    }

    public string VersionString {
      get { return "ginger version 0.0.1"; }
    }

    public Image Icon;
  }
}
