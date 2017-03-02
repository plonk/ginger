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
    ListBox _channelListBox;
    Notebook _channelPropertiesNotebook;
    ChannelInfoPage _channelInfoPage;
    Server Server;

    public ChannelPage()
    {
      Margin = 10;
      Spacing = 10;
      var hbox = new HBox();

      _channelListBox = new ListBox();
      _channelListBox.SelectionChanged += async (sender, e) => {
        await UpdateChannelPropertiesAsync(Server);
      };

      var cmds = CommandBox();

      hbox.PackStart(_channelListBox, true);
      hbox.PackStart(cmds);

      PackStart(hbox);

      _channelPropertiesNotebook = ChannelPropertiesNotebook();
      _channelPropertiesNotebook.CurrentTabChanged += async (sender, e) => {
        await UpdateChannelPropertiesAsync(Server);
      };

      PackStart(_channelPropertiesNotebook);
    }

    Notebook ChannelPropertiesNotebook()
    {
      var nb = new Notebook();

      _channelInfoPage = new ChannelInfoPage();

      nb.Add(_channelInfoPage, "チャンネル情報");
      nb.Add(new ConnectionsPage(), "接続");
      nb.Add(new RelayTreePage(), "リレーツリー");
      return nb;
    }

    Widget CommandBox()
    {
      var vbox = new VBox() { WidthRequest = 80 };

      var p = new Button("再生");
      p.Clicked += (sender, e) => {
        var channel = (Channel) _channelListBox.SelectedItem;
        if (channel != null)
          MessageDialog.AskQuestion($"{channel.Info.Name}を再生しようぜーっ", new Command[] { Command.Ok });
      };
      var d = new Button("切断");
      d.Clicked += (sender, e) => {
        var channel = (Channel) _channelListBox.SelectedItem;
        if (channel != null)
          MessageDialog.AskQuestion($"{channel.Info.Name}を切断しようぜーっ", new Command[] { Command.Ok });
      };
      var r = new Button("再接続");
      r.Clicked += (sender, e) => {
        var channel = (Channel) _channelListBox.SelectedItem;
        if (channel != null)
          MessageDialog.AskQuestion($"{channel.Info.Name}を再接続しようぜーっ", new Command[] { Command.Ok });
      };
      var b = new Button("配信...");
      b.Clicked += (sender, e) => {
        MessageDialog.AskQuestion($"磯野ー！配信しようぜー！", new Command[] { Command.Ok, Command.Cancel });
      };

      foreach (var w in new Widget[] { p, d, r, b }) {
        vbox.PackStart(w);
      }
      return vbox;
    }

    async Task UpdateChannelPropertiesAsync(Server server)
    {
      var page = _channelPropertiesNotebook.CurrentTab.Child as ChannelView;

      if (page != null)
        await page.UpdateAsync(server, ((Channel) _channelListBox.SelectedItem)?.ChannelId);
    }

    async Task ServerView.UpdateAsync(Server server)
    {
      Server = server;
      var channels = await server.GetChannelsAsync();

      // チャンネルリストを更新。
      _channelListBox.Items.Clear();
      foreach (var channel in channels) {
        _channelListBox.Items.Add(channel, channel.Info.Name);
      }

      // チャンネルのプロパティのノートブックを更新。
      await UpdateChannelPropertiesAsync(server);
    }
  }
}
