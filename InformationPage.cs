using System;
using System.Threading.Tasks;
using Xwt;
using Xwt.Drawing;
using System.Diagnostics;
using System.Reflection;
using System.IO;

namespace ginger
{
  public class InformationPage
    : VBox, ServerView
  {
    Label _agentLabel;
    Label _uptimeLabel;
    Label _firewallLabel;
    Label _globalIpLabel;
    Label _localIpLabel;

    public InformationPage()
      : base()
    {
      Margin = 10;

      var table = new Table();
      table.SetColumnSpacing(1, 10);

      table.Add(new Label("ソフトウェア"), 0, 0, 1, 1, false, false, WidgetPlacement.End);
      _agentLabel = new Label();
      table.Add(_agentLabel, 1, 0);

      table.Add(new Label("稼働時間"), 0, 1, 1, 1, false, false, WidgetPlacement.End);
      _uptimeLabel = new Label();
      table.Add(_uptimeLabel, 1, 1);

      table.Add(new Label("ファイアウォール"), 0, 2, 1, 1, false, false, WidgetPlacement.End);
      _firewallLabel = new Label();
      table.Add(_firewallLabel, 1, 2);

      table.Add(new Label("グローバルIP"), 0, 3, 1, 1, false, false, WidgetPlacement.End);
      _globalIpLabel = new Label();
      table.Add(_globalIpLabel, 1, 3);

      table.Add(new Label("ローカルIP"), 0, 4, 1, 1, false, false, WidgetPlacement.End);
      _localIpLabel = new Label();
      table.Add(_localIpLabel, 1, 4);

      PackStart(table);

      var image_path = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
        "ginger.png");
      var image = Image.FromFile(image_path);
      var imageView = new ImageView(image);
      PackStart(imageView, true, WidgetPlacement.Center);
    }

    static string FormatTimeSpan(TimeSpan span)
    {
      String result = "";

      var hours = span.Days * 24 + span.Hours;

      if (hours != 0)
        result += $"{hours}時間";
      if (result != "" || span.Minutes != 0)
        result += $"{span.Minutes}分";
      if (result != "" || span.Seconds != 0)
        result += $"{span.Seconds}秒";

      return result;
    }

    static string FormatEndPoint(object[] endpoint)
    {
      if (endpoint == null)
        return "不明";
      else
        return $"{endpoint[0]}:{endpoint[1]}";
    }

    string FormatFirewallStatus(bool? isFirewalled)
    {
      if (isFirewalled.HasValue) {
        if (isFirewalled.Value)
          return "あり";
        else
          return "なし";
      } else
        return "不明";
    }

    async Task ServerView.UpdateAsync(Server server)
    {
      if (server != null) {
        var info = await server.GetVersionInfoAsync();
        _agentLabel.Text = info.AgentName;
        var status = await server.GetStatusAsync();
        _uptimeLabel.Text = FormatTimeSpan(TimeSpan.FromSeconds(status.Uptime));
        _firewallLabel.Text = FormatFirewallStatus(status.IsFirewalled);
        _globalIpLabel.Text = FormatEndPoint(status.GlobalRelayEndPoint);
        _localIpLabel.Text = FormatEndPoint(status.LocalRelayEndPoint);
      }
      else {
        _agentLabel.Text = "";
        _uptimeLabel.Text = "";
        _firewallLabel.Text = "";
        _globalIpLabel.Text = "";
        _localIpLabel.Text = "";
      }
    }
  }
}
