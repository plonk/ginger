using System;
using Xwt;
using System.Threading.Tasks;

namespace ginger
{
  public class RootServersPage
    : VBox, ServerView
  {
    BrowserContext _context;
    ListBox _listBox = new ListBox();
    Button _addButton;
    Button _deleteButton;
    Label _name = new Label();
    Label _announceUri = new Label();
    Label _channelsUri = new Label();

    public RootServersPage(BrowserContext context)
    {
      _context = context;

      Margin = 10;

      _listBox.SelectionChanged += (sender, e) => {
        UpdateLabels();
      };

      var hbox = new HBox();
      hbox.PackStart(_listBox, true, true);

      var commandBox = CommandBox();
      hbox.PackStart(commandBox, false, WidgetPlacement.Center);

      PackStart(hbox);

      var table = new Table();
      table.SetColumnSpacing(1, 10);

      table.Add(new Label("名前"), 0, 0, 1, 1, false, false, WidgetPlacement.End);
      table.Add(new Label("掲載用URL"), 0, 1, 1, 1, false, false, WidgetPlacement.End);
      table.Add(new Label("ChリストURL"), 0, 2, 1, 1, false, false, WidgetPlacement.End);

      table.Add(_name, 1, 0, 1, 1, false, false, WidgetPlacement.Start);
      table.Add(_announceUri, 1, 1, 1, 1, false, false, WidgetPlacement.Start);
      table.Add(_channelsUri, 1, 2, 1, 1, false, false, WidgetPlacement.Start);

      PackStart(table);
    }

    void UpdateLabels()
    {
      if (_listBox.SelectedItem != null) {
        var server = (YellowPage) _listBox.SelectedItem;
        _name.Text = server.Name;
        _announceUri.Text = server.AnnounceUri != null ? server.AnnounceUri : server.Uri;
        _channelsUri.Text = server.ChannelsUri;
      }
      else {
        _name.Text = _announceUri.Text = _channelsUri.Text = "";
      }
    }

    Widget CommandBox()
    {
      var vbox = new VBox {WidthRequest = 80 };

      _addButton = new Button("追加") { Sensitive = false, TooltipText = "まだ作ってありませんです。"};
      _deleteButton = new Button("削除") { Sensitive = false, TooltipText = "ないにゃあ……" };

      vbox.PackStart(_addButton);
      vbox.PackStart(_deleteButton);

      return vbox;
    }

    async Task ServerView.UpdateAsync()
    {
      if (_context.Server != null) {
        var yellowPages = await _context.Server.GetYellowPagesAsync();

        int row = _listBox.SelectedRow;
        _listBox.Items.Clear();
        foreach (var rootServer in yellowPages) {
          _listBox.Items.Add(rootServer, rootServer.Name);
        }
        if (row >= 0)
          _listBox.SelectRow(row);
      }
      else {
        _listBox.Items.Clear();
      }
    }
  }
}

