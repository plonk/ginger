using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xwt;
using System.Diagnostics;

namespace ginger
{
  public class ChannelInfoPage
    : VBox, ChannelView
  {
    TextEntry _nameTextEntry        = new TextEntry();
    TextEntry _genreTextEntry       = new TextEntry();
    TextEntry _descTextEntry        = new TextEntry();
    TextEntry _urlTextEntry         = new TextEntry();
    TextEntry _commentTextEntry     = new TextEntry();
    TextEntry _channelIdTextEntry   = new TextEntry();
    TextEntry _contentTypeTextEntry = new TextEntry();
    TextEntry _uptimeTextEntry      = new TextEntry();
    TextEntry _bitrateTextEntry     = new TextEntry();
    TextEntry _trackNameTextEntry   = new TextEntry();
    TextEntry _albumTextEntry       = new TextEntry();
    TextEntry _creatorTextEntry     = new TextEntry();
    TextEntry _trackGenreTextEntry  = new TextEntry();
    TextEntry _trackUrlTextEntry    = new TextEntry();

    List<TextEntry> _entries;

    Server Server;
    string ChannelId;

    public ChannelInfoPage()
      : base()
    {
      Margin = 10;
      var table = new Table();
      table.SetColumnSpacing(1, 10);

      var labels = new List<string>() {
        "チャンネル名",
        "ジャンル",
        "概要",
        "コンタクトURL",
        "配信者コメント",
        "チャンネルID",
        "形式",
        "ビットレート",
        "配信・リレー時間",
        "タイトル",
        "アルバム",
        "アーティスト",
        "ジャンル",
        "URL"
      };
      _entries = new List<TextEntry>() {
        _nameTextEntry,
        _genreTextEntry,
        _descTextEntry,
        _urlTextEntry,
        _commentTextEntry,
        _channelIdTextEntry,
        _contentTypeTextEntry,
        _bitrateTextEntry,
        _uptimeTextEntry,
        _trackNameTextEntry,
        _albumTextEntry,
        _creatorTextEntry,
        _trackGenreTextEntry,
        _trackUrlTextEntry,
      };
      for (int i = 0; i < 9; i++) {
        table.Add(new Label(labels[i]),
          0, i,
          1, 1,
          false, false,
          WidgetPlacement.End);
        table.Add(_entries[i],
          1, i,
          1, 1,  // rowspan, colspan
          true); // hexpand
      }

      PackStart(table, true, true);

      var trackTable = new Table();
      trackTable.SetColumnSpacing(1, 10);

      for (int i = 9; i < labels.Count; i++) {
        trackTable.Add(new Label(labels[i]),
          0, i,
          1, 1,
          false, false,
          WidgetPlacement.End);
        trackTable.Add(_entries[i],
          1, i,
          1, 1,  // rowspan, colspan
          true); // hexpand
      }

      var expander = new Expander();
      expander.Label = "トラック情報";
      expander.Content = trackTable;
      PackStart(expander);

      var applyButton = new Button("適用") { WidthRequest = 80 };
      applyButton.Clicked += async (sender, e) => {
        await SetChannelInfo();
      };
      PackStart(applyButton, false, WidgetPlacement.Center, WidgetPlacement.End);
    }

    async Task SetChannelInfo()
    {
      await Server.SetChannelInfoAsync(ChannelId, BuildInfo(), BuildTrack());
    }

    ChannelInfo BuildInfo()
    {
      return new ChannelInfo() {
        Name = _nameTextEntry.Text,
        Url = _urlTextEntry.Text,
        Genre = _genreTextEntry.Text,
        Desc = _descTextEntry.Text,
        Comment = _commentTextEntry.Text,
      };
    }

    Track BuildTrack()
    {
      return new Track() {
        Name = _trackNameTextEntry.Text,
        Genre = _trackGenreTextEntry.Text,
        Album = _albumTextEntry.Text,
        Creator = _creatorTextEntry.Text,
        Url = _trackUrlTextEntry.Text,
      };
    }

    void UpdateEntries(string channelId, ChannelInfo i, Track t, ChannelStatus s)
    {
      var values = new string[] {
        i.Name,
        i.Genre,
        i.Desc,
        i.Url,
        i.Comment,
        channelId,
        i.ContentType,
        i.Bitrate.ToString(),
        s.Uptime.ToString(),
        t.Name,
        t.Album,
        t.Creator,
        t.Genre,
        t.Url
      };
      for (int j = 0; j < _entries.Count; j++) {
        _entries[j].Text = values[j];
      }
    }

    void ClearEntries()
    {
      for (int j = 0; j < _entries.Count; j++) {
        _entries[j].Text = "";
      }
    }

    async Task ChannelView.UpdateAsync(Server server, string channelId)
    {
      Server = server;
      ChannelId = channelId;

      if (channelId != null) {
        var result = await server.GetChannelInfoAsync(channelId);
        var status = await server.GetChannelStatusAsync(channelId);

        UpdateEntries(channelId, result.Info, result.Track, status);

      } else {
        ClearEntries();
      }
    }
  }
}
