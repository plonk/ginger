using System;
using Xwt;

namespace ginger
{
  public class PrefsDialog
  : Dialog
  {
    BrowserContext _context;
    ListBox _listBox;
    TextEntry _hostname;
    SpinButton _port;
    Button _addButton;
    Button _deleteButton;

    public PrefsDialog(BrowserContext context)
      : base()
    {
      _context = context;

      var vbox = new VBox();
      vbox.WidthRequest = 300;

      var hbox = new HBox();
      _listBox = new ListBox();
      PopulateListBox();
      var commandBox = CommandBox();
      hbox.PackStart(_listBox, true, true);
      hbox.PackStart(commandBox, false, WidgetPlacement.Center);

      vbox.PackStart(hbox);

      Content = vbox;
    }

    void PopulateListBox()
    {
      foreach (var server in _context.Ginger.Servers) {
        string name = $"{server.Hostname}:{server.Port}";
        _listBox.Items.Add(server, name);
      }
    }

    Widget CommandBox()
    {
      var vbox = new VBox() { WidthRequest = 80 };

      _addButton = new Button("追加");
      _deleteButton = new Button("削除");

      vbox.PackStart(_addButton);
      vbox.PackStart(_deleteButton);

      return vbox;
    }
  }
}

