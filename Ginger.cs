using System;
using Xwt;

namespace ginger
{
  public class Ginger
  {
    // エントリーポイント
    [STAThread]
    static void Main ()
    {
      Application.Initialize (ToolkitType.Gtk);
      var model = new AppModel.ProgramModel ();
      new View.Program (model);
      model.Start ();
      Application.Run ();
    }
  }
}
