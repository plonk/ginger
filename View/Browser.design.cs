using System;
using Xwt;

namespace ginger.View
{
  // ウィジェットの初期化など。
  public partial class Browser
  {
    ComboBox _comboBox;
    ListBox _channelList;

    // ウィジェットの作成とコールバックの設定。
    void Build ()
    {
      Width = 500;
      Height = 400;

      BuildMainMenu ();

      var bar = ServentBar ();


      var vbox = new VBox ();
      vbox.PackStart (bar);
      vbox.PackStart (Notebook ());

      Content = vbox;

      Closed += (sender, e) => {
        _model.Close ();
      };
      Show ();
    }

    Widget ServentBar ()
    {
      _comboBox = new ComboBox ();
      _comboBox.SelectionChanged += (sender, e) => {
        if (!_updating)
          _model.SelectedServent = _model.Servents [_comboBox.SelectedIndex];
      };
      return _comboBox;
    }

    MenuItem BuildFileMenuItem ()
    {
      var m = new MenuItem ("ファイル");
      var s = new Menu ();
      var b = new MenuItem ("ブラウザで表示");
      b.Clicked += (sender, e) => _model.OpenInBrowser ();
      var q = new MenuItem ("閉じる");
      q.Clicked += (sender, e) => this.Close ();
      s.Items.Add (b);
      s.Items.Add (new SeparatorMenuItem ());
      s.Items.Add (q);
      m.SubMenu = s;
      return m;
    }

    MenuItem BuildEditMenuItem ()
    {
      return new MenuItem ("編集");
    }

    MenuItem BuildChannelMenuItem ()
    {
      return new MenuItem ("チャンネル");
    }

    MenuItem BuildToolsMenuItem ()
    {
      return new MenuItem ("ツール");
    }

    MenuItem BuildHelpMenuItem ()
    {
      var s = new Menu ();
      var h = new MenuItem ("ヘルプ");
      var v = new MenuItem ("gingerのバージョン情報");
      v.Clicked += (sender, e) => MessageDialog.ShowMessage (this, _model.VersionString);
      h.SubMenu = s;
      s.Items.Add (v);
      return h;
    }

    void BuildMainMenu ()
    {
      MainMenu = new Menu ();
      MainMenu.Items.Add (BuildFileMenuItem ());
      // MainMenu.Items.Add (BuildEditMenuItem ());
      // MainMenu.Items.Add (BuildChannelMenuItem ());
      // MainMenu.Items.Add (BuildToolsMenuItem ());
      MainMenu.Items.Add (BuildHelpMenuItem ());
    }

    Notebook Notebook ()
    {
      Notebook nb = new Notebook ();
      nb.Add (ChannelPage (), "チャンネル");
      nb.Add (new Label ("接続待ち受け"), "ポートと認証");
      nb.Add (new Label ("帯域制御"), "帯域制御");
      nb.Add (new Label ("イエローページ"), "イエローページ");
      nb.Add (new Label ("バージョン情報とプラグイン情報"), "バージョン情報");
      nb.TabOrientation = NotebookTabOrientation.Top;
      return nb;
    }

    Widget ChannelPage ()
    {
      var vbox = new VBox ();
      var hbox = new HBox ();

      var list = new ListBox ();

      _channelList = list;

      var cmds = CommandBox ();

      hbox.PackStart (list, true);
      hbox.PackStart (cmds);

      vbox.PackStart (hbox);

      var props = ChannelProperties ();

      vbox.PackStart (props);
      return vbox;
    }

    Widget ChannelProperties ()
    {
      var nb = new Notebook ();

      nb.Add (new Label (""), "接続");
      nb.Add (new Label (""), "チャンネル情報");
      nb.Add (new Label (""), "リレーツリー");
      return nb;
    }

    Widget CommandBox ()
    {
      var vbox = new VBox ();

      var p = new Button ("再生");
      var d = new Button ("切断");
      var r = new Button ("再接続");
      var b = new Button ("配信...");

      foreach (var w in new Widget[] { p, d, r, b }) {
        vbox.PackStart (w);
      }
      return vbox;
    }

  }
}

