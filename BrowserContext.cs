using System;
using Xwt;

namespace ginger
{
  public class BrowserContext
  {
    public Ginger Ginger;
    public Server Server; // 現在選択中のサーバー
    public Window Window;
    public Channel Channel;

    public BrowserContext(Ginger ginger, Window window)
    {
      Ginger = ginger;
      Window = window;
    }
  }
}

