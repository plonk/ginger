using System;
using Xwt;
using System.Diagnostics;

namespace ginger.View
{
  // ブラウザのビュー。Changed イベントに反応して状態を表示する。
  public partial class Browser
    : Xwt.Window
  {
    AppModel.BrowserModel _model;
    bool _updating = false;

    public Browser(AppModel.BrowserModel model)
      : base()
    {
      Build();
      _model = model;
      _model.Changed += Update;
      // 状態の最初の読み込み。
      Update();
    }

 
    void DoUpdate()
    {
      // コンボボックスにサーバントのリストをロードする。
      _comboBox.Items.Clear();
      foreach (var servent in _model.Servers) {
        _comboBox.Items.Add(servent.ToString());
      }

      var idx = _model.Servers.IndexOf(_model.SelectedServer);
      if (idx != -1)
        _comboBox.SelectedItem = _comboBox.Items [idx];

      Title = _model.Title;

      LoadChannels();

      LoadChannelInfo();

      _versionLabel.Text = _model.SelectedServer?.VersionInfo?.AgentName;

      if (!_model.IsOpen)
        this.Dispose();
    }

    void LoadChannelInfo()
    {
      if (_model.SelectedChannel == null)
        return;

      var c = _model.SelectedChannel;
      var i = _model.SelectedChannel.Info;
      var s = _model.SelectedChannel.Status;
      var t = _model.SelectedChannel.Track;

      var data = new object[] {
        i.Name,
        i.Genre,
        i.Desc,
        i.Url,
        i.Comment,
        c.ChannelId,
        s.Uptime,
        i.Bitrate,
        t.Name,
        t.Album,
        t.Creator,
        t.Genre,
        t.Url
      };

      Debug.Assert(data.Length == _channelInfoTextEntries.Count);
      for (int j = 0; j < data.Length; j++) {
        _channelInfoTextEntries [j].Text = data [j].ToString();
      }
    }

    void LoadChannels()
    {
      int r = _channelList.SelectedRow;
      _channelList.Items.Clear();
      int i = 0;
      foreach (var ch in _model.Channels) {
        _channelList.Items.Add(i, ch.Info.Name);
      }
      _channelList.SelectRow(r);
    }

    // 状態の反映。
    void Update()
    {
      _updating = true;
      DoUpdate();
      _updating = false;
    }

  }
}
