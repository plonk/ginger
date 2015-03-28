using System;
using Xwt;

namespace ginger.View
{
  public class Program
  {
    public Program (AppModel.ProgramModel model)
    {
      // 開かれたブラウザモデルにビューを設定する。
      model.BrowserAdded += (browser) => new View.Browser (browser);
      // プログラムモデルが終了したら実際に終了する。
      model.Exit += Application.Exit;
    }

  }
}
