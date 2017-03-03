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
    Label _messageArea;
    MenuItem _browserMenuItem;
    MenuItem _quitMenuItem;
    MenuItem _aboutMenuItem;

    // ウィジェットの作成とコールバックの設定。
    void Build()
    {
      BuildMainMenu();

      var vbox = new VBox();
      vbox.Spacing = 10;

      var buttonWidth = 80;
      var buttonBox = new HBox() { ExpandHorizontal = false };
      _reloadButton = new Button("再読み込み") {
        WidthRequest = buttonWidth,
        TooltipText = "再読み込みします。"
      };
      _autoReloadButton = new ToggleButton("自動") {
        WidthRequest = buttonWidth,
        TooltipText = "自動読み込みを開始します。"
      };
      _pauseButton = new ToggleButton("停止") {
        WidthRequest = buttonWidth,
        TooltipText = "自動読み込みを停止します。"
      };
      buttonBox.PackStart(_reloadButton);
      buttonBox.PackStart(_autoReloadButton);
      buttonBox.PackStart(_pauseButton);
      vbox.PackStart(buttonBox);

      var bar = ServerBar();
      vbox.PackStart(bar);

      var nb = Notebook();
      _notebook = nb;
      vbox.PackStart(nb, true, true);

      _messageArea = new Label();
      vbox.PackStart(_messageArea, true, WidgetPlacement.Center, WidgetPlacement.Center);

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
      var fileMenuItem = new MenuItem("ファイル");
      var fileMenu = new Menu();
      _browserMenuItem = new MenuItem("ブラウザで表示");
      _quitMenuItem = new MenuItem("終了");
      fileMenu.Items.Add(_browserMenuItem);
      fileMenu.Items.Add(new SeparatorMenuItem());
      fileMenu.Items.Add(_quitMenuItem);
      fileMenuItem.SubMenu = fileMenu;
      return fileMenuItem;
    }

    MenuItem BuildHelpMenuItem()
    {
      var s = new Menu();
      var h = new MenuItem("ヘルプ");
      _aboutMenuItem = new MenuItem("gingerのバージョン情報");
      h.SubMenu = s;
      s.Items.Add(_aboutMenuItem);
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
      nb.Add(new InformationPage(_context), "情報");
      nb.Add(new ChannelPage(_context), "チャンネル");
      nb.Add(new SettingsPage(_context), "設定");
      nb.Add(new Label("イエローページ"), "イエローページ");
      nb.TabOrientation = NotebookTabOrientation.Top;
      return nb;
    }

  }
}

