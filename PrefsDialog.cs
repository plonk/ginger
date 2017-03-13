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
    SpinButton _port = new SpinButton {
      MaximumValue = 65535,
      IncrementValue = 1.0,
      Digits = 0
    };
    Button _addButton;
    Button _deleteButton;
    bool _updating;

    public PrefsDialog(BrowserContext context)
    {
      _context = context;

      var vbox = new VBox();
      vbox.WidthRequest = 300;

      var hbox = new HBox();
      _listBox = new ListBox();

      _listBox.SelectionChanged += (sender, e) => {
        if (!_updating)
          Update();
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

      _name.Changed += (sender, e) => {
        _updating = true;
        var server = ((Server) _listBox.SelectedItem);
        server.Name = _name.Text;
        int row = _listBox.SelectedRow;
        _listBox.Items.RemoveAt(row);
        _listBox.Items.Insert(row, server, server.Name);
        _listBox.SelectRow(row);
        _updating = false;
      };
      _hostname.Changed += (sender, e) => {
        var server = (Server) _listBox.SelectedItem;
        if (!_updating && server != null)
          server.Hostname = _hostname.Text;
      };
      _port.ValueChanged += (sender, e) => {
        var server = (Server) _listBox.SelectedItem;
        if (!_updating && server != null)
          server.Port = (int) _port.Value;
      };

      vbox.PackStart(table);

      var okButton = new DialogButton(Command.Ok);
      okButton.Clicked += (sender, e) => {
        _context.Ginger.Servers.Clear();
        foreach (Server server in _listBox.Items) {
          _context.Ginger.Servers.Add(server);
        }
        Respond(Command.Ok);
        Close();
      };
      Buttons.Add(okButton);
      Content = vbox;

      Update();

      Closed += (sender, e) => {
        _context.Ginger.SaveSettings();
      };
    }

    void PopulateListBox()
    {
      _listBox.Items.Clear();
      foreach (var server in _context.Ginger.Servers) {
        _listBox.Items.Add(server, server.Name);
      }
    }

    void Update()
    {
      var server = (Server) _listBox.SelectedItem;
      if (server == null) {
        _name.Text = "";
        _hostname.Text = "";
        _port.Value = 0;
        _name.Sensitive = _hostname.Sensitive = _port.Sensitive = false;
      }
      else {
        _name.Text = server.Name;
        _hostname.Text = server.Hostname;
        _port.Value = server.Port;
        _name.Sensitive = _hostname.Sensitive = _port.Sensitive = true;
      }
    }

    Widget CommandBox()
    {
      var vbox = new VBox { WidthRequest = 80 };

      _addButton = new Button("追加");
      _deleteButton = new Button("削除");
      _addButton.Clicked += (sender, e) => {
        var server = new Server("新規サーバー", "", 7144);
        _context.Ginger.Servers.Add(server);
        PopulateListBox();
        _listBox.SelectRow(_listBox.Items.Count - 1);
      };
      _deleteButton.Clicked += (sender, e) => {
        var server = (Server) _listBox.SelectedItem;
        if (server != null) {
          int row = _listBox.SelectedRow;
          _context.Ginger.Servers.Remove(server);
          PopulateListBox();
          _listBox.SelectRow(Math.Min(row, _listBox.Items.Count - 1));
        }
      };

      vbox.PackStart(_addButton);
      vbox.PackStart(_deleteButton);

      return vbox;
    }
  }
}

