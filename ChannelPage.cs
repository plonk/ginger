using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xwt;
using System.Diagnostics;

namespace ginger
{
  public class ChannelPage
    : VBox, DataView
  {
    ListBox _channelListBox;
    Notebook _channelPropertiesNotebook;
    ChannelInfoPage _channelInfoPage;

    public ChannelPage()
    {
      Margin = 10;
      Spacing = 10;
      var hbox = new HBox();

      _channelListBox = new ListBox();
      _channelListBox.SelectionChanged += (sender, e) => {
        if (_channelListBox.SelectedItem == null)
          return;

        var channel = (Channel) _channelListBox.SelectedItem;
        _channelInfoPage.ChannelId = channel.ChannelId;


      };

      var cmds = CommandBox();
      cmds.WidthRequest = 80;

      hbox.PackStart(_channelListBox, true);
      hbox.PackStart(cmds);

      PackStart(hbox);

      _channelPropertiesNotebook = ChannelPropertiesNotebook();

      PackStart(_channelPropertiesNotebook);
    }

    Notebook ChannelPropertiesNotebook()
    {
      var nb = new Notebook();

      _channelInfoPage = new ChannelInfoPage();

      nb.Add(new Label(""), "接続");
      nb.Add(_channelInfoPage, "チャンネル情報");
      nb.Add(new Label(""), "リレーツリー");
      return nb;
    }

    Widget CommandBox()
    {
      var vbox = new VBox();

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

    async Task UpdateChannelProperties(Server server)
    {
      Debug.Print("ChannelPage UpdateChannelProperties");
      var page = _channelPropertiesNotebook.CurrentTab.Child as DataView;
      Debug.Print("{0}", page ==null);

      if (page != null)
        await page.UpdateAsync(server);
    }

    async Task DataView.UpdateAsync(Server server)
    {
      Debug.Print("ChannelPage UpdateAsync");
      var channels = await server.GetChannelsAsync();

      _channelListBox.Items.Clear();
      foreach (var channel in channels) {
        _channelListBox.Items.Add(channel, channel.Info.Name);
      }
      _channelInfoPage.ChannelId = null;

      await UpdateChannelProperties(server);
    }
  }
}
