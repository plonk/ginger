using System;
using Xwt;

namespace ginger
{
  public class RootServerAddDialog
    : Dialog
  {
    Preset[] Presets = {
      new Preset {
        Name = "SP",
        Hostname = "bayonet.ddo.jp",
        Port = 7146,
        ChannelsUrl = "http://bayonet.doo.jp/sp/",
      },
      new Preset {
        Name = "Temporary yellow Pages",
        Hostname = "temp.orz.hm",
        Port = 7144,
        ChannelsUrl = "http://temp.orz.hm/yp/",
      },
      new Preset {
        Name = "Turf-Page",
        Hostname = "takami98.luna.ddns.vc",
        Port = 7144,
        ChannelsUrl = "http://peercast.takami98.net/turf-page/",
      },
    };

    public RootServerAddDialog()
    {
      Title = "イエローページの追加";
      Build();

      _hostname.Changed += (sender, e) => {
        UpdateChannelsUrl();
      };

      Buttons.GetCommandButton(Command.Ok).Clicked += (sender, e) => {
        Respond(Command.Ok);
        Close();
      };
      Buttons.GetCommandButton(Command.Cancel).Clicked += (sender, e) => {
        Respond(Command.Cancel);
        Close();
      };
    }

    public YellowPage YellowPage {
      get {
        return new YellowPage {
          Name = _name.Text,
          Uri = BuildPcpUrl(_hostname.Text, (int) _port.Value),
          AnnounceUri = BuildPcpUrl(_hostname.Text, (int) _port.Value),
          ChannelsUri = _channelsUrl.Text + "index.txt",
        };
      }
    }

    string BuildPcpUrl(string hostname, int port)
    {
      if (port == 7144)
        return $"pcp://{hostname}/";
      else
        return $"pcp://{hostname}:{port}/";
    }

    void UpdateChannelsUrl()
    {
      _channelsUrl.Text = $"http://{_hostname.Text}/";
    }

    TextEntry _name = new TextEntry() { Text = "新規イエローページ" };
    TextEntry _hostname = new TextEntry();
    SpinButton _port = new SpinButton { MaximumValue = 65535, IncrementValue = 1, Digits = 0, Value = 7144 };
    TextEntry _channelsUrl = new TextEntry();

    void LoadFromPreset(Preset preset)
    {
      _name.Text = preset.Name;
      _hostname.Text = preset.Hostname;
      _port.Value = preset.Port;
      _channelsUrl.Text = preset.ChannelsUrl;
    }

    MenuItem BuildPresetsMenuItem()
    {
      var presets = new MenuItem("プリセット");
      presets.SubMenu = new Menu();

      foreach (var preset in Presets) {
        var item = new MenuItem(preset.Name);
        item.Clicked += (sender, e) => {
          LoadFromPreset(preset);
        };
        presets.SubMenu.Items.Add(item);
      }

      return presets;
    }

    Menu BuildMainMenu()
    {
      var mainMenu = new Menu();
      mainMenu.Items.Add(BuildPresetsMenuItem());
      return mainMenu;
    }

    void Build()
    {
      MainMenu = BuildMainMenu();

      var table = new Table();

      table.Add(new Label("名前"),       0, 0, 1, 1, false, false, WidgetPlacement.End);
      table.Add(_name,                   1, 0, 1, 2, true);
      table.Add(new Label("ホスト名"),    0, 1, 1, 1, false, false, WidgetPlacement.End);
      table.Add(_hostname,               1, 1, 1, 2, true);
      table.Add(new Label("ポート"),      0, 2, 1, 1, false, false, WidgetPlacement.End);
      table.Add(_port,                   1, 2, 1, 2, false, false, WidgetPlacement.Start);
      table.Add(new Label("ChリストURL"), 0, 3, 1, 1, false, false, WidgetPlacement.End);
      table.Add(_channelsUrl,            1, 3, 1, 1, true);
      table.Add(new Label("index.txt"),  2, 3);

      this.Content = table;
      this.Buttons.Add(new Command[] { Command.Ok, Command.Cancel });
    }
  }

  class Preset {
    public string Name;
    public string Hostname;
    public int Port;
    public string ChannelsUrl;
  }
}

