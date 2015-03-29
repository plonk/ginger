using System;
using Xwt;

namespace XwtTest
{
  public class MainClass
  {
    // エントリーポイント
    [STAThread]
    static void Main ()
    {
      Application.Initialize (ToolkitType.Gtk);
      var w = new Window ();

      w.Show();
      Application.Run ();

    }
  }
}


