using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using Xwt;

namespace ginger
{
  public class ChannelInfoPage
    : VBox, ChannelView
  {
    BrowserContext _context;
    TextEntry _name = new TextEntry();
    TextEntry _genre = new TextEntry();
    TextEntry _desc = new TextEntry();
    TextEntry _url = new TextEntry();
    TextEntry _comment = new TextEntry();
    Label _channelId = new Label();
    Label _contentType = new Label();
    Label _uptime = new Label();
    Label _bitrate = new Label();

    List<Widget> _entries;

    public ChannelInfoPage(BrowserContext context)
      : base()
    {
      _context = context;

      Margin = 10;
      var table = new Table();
      table.SetColumnSpacing(1, 10);

      var labels = new List<string>() {
        "名前",
        "ジャンル",
        "概要",
        "コンタクト",
        "コメント",
        "ID",
        "形式",
        "ビットレート",
        "稼動時間",

      };
      _entries = new List<Widget>() {
        _name,
        _genre,
        _desc,
        _url,
        _comment,
        _channelId,
        _contentType,
        _bitrate,
        _uptime,

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

      var applyButton = new Button("適用") { WidthRequest = 80 };
      applyButton.Clicked += async (sender, e) => {
        await SetChannelInfo();
      };
      PackStart(applyButton, false, WidgetPlacement.Center, WidgetPlacement.End);
    }

    async Task SetChannelInfo()
    {
      await _context.Server.SetChannelInfoAsync(_context.Channel.ChannelId, BuildInfo(), _context.Channel.Track);
    }

    ChannelInfo BuildInfo()
    {
      return new ChannelInfo() {
        Name = _name.Text,
        Url = _url.Text,
        Genre = _genre.Text,
        Desc = _desc.Text,
        Comment = _comment.Text,
      };
    }

    void SetText(Widget widget, string text)
    {
      if (widget is TextEntry)
        ((TextEntry) widget).Text = text;
      else if (widget is Label)
        ((Label) widget).Text = text;
      else
        throw new ArgumentOutOfRangeException("widget");
    }

    void UpdateEntries(string channelId, ChannelInfo i, ChannelStatus s)
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

      };
      for (int j = 0; j < _entries.Count; j++) {
        SetText(_entries[j], values[j]);
      }
    }

    void ClearEntries()
    {
      for (int j = 0; j < _entries.Count; j++) {
        SetText(_entries[j], "");
      }
    }

    async Task ChannelView.UpdateAsync()
    {
      if (_context.Channel != null) {
        UpdateEntries(_context.Channel.ChannelId,
          _context.Channel.Info, 
          _context.Channel.Status);
      }
      else {
        ClearEntries();
      }
    }
  }
}
