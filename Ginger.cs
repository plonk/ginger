using System;
using Xwt;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Reflection;
using Xwt.Drawing;
using Newtonsoft.Json;

namespace ginger
{
  // プログラム全体の挙動。
  // ブラウザウィンドウが 0 個になったら終了する。
  public class Ginger
  {
    public List<Browser> Browsers = new List<Browser>();
    public List<Server> Servers = new List<Server>();
    public Image Icon;

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
      Application.Initialize(ToolkitType.Gtk);
    }

    void AddBrowser(Browser browser)
    {
      Browsers.Add(browser);
      browser.Closed += (sender, e) => {
        Browsers.Remove(browser);
        if (Browsers.Count == 0)
          Application.Exit();
      };
    }

    string SettingsDirectoryPath()
    {
      var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
      return Path.Combine(appData, "ginger");
    }

    string SettingsFilePath()
    {
      return Path.Combine(SettingsDirectoryPath(), "settings.json");
    }

    GingerSettings Settings;
    void LoadSettings()
    {
      Directory.CreateDirectory(SettingsDirectoryPath());
      string text;
      try {
        text = File.ReadAllText(SettingsFilePath());
        Settings = JsonConvert.DeserializeObject<GingerSettings>(text);
      } catch (FileNotFoundException) {
        Settings = new GingerSettings();
        SaveSettings();
      }
      Servers = Settings.servers;
//      Servers.Add(new Server("ゲストOS", "localhost", 7144));
//      Servers.Add(new Server("ホストOS", "windows", 7144));
    }

    public void SaveSettings()
    {
      var text = JsonConvert.SerializeObject(Settings, Formatting.Indented);
      Directory.CreateDirectory(SettingsDirectoryPath());
      File.WriteAllText(SettingsFilePath(), text);
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
      get { return "ginger version 0.0.1-alpha"; }
    }
  }

  class GingerSettings
  {
    public List<Server> servers;

    public GingerSettings()
    {
      servers = new List<Server>();
    }
  }
}
