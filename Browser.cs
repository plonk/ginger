﻿using System;
using Xwt;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Net;

namespace ginger
{
  // 「ブラウザ」ウィンドウ
  public partial class Browser
      : Window
  {
    BrowserContext _context;
    bool _updating;

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

      _serversMenuItem.Clicked += (sender, e) => {
        var dialog = new PrefsDialog(_context);
        dialog.Run(_context.Window);
        UpdateView();
      };

      _aboutMenuItem.Clicked += (sender, e) => {
        MessageDialog.ShowMessage(this, "バージョン情報",
          ginger.VersionString + "\n\n" + "Json.NET");
      };

      _reloadButton.Clicked += async (sender, e) => {
        await UpdateAsync();
      };

      ReloadComboBox();

      _comboBox.SelectionChanged += async (sender, e) => {
        if (_updating)
          return;

        _context.Server = (Server) _comboBox.SelectedItem;
        UpdateView();
        await UpdateAsync();
      };

      _editServersButton.Clicked += (sender, e) => {
        var dialog = new PrefsDialog(_context);
        dialog.Run(_context.Window);
        UpdateView();
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

      // 登録されたサーバーがなかった場合。
      if (_context.Ginger.Servers.Count == 0) {
        var tmp = MessageDialog.RootWindow;
        MessageDialog.RootWindow = _context.Window;
        var response = MessageDialog.AskQuestion("おっと。サーバーが１つも登録されていません。7144ポートのローカルホストを登録しますか？", Command.Yes, Command.No);
        MessageDialog.RootWindow = tmp;
        if (response == Command.Yes) {
          var server = new Server("ローカルホスト", "localhost", 7144);
          _context.Ginger.Servers.Add(server);
          _context.Ginger.SaveSettings();
          _context.Server = server;
        }
        UpdateView();
        Application.Invoke(() => {
          UpdateAsync().RunSynchronously();
        });
      }
      else {
        _context.Server = _context.Ginger.Servers[0];
        UpdateView();
        Application.Invoke(() => {
          UpdateAsync().RunSynchronously();
        });
      }
    }

    void ReloadComboBox()
    {
      _comboBox.Items.Clear();
      foreach (var server in _context.Ginger.Servers) {
        _comboBox.Items.Add(server, server.Name);
      }
    }

    void UpdateView()
    {
      _updating = true;

      ReloadComboBox();
      if (!_context.Ginger.Servers.Contains(_context.Server)) {
        _context.Server = null;
      }

      _comboBox.SelectedItem = _context.Server;

      if (_comboBox.SelectedIndex == -1) {
        _notebook.Hide();
        _messageArea.Show();
      }
      else {
        _notebook.Show();
        _messageArea.Hide();
      }

      _updating = false;
    }

    // 更新処理
    async Task UpdateAsync()
    {
      var dataview = _notebook.CurrentTab.Child as ServerView;
      if (dataview != null) {
        try {
          _statusLabel.Text = "更新開始……";
          var t = DateTime.Now;
          await dataview.UpdateAsync();
          var msec = (int) (DateTime.Now - t).TotalMilliseconds;
          _statusLabel.Text = $"更新開始……完了 ({msec}ms)";
        }
//        catch (WebException e) {
//          MessageDialog.ShowError(this, "エラー", e.Message);
//          _context.Server = null;
//          UpdateView();
//          _statusLabel.Text = "エラー";
//          throw;
//        }
        catch (Exception e) {
          MessageDialog.ShowError(this, "エラー", e.Message);
          _context.Server = null;
          UpdateView();
          _statusLabel.Text = $"{e.Message}";
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
