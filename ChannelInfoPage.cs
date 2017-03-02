using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xwt;
using System.Diagnostics;

namespace ginger
{
  public class ChannelInfoPage
    : Table, ChannelView
  {
    List<TextEntry> _channelInfoTextEntries = new List<TextEntry>();

    public ChannelInfoPage()
      : base()
    {
      Margin = 10;
      SetColumnSpacing(1, 10);

      var labels = new List<string>() {
        "チャンネル名",
        "ジャンル",
        "概要",
        "コンタクトURL",
        "配信者コメント",
        "チャンネルID",
        "配信・リレー時間",
        "ビットレート",
        "トラックタイトル",
        "アルバム",
        "アーティスト",
        "トラックジャンル",
        "トラックURL"
      };
      for (int i = 0; i < labels.Count; i++) {
        Add(new Label(labels[i]), 0, i);
        var entry = new TextEntry();
        Add(entry, 1, i);
        _channelInfoTextEntries.Add(entry);
      }

      var apply = new Button("適用");
      Add(apply, 1, labels.Count + 1);
    }

    async Task ChannelView.UpdateAsync(Server server, string channelId)
    {
      if (channelId != null) {
        var result = await server.GetChannelInfoAsync(channelId);
        var i = result.Info;
        var t = result.Track;
        var s = await server.GetChannelStatusAsync(channelId);
        var values = new string[] {
          i.Name,
          i.Genre,
          i.Desc,
          i.Url,
          i.Comment,
          channelId,
          s.Uptime.ToString(),
          i.Bitrate.ToString(),
          t.Name,
          t.Album,
          t.Creator,
          t.Genre,
          t.Url
        };
        // i.ContentType, i.MimeType
        for (int j = 0; j < values.Length; j++) {
          _channelInfoTextEntries[j].Text = values[j];
        }
      } else {
        for (int j = 0; j < _channelInfoTextEntries.Count; j++) {
          _channelInfoTextEntries[j].Text = "";
        }
      }
    }
  }
}
