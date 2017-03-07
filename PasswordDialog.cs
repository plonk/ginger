using System;
using Xwt;

namespace ginger
{
  public class PasswordDialog : Dialog
  {
    TextEntry _username;
    PasswordEntry _password;

    public string Username { get { return _username.Text; } }
    public string Password { get { return _password.Password; } }

    public PasswordDialog()
    {
      var table = new Table();

      _username = new TextEntry();
      _password = new PasswordEntry() { WidthRequest = 220 };

      table.Add(new Label("認証ID"), 0, 0);
      table.Add(new Label("パスワード"), 0, 1);

      table.Add(_username, 1, 0);
      table.Add(_password, 1, 1);

      var ok = new DialogButton(Command.Ok);
      var cancel = new DialogButton(Command.Cancel);

      ok.Clicked += (sender, e) => {
        Respond(Command.Ok);
        Close();
      };
      cancel.Clicked += (sender, e) => {
        Respond(Command.Cancel);
        Close();
      };
      Buttons.Add(ok);
      Buttons.Add(cancel);

      Content = table;
    }
  }
}

