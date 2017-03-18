using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xwt;
using System.Diagnostics;

namespace ginger
{
  // サーバーノートブックのチャンネルページ。
  public class ChannelPage
    : VBox, ServerView
  {
    BrowserContext _context;
    ListBox _channelListBox;
    Notebook _channelNotebook;
    bool _isUpdating = false;

    public ChannelPage(BrowserContext context)
    {
      _context = context;

      Margin = 10;
      Spacing = 10;

      // チャンネルリストとコマンドボックス。
      var hbox = new HBox();
      _channelListBox = new ListBox();
      _channelListBox.SelectionChanged += async (sender, e) => {
        _context.Channel = (Channel) _channelListBox.SelectedItem;

        if (!_isUpdating) {
          await UpdateNotebookAsync();
        }
      };
      var cmds = CommandBox();
      hbox.PackStart(_channelListBox, true);
      hbox.PackStart(cmds);
      PackStart(hbox);

      // チャンネルプロパティのノートブック。
      _channelNotebook = ChannelNotebook();
      _channelNotebook.CurrentTabChanged += async (sender, e) => {
        await UpdateNotebookAsync();
      };
      PackStart(_channelNotebook, true, true);
    }

    // チャンネルプロパティのノートブックを作成。
    Notebook ChannelNotebook()
    {
      var nb = new Notebook();

      nb.Add(new ChannelInfoPage(_context), "チャンネル情報");
      nb.Add(new ConnectionsPage(_context), "接続一覧");
      nb.Add(new RelayTreePage(_context), "リレーツリー");
      nb.Add(new TrackInfoPage(_context), "トラック情報");
      return nb;
    }

    // チャンネル操作のコマンドボタンボックスを作成。
    Widget CommandBox()
    {
      var vbox = new VBox { WidthRequest = 80 };

      // 再生
      var play = new Button("再生");
      play.Clicked += (sender, e) => {
        var channel = (Channel) _channelListBox.SelectedItem;

        if (channel != null) {
          var hostname = _context.Server.Hostname;
          var port = _context.Server.Port;
          var id = channel.ChannelId;

          Process.Start($"http://{hostname}:{port}/pls/{id}.m3u");
        }
      };
      // 切断
      var disconnect = new Button("切断");
      disconnect.Clicked += async (sender, e) => {
        var channel = (Channel) _channelListBox.SelectedItem;
        if (channel != null) {
          await _context.Server.StopChannelAsync(channel.ChannelId);
          await UpdateAsync();
        }
      };
      // 再接続
      var reconnect = new Button("再接続");
      reconnect.Clicked += async (sender, e) => {
        var channel = (Channel) _channelListBox.SelectedItem;
        if (channel != null) {
          try {
            await _context.Server.BumpChannelAsync(channel.ChannelId);
          }
          catch (Exception ex) {
            MessageDialog.ShowError(_context.Window, "エラー", ex.Message);
            throw;
          }
          await UpdateAsync();
        }
      };
      // 配信
      var broadcast = new Button("配信...");
      broadcast.Sensitive = false;
      broadcast.TooltipText = "未実装です。";

      foreach (var w in new Widget[] { play, disconnect, reconnect, broadcast }) {
        vbox.PackStart(w);
      }
      return vbox;
    }

    // チャンネルプロパティのノートブックを更新。
    async Task UpdateNotebookAsync()
    {
      try {
        var page = _channelNotebook.CurrentTab.Child as ChannelView;

        if (page != null) {
          await page.UpdateAsync();
        }
      }
      catch (Exception e) {
        MessageDialog.ShowError(_context.Window, "エラー", e.Message);
        throw;
      } 
    }

    // チャンネルリストを更新。
    void UpdateChannelListBox(Channel[] channels)
    {
      int row = _channelListBox.SelectedRow;
      _channelListBox.Items.Clear();
      foreach (var channel in channels) {
        _channelListBox.Items.Add(channel, channel.Info.Name);
      }
      if (row >= 0)
        _channelListBox.SelectRow(row);
    }

    // ServerView インターフェイスの実装。
    public async Task UpdateAsync()
    {
      _isUpdating = true;

      var channels = await _context.Server.GetChannelsAsync();
      UpdateChannelListBox(channels);
      await UpdateNotebookAsync();

      _isUpdating = false;
    }
  }
}
