using System;
using Xwt;
using System.Collections.Generic;

namespace ginger.View
{
  // ウィジェットの初期化など。
  public partial class Browser
  {
    ComboBox _comboBox;
    ListBox _channelList;

    // ウィジェットの作成とコールバックの設定。
    void Build()
    {
      Width = 500;
      Height = 400;

      BuildMainMenu();

      var bar = ServerBar();


      var vbox = new VBox();
      vbox.Spacing = 10;
      vbox.PackStart(bar);
      vbox.PackStart(Notebook());

      Content = vbox;

      Closed += (sender, e) => {
        _model.Close();
      };
      Show();
    }

    Widget ServerBar()
    {
      _comboBox = new ComboBox();
      _comboBox.SelectionChanged += (sender, e) => {
        if (!_updating)
          _model.SelectedServer = _model.Servers [_comboBox.SelectedIndex];
      };
      return _comboBox;
    }

    MenuItem BuildFileMenuItem()
    {
      var m = new MenuItem("ファイル");
      var s = new Menu();
      var b = new MenuItem("ブラウザで表示");
      b.Clicked += (sender, e) => _model.OpenInBrowser();
      var q = new MenuItem("終了");
      q.Clicked += (sender, e) => _model.Program.Exit();
      s.Items.Add(b);
      s.Items.Add(new SeparatorMenuItem());
      s.Items.Add(q);
      m.SubMenu = s;
      return m;
    }

    MenuItem BuildEditMenuItem()
    {
      return new MenuItem("編集");
    }

    MenuItem BuildChannelMenuItem()
    {
      return new MenuItem("チャンネル");
    }

    MenuItem BuildToolsMenuItem()
    {
      return new MenuItem("ツール");
    }

    MenuItem BuildHelpMenuItem()
    {
      var s = new Menu();
      var h = new MenuItem("ヘルプ");
      var v = new MenuItem("gingerのバージョン情報");
      v.Clicked += (sender, e) => MessageDialog.ShowMessage(this, _model.VersionString);
      h.SubMenu = s;
      s.Items.Add(v);
      return h;
    }

    void BuildMainMenu()
    {
      MainMenu = new Menu();
      MainMenu.Items.Add(BuildFileMenuItem());
      // MainMenu.Items.Add (BuildEditMenuItem ());
      // MainMenu.Items.Add (BuildChannelMenuItem ());
      // MainMenu.Items.Add (BuildToolsMenuItem ());
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

    public Label _versionLabel;

    Widget InformationPage()
    {
      _versionLabel = new Label("私が情報だ！");
      return _versionLabel;
    }

    Widget ChannelPage()
    {
      var vbox = new VBox();
      vbox.Margin = 10;
      vbox.Spacing = 10;
      var hbox = new HBox();

      var list = new ListBox();
      list.SelectionChanged += (sender, e) => {
        if (!_updating)
          _model.SelectedChannel = _model.Channels [list.SelectedRow];
      };
      _channelList = list;

      var cmds = CommandBox();
      cmds.WidthRequest = 80;

      hbox.PackStart(list, true);
      hbox.PackStart(cmds);

      vbox.PackStart(hbox);

      var props = ChannelProperties();

      vbox.PackStart(props);
      return vbox;
    }

    Label _channelInfoLabel;

    Widget ChannelProperties()
    {
      var nb = new Notebook();

      nb.Add(new Label(""), "接続");
      _channelInfoLabel = new Label("");
      nb.Add(ChannelInfoPage(), "チャンネル情報");
      nb.Add(new Label(""), "リレーツリー");
      return nb;
    }

    Widget CommandBox()
    {
      var vbox = new VBox();

      var p = new Button("再生");
      var d = new Button("切断");
      var r = new Button("再接続");
      var b = new Button("配信...");

      foreach (var w in new Widget[] { p, d, r, b }) {
        vbox.PackStart(w);
      }
      return vbox;
    }

    List<TextEntry> _channelInfoTextEntries = new List<TextEntry>();

    Widget ChannelInfoPage()
    {
      var t = new Table();
      t.Margin = 10;
      t.SetColumnSpacing(1, 10);

      var labels = new List<string>() {
        "チャンネル名",
        "ジャンル",
        "概要",
        "コンタクトURL",
        "配信者コメント",
        "チャンネルID",
        "配信・リレー時間",
        "ビットレート",
        "トラックタイトル",
        "アルバム",
        "アーティスト",
        "トラックジャンル",
        "トラックURL"
      };
      for (int i = 0; i < labels.Count; i++) {
        t.Add(new Label(labels [i]), 0, i);
        var entry = new TextEntry();
        t.Add(entry, 1, i);
        _channelInfoTextEntries.Add(entry);
      }

      var apply = new Button("適用");
      t.Add(apply, 1, labels.Count + 1);

      return t;
    }

  }
}

