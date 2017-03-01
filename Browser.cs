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
    public Browser(Ginger ginger)
      : base()
    {
      Build();
    }

    //public string Title {
    //  get {
    //    if (SelectedServer != null) {
    //      return string.Format("{0}で稼働中の{1} - ginger", SelectedServer.ToString(), SelectedServer.VersionInfo.AgentName);
    //    } else {
    //      return "ginger";
    //    }
    //  }
    //}

    public Server SelectedServer;

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
      if (SelectedServer != null) {
        Process.Start($"http://{SelectedServer.Hostname}:{SelectedServer.Port}/");
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
