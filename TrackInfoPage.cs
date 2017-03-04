using System;
using Xwt;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ginger
{
  public class TrackInfoPage : VBox, ChannelView
  {
    BrowserContext _context;

    TextEntry _name = new TextEntry();
    TextEntry _album = new TextEntry();
    TextEntry _creator = new TextEntry();
    TextEntry _genre = new TextEntry();
    TextEntry _url = new TextEntry();

    List<Widget> _entries;

    public TrackInfoPage(BrowserContext context)
    {
      _context = context;
      Margin = 10;

      var trackTable = new Table();
      trackTable.SetColumnSpacing(1, 10);

      var labels = new List<string>() {
        "タイトル",
        "アルバム",
        "アーティスト",
        "ジャンル",
        "URL"
      };
      _entries = new List<Widget>() {
        _name,
        _album,
        _creator,
        _genre,
        _url,
      };

      for (int i = 0; i < labels.Count; i++) {
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

      PackStart(trackTable);

      var applyButton = new Button("適用") { WidthRequest = 80 };
      applyButton.Clicked += async (sender, e) => {
        await SetChannelInfo();
      };
      PackStart(applyButton, false, WidgetPlacement.Center, WidgetPlacement.End);
    }

    async Task SetChannelInfo()
    {
      MessageDialog.ShowMessage(BuildTrack().Name);
      await _context.Server.SetChannelInfoAsync(_context.Channel.ChannelId, _context.Channel.Info, BuildTrack());
    }

    Track BuildTrack()
    {
      return new Track() {
        Name = _name.Text,
        Genre = _genre.Text,
        Album = _album.Text,
        Creator = _creator.Text,
        Url = _url.Text,
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

    void UpdateEntries(Track t)
    {
      var values = new string[] {
        t.Name,
        t.Album,
        t.Creator,
        t.Genre,
        t.Url
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
        var result = await _context.Server.GetChannelInfoAsync(_context.Channel.ChannelId);
        UpdateEntries(result.Track);
      }
      else {
        ClearEntries();
      }
    }
  }
}

