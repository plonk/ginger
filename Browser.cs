using System;
using Xwt;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ginger
{
  // 「ブラウザ」ウィンドウ
  public partial class Browser
      : Xwt.Window
  {
    // 現在選択中のサーバー
    public Server Server;

    public Browser(Ginger ginger)
      : base()
    {
      Build();

      Title = "ginger";

      _reloadButton.Clicked += async (sender, e) => {
        await UpdateAsync();
      };

      foreach (var server in ginger.KnownServers) {
        _comboBox.Items.Add(server, $"{server.Hostname}:{server.Port}");
      }

      _comboBox.SelectionChanged += async (sender, e) => {
        Server = (Server)_comboBox.SelectedItem;
        await UpdateAsync();
      };

      _notebook.CurrentTabChanged += async (sender, e) => {
        await UpdateAsync();
      };
    }

    async Task UpdateAsync()
    {
      var dataview = _notebook.CurrentTab.Child as ServerView;
      if (dataview != null) {

        _statusLabel.Text = "更新中...";
        var t = DateTime.Now;
        await dataview.UpdateAsync(Server);
        var msec = (DateTime.Now - t).Milliseconds;
        _statusLabel.Text = $"更新完了 ({msec}ms)";
      } else {
        _statusLabel.Text = "更新することないです。";
      }

      Title = $"{Server.Hostname}:{Server.Port} - ginger";

    }

    //Channel _selectedChannel;
    //public Channel SelectedChannel {
    //  get {
    //    return _selectedChannel;
    //  }
    //  set {
    //    Debug.Assert(Channels.Contains(value));
    //    _selectedChannel = value;
    //    Console.WriteLine("{0} selected", value);
    //    Changed();
    //  }
    //}

    public void OpenInBrowser()
    {
      if (Server != null) {
        Process.Start($"http://{Server.Hostname}:{Server.Port}/");
      }
    }

    //async Task Update()
    //{
    //  // コンボボックスにサーバントのリストをロードする。
    //  _comboBox.Items.Clear();
    //  foreach (var servent in _model.Servers) {
    //    _comboBox.Items.Add(servent.ToString());
    //  }

    //  var idx = _model.Servers.IndexOf(_model.SelectedServer);
    //  if (idx != -1)
    //    _comboBox.SelectedItem = _comboBox.Items[idx];

    //  Title = _model.Title;

    //  LoadChannels();

    //  LoadChannelInfo();

    //  LoadVersionInfo();

    //  if (!_model.IsOpen)
    //    this.Dispose();
    //}

    //async Task LoadVersionInfo()
    //{
    //  if (_model.SelectedServer == null) {
    //    _versionText.Markdown = "";
    //    return;
    //  }

    //  if (_model.SelectedServer.VersionInfo.AgentName == null)
    //    _versionText.Markdown = "";
    //  else
    //    _versionText.Markdown = _model.SelectedServer?.VersionInfo?.AgentName;
    //}

    //async Task LoadChannelInfo()
    //{
    //  if (_model.SelectedChannel == null)
    //    return;

    //  var c = _model.SelectedChannel;
    //  var i = _model.SelectedChannel.Info;
    //  var s = _model.SelectedChannel.Status;
    //  var t = _model.SelectedChannel.Track;

    //  var data = new object[] {
    //    i.Name,
    //    i.Genre,
    //    i.Desc,
    //    i.Url,
    //    i.Comment,
    //    c.ChannelId,
    //    s.Uptime,
    //    i.Bitrate,
    //    t.Name,
    //    t.Album,
    //    t.Creator,
    //    t.Genre,
    //    t.Url
    //  };

    //  Debug.Assert(data.Length == _channelInfoTextEntries.Count);
    //  for (int j = 0; j < data.Length; j++) {
    //    _channelInfoTextEntries[j].Text = data[j].ToString();
    //  }
    //}

    //async Task LoadChannels()
    //{
    //  int r = _channelList.SelectedRow;
    //  _channelList.Items.Clear();
    //  int i = 0;
    //  foreach (var ch in _model.Channels) {
    //    _channelList.Items.Add(i, ch.Info.Name);
    //  }
    //  _channelList.SelectRow(r);
    //}

  }
}
