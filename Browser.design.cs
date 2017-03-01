using System;
using Xwt;
using System.Collections.Generic;

namespace ginger
{
  // ウィジェットの初期化など。
  public partial class Browser
  {
    ComboBox _comboBox;
    Button _reloadButton;
    ToggleButton _autoReloadButton;
    ToggleButton _pauseButton;
    Label _statusLabel;
    Notebook _notebook;

    // ウィジェットの作成とコールバックの設定。
    void Build()
    {
      Width = 500;
      Height = 400;

      BuildMainMenu();

      var vbox = new VBox();
      vbox.Spacing = 10;

      var buttonWidth = 30;
      var buttonBox = new HBox() { ExpandHorizontal = false };
      _reloadButton = new Button("↻") { WidthRequest = buttonWidth, TooltipText = "再読み込み" };
      _autoReloadButton = new ToggleButton("⏵") { WidthRequest = buttonWidth, TooltipText = "自動読み込み開始" };
      _pauseButton = new ToggleButton("⏸") { WidthRequest = buttonWidth, TooltipText = "自動読み込み停止" };
      buttonBox.PackStart(_reloadButton);
      buttonBox.PackStart(_autoReloadButton);
      buttonBox.PackStart(_pauseButton);
      vbox.PackStart(buttonBox);

      var bar = ServerBar();
      vbox.PackStart(bar);

      var nb = Notebook();
      _notebook = nb;
      vbox.PackStart(nb);

      _statusLabel = new Label();
      vbox.PackStart(_statusLabel);

      Content = vbox;
      Show();
    }

    Widget ServerBar()
    {
      var hbox = new HBox();
      hbox.PackStart(new Label("サーバー:"));
      _comboBox = new ComboBox();
      hbox.PackStart(_comboBox, true, true); // expand, fill
      return hbox;
    }

    MenuItem BuildFileMenuItem()
    {
      var m = new MenuItem("ファイル");
      var s = new Menu();
      var b = new MenuItem("ブラウザで表示");
      var q = new MenuItem("終了");
      s.Items.Add(b);
      s.Items.Add(new SeparatorMenuItem());
      s.Items.Add(q);
      m.SubMenu = s;
      return m;
    }

    MenuItem BuildHelpMenuItem()
    {
      var s = new Menu();
      var h = new MenuItem("ヘルプ");
      var v = new MenuItem("gingerのバージョン情報");
      h.SubMenu = s;
      s.Items.Add(v);
      return h;
    }

    void BuildMainMenu()
    {
      MainMenu = new Menu();
      MainMenu.Items.Add(BuildFileMenuItem());
      MainMenu.Items.Add(BuildHelpMenuItem());
    }

    Notebook Notebook()
    {
      Notebook nb = new Notebook();
      nb.Add(InformationPage(), "情報");
      nb.Add(ChannelPage(), "チャンネル");
      nb.Add(new Label("接続待ち受け"), "ポートと認証");
      nb.Add(new Label("帯域制御"), "帯域制御");
      nb.Add(new Label("イエローページ"), "イエローページ");
      nb.TabOrientation = NotebookTabOrientation.Top;
      return nb;
    }

    Widget InformationPage()
    {
      return new InformationPage();
    }

    Widget ChannelPage()
    {
      return new ChannelPage();
    }

  }
}

