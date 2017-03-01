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
      ginger.Browsers.Add(new Browser(ginger));
      Application.Run();
    }

    public List<Browser> Browsers = new List<Browser>();

    void LoadSettings()
    {
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
