using System;
using Xwt;

namespace ginger
{
  public class PrefsDialog
  : Dialog
  {
    BrowserContext _context;
    ListBox _listBox;
    TextEntry _name = new TextEntry();
    TextEntry _hostname = new TextEntry();
    SpinButton _port = new SpinButton { MaximumValue = 65535, IncrementValue = 1.0, Digits = 0 };
    Button _addButton;
    Button _deleteButton;

    public PrefsDialog(BrowserContext context)
    {
      _context = context;

      var vbox = new VBox();
      vbox.WidthRequest = 300;

      var hbox = new HBox();
      _listBox = new ListBox();

      _listBox.SelectionChanged += (sender, e) => {
        LoadFields();
      };

      PopulateListBox();

      var commandBox = CommandBox();
      hbox.PackStart(_listBox, true, true);
      hbox.PackStart(commandBox, false, WidgetPlacement.Center);

      vbox.PackStart(hbox);

      var table = new Table();

      table.Add(new Label("名前"), 0, 0);
      table.Add(new Label("ホスト名"), 0, 1);
      table.Add(new Label("ポート番号"), 0, 2);

      table.Add(_name, 1, 0, 1, 1, true, true);
      table.Add(_hostname, 1, 1, 1, 1, true, true);
      table.Add(_port, 1, 2, 1, 1, false, false, WidgetPlacement.Start);

      vbox.PackStart(table);

      var okButton = new DialogButton(Command.Ok);
      okButton.Clicked += (sender, e) => Respond(Command.Ok);
      var cancelButton = new DialogButton(Command.Cancel);
      cancelButton.Clicked += (sender, e) => Respond(Command.Cancel);
      Buttons.Add(okButton);
      Buttons.Add(cancelButton);
      Content = vbox;

      LoadFields();
    }

    void PopulateListBox()
    {
      foreach (var server in _context.Ginger.Servers) {
        string name = $"{server.Hostname}:{server.Port}";
        _listBox.Items.Add(server, name);
      }
    }

    void LoadFields()
    {
      var server = (Server) _listBox.SelectedItem;
      if (server == null) {
        _name.Text = "";
        _hostname.Text = "";
        _port.Value = 0;
        return;
      }
      else {
        _name.Text = server.Name;
        _hostname.Text = server.Hostname;
        _port.Value = server.Port;
      }
    }

    Widget CommandBox()
    {
      var vbox = new VBox { WidthRequest = 80 };

      _addButton = new Button("追加");
      _deleteButton = new Button("削除");

      _addButton.Clicked += (sender, e) => {
        var server = new Server("新規サーバー", "", 7144);
        _listBox.Items.Add(server, "新規サーバー");
        _listBox.SelectRow(_listBox.Items.Count - 1);
      };

      vbox.PackStart(_addButton);
      vbox.PackStart(_deleteButton);

      return vbox;
    }
  }
}

