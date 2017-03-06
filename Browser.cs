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
    BrowserContext _context;

    public Browser(Ginger ginger)
    {
      _context = new BrowserContext(ginger, this);

      Icon = ginger.Icon;

      Build();

      Title = "ginger";

      _browserMenuItem.Clicked += (sender, e) => {
        if (_context.Server == null) {
          MessageDialog.ShowError(this, "先にサーバーを選んでね。");
          return;
        }
        Process.Start($"http://{_context.Server.Hostname}:{_context.Server.Port}/");
      };

      _quitMenuItem.Clicked += (sender, e) => {
        ginger.RequestExit();
      };

      _prefsMenuItem.Clicked += (sender, e) => {
        var dialog = new PrefsDialog(_context);
        dialog.Run(_context.Window);
      };

      _aboutMenuItem.Clicked += (sender, e) => {
        MessageDialog.ShowMessage(this, "バージョン情報", ginger.VersionString);
      };

      _reloadButton.Clicked += async (sender, e) => {
        await UpdateAsync();
      };

      foreach (var server in ginger.Servers) {
        _comboBox.Items.Add(server, $"{server.Hostname}:{server.Port}");
      }

      _comboBox.SelectionChanged += async (sender, e) => {
        _context.Server = (Server) _comboBox.SelectedItem;
        UpdateView();
        await UpdateAsync();
      };

      _notebook.CurrentTabChanged += async (sender, e) => {
        await UpdateAsync();
      };

      _messageArea.Text = "サーバーを選んでくださいです。";
      UpdateView();

      Func<bool> callback = null;
      callback = () => {
        if (_autoReloadButton.Active) {
          UpdateAsync().ContinueWith((prev) => {
            Application.TimeoutInvoke(1000, callback);
          });
        } else {
          Application.TimeoutInvoke(1000, callback);
        }
        return false;
      };
          
      Application.TimeoutInvoke(1000, callback);
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
          _statusLabel.Text = "更新開始……";
          var t = DateTime.Now;
          await dataview.UpdateAsync();
          var msec = (DateTime.Now - t).Milliseconds;
          _statusLabel.Text = $"更新開始……完了 ({msec}ms)";
        }
        catch (Exception e) {
          MessageDialog.ShowError(this, "エラー", e.Message);
          throw;
        }
      }
      else {
        _statusLabel.Text = "更新することないです。";
      }

      if (_context.Server != null) {
        Title = $"{_context.Server.Hostname}:{_context.Server.Port} - ginger";
      }
      else {
        Title = "ginger";
      }
    }

    public void OpenInBrowser()
    {
      if (_context.Server != null) {
        Process.Start($"http://{_context.Server.Hostname}:{_context.Server.Port}/");
      }
    }
  }
}
