using System;
using Xwt;
using System.Diagnostics;

namespace ginger
{
  public class Ginger
  {
    // エントリーポイント
    [STAThread]
    static void Main()
    {
      Application.Initialize(ToolkitType.Gtk);

      var model = new AppModel.ProgramModel();

      // 開かれたブラウザモデルにビューを設定する。
      model.BrowserAdded += browser => new View.Browser(browser);
      model.OnStart();

      Application.Run();
    }
  }
}
