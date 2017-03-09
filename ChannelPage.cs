using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xwt;
using System.Diagnostics;

namespace ginger
{
  // ノートブックの「チャンネル」ページ
  public class ChannelPage
    : VBox, ServerView
  {
    BrowserContext _context;
    ListBox _channelListBox;
    Notebook _channelPropertiesNotebook;
    int SelectedRow;

    bool _isUpdating = false;

    public ChannelPage(BrowserContext context)
    {
      _context = context;

      Margin = 10;
      Spacing = 10;
      var hbox = new HBox();

      _channelListBox = new ListBox();
      SelectedRow = _channelListBox.SelectedRow;
      _channelListBox.SelectionChanged += async (sender, e) => {
        if (_channelListBox.SelectedRow != SelectedRow) {
          SelectedRow = _channelListBox.SelectedRow;
          _context.Channel = (Channel)_channelListBox.SelectedItem;

          if (!_isUpdating) {
            await UpdateChannelPropertiesAsync();
          }
        }
      };

      var cmds = CommandBox();

      hbox.PackStart(_channelListBox, true);
      hbox.PackStart(cmds);

      PackStart(hbox);

      _channelPropertiesNotebook = ChannelPropertiesNotebook();
      _channelPropertiesNotebook.CurrentTabChanged += async (sender, e) => {
        await UpdateChannelPropertiesAsync();
      };

      PackStart(_channelPropertiesNotebook, true, true);
    }

    Notebook ChannelPropertiesNotebook()
    {
      var nb = new Notebook();

      nb.Add(new ChannelInfoPage(_context), "チャンネル情報");
      nb.Add(new ConnectionsPage(_context), "接続");
      nb.Add(new RelayTreePage(_context), "リレーツリー");
      nb.Add(new TrackInfoPage(_context), "トラック情報");
      return nb;
    }

    Widget CommandBox()
    {
      var vbox = new VBox { WidthRequest = 80 };

      var playButton = new Button("再生");
      playButton.Clicked += (sender, e) => {
        var channel = (Channel)_channelListBox.SelectedItem;

        if (channel != null) {
          var hostname = _context.Server.Hostname;
          var port = _context.Server.Port;
          var id = channel.ChannelId;

          Process.Start($"http://{hostname}:{port}/pls/{id}.m3u");
        }
      };
      var disconnectButton = new Button("切断");
      disconnectButton.Clicked += async (sender, e) => {
        var channel = (Channel)_channelListBox.SelectedItem;
        if (channel != null) {
          await _context.Server.StopChannelAsync(channel.ChannelId);
          await UpdateAsync();
        }
      };
      var reconnectButton = new Button("再接続");
      reconnectButton.Clicked += async (sender, e) => {
        var channel = (Channel)_channelListBox.SelectedItem;
        if (channel != null) {
          try {
            await _context.Server.BumpChannelAsync(channel.ChannelId);
          } catch (Exception ex) {
            MessageDialog.ShowError(_context.Window, "エラー", ex.Message);
            throw;
          }
          await UpdateAsync();
        }
      };
      var broadcastButton = new Button("配信...");
      broadcastButton.Sensitive = false;
      broadcastButton.TooltipText = "未実装です。";

      foreach (var w in new Widget[] { playButton, disconnectButton, reconnectButton, broadcastButton }) {
        vbox.PackStart(w);
      }
      return vbox;
    }

    async Task UpdateChannelPropertiesAsync()
    {
      try {
        var page = _channelPropertiesNotebook.CurrentTab.Child as ChannelView;

        if (page != null) {
          await page.UpdateAsync();
        }
      }
      catch (Exception e) {
        MessageDialog.ShowError(_context.Window, "エラー", e.Message);
        throw;
      } 
    }

    public async Task UpdateAsync()
    {
      _isUpdating = true;

      var channels = await _context.Server.GetChannelsAsync();

      // チャンネルリストを更新。
      int row = _channelListBox.SelectedRow;
      _channelListBox.Items.Clear();
      foreach (var channel in channels) {
        _channelListBox.Items.Add(channel, channel.Info.Name);
      }
      if (row >= 0)
        _channelListBox.SelectRow(row);

      // チャンネルのプロパティのノートブックを更新。
      await UpdateChannelPropertiesAsync();

      _isUpdating = false;
    }
  }
}
