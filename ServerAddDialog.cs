using System;
using Xwt;

namespace ginger
{
  public class ServerAddDialog
    : Xwt.Dialog
  {
    public ServerAddDialog()
    {
      Build();
    }

    TextEntry _hostnameEntry;
    SpinButton _portSpinButton;

    public String Hostname { get { return _hostnameEntry.Text; } }

    public int Port { get { return (int)_portSpinButton.Value; } }

    void Build()
    {
      var vbox = new VBox();
      {
        vbox.PackStart(new Label("あらら。サーバーが登録されていません。\nサーバーを追加してください。"));
        var hbox = new HBox();
        {
          _hostnameEntry = new TextEntry();
          _hostnameEntry.Text = "localhost";
          hbox.PackStart(_hostnameEntry);
          {
            _portSpinButton = new SpinButton();
            _portSpinButton.Digits = 0;
            _portSpinButton.IncrementValue = 1;
            _portSpinButton.MaximumValue = 65535;
            _portSpinButton.Value = 7144;
            hbox.PackStart(_portSpinButton);
          }
        }
        vbox.PackStart(hbox);
      }
      this.Content = vbox;
      this.Buttons.Add(new Command[] { Command.Add, Command.Cancel });
    }
  }
}

