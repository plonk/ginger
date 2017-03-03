using System;
using Xwt;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ginger
{
  // 「ブラウザ」ウィンドウ
  public partial class Browser
      : Window
  {
    // 現在選択中のサーバー
    public Server Server;

    public Browser(Ginger ginger)
      : base()
    {
      Build();

      Title = "ginger";

      _browserMenuItem.Clicked += (sender, e) => {
        if (Server == null) {
          MessageDialog.ShowError(this, "先にサーバーを選んでね。");
          return;
        }
        System.Diagnostics.Process.Start($"http://{Server.Hostname}:{Server.Port}/");
      };

      _quitMenuItem.Clicked += (sender, e) => {
        ginger.RequestExit();
      };

      _aboutMenuItem.Clicked += (sender, e) => {
        MessageDialog.ShowMessage(this, "バージョン情報", ginger.VersionString);
      };

      _reloadButton.Clicked += async (sender, e) => {
        await UpdateAsync();
      };

      foreach (var server in ginger.KnownServers) {
        _comboBox.Items.Add(server, $"{server.Hostname}:{server.Port}");
      }

      _comboBox.SelectionChanged += async (sender, e) => {
        Server = (Server)_comboBox.SelectedItem;
        UpdateView();
        await UpdateAsync();
      };

      _notebook.CurrentTabChanged += async (sender, e) => {
        await UpdateAsync();
      };

      _messageArea.Text = "サーバーを選んでくださいです。";
      UpdateView();
    }

    void UpdateView()
    {
      if (_comboBox.SelectedIndex == -1) {
        _notebook.Hide();
        _messageArea.Show();
      }
      else {
        _notebook.Show();
        _messageArea.Hide();
      }
    }

    async Task UpdateAsync()
    {
      var dataview = _notebook.CurrentTab.Child as ServerView;
      if (dataview != null) {
        try {
          _statusLabel.Text = "更新中...";
          var t = DateTime.Now;
          await dataview.UpdateAsync(Server);
          var msec = (DateTime.Now - t).Milliseconds;
          _statusLabel.Text = $"更新完了 ({msec}ms)";
        }
        catch (Exception e) {
          MessageDialog.ShowError(this, "エラー", e.Message);
          throw;
        }
      }
      else {
        _statusLabel.Text = "更新することないです。";
      }

      if (Server != null) {
        Title = $"{Server.Hostname}:{Server.Port} - ginger";
      }
      else {
        Title = "ginger";
      }
    }

    public void OpenInBrowser()
    {
      if (Server != null) {
        Process.Start($"http://{Server.Hostname}:{Server.Port}/");
      }
    }
  }
}
