using System;
using Xwt;

namespace ginger.View
{
  // ブラウザのビュー。Changed イベントに反応して状態を表示する。
  public partial class Browser
    : Xwt.Window
  {
    AppModel.BrowserModel _model;
    bool _updating = false;

    public Browser (AppModel.BrowserModel model)
      : base()
    {
      Build ();
      _model = model;
      _model.Changed += Update;
      // 状態の最初の読み込み。
      Update ();
    }

 
    void DoUpdate ()
    {
      // コンボボックスにサーバントのリストをロードする。
      _comboBox.Items.Clear ();
      foreach (var servent in _model.Servents) {
        _comboBox.Items.Add (servent.ToString ());
      }

      var idx = _model.Servents.IndexOf (_model.SelectedServent);
      if (idx != -1)
        _comboBox.SelectedItem = _comboBox.Items [idx];

      Title = _model.Title;

      LoadChannels ();

      if (!_model.IsOpen)
        this.Dispose ();
    }

    void LoadChannels()
    {
      _channelList.Items.Clear ();
      int i = 0;
      foreach (var ch in _model.Channels) {
        _channelList.Items.Add (i, ch.info.Name);
      }
    }

    // 状態の反映。
    void Update()
    {
      _updating = true;
      DoUpdate ();
      _updating = false;
    }

  }
}
