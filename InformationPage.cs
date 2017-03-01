using System;
using System.Threading.Tasks;
using Xwt;
using System.Diagnostics;

namespace ginger
{
  public class InformationPage
    : VBox, DataView
  {
    Label _agentLabel;
    Label _uptimeLabel;
    Label _globalIpLabel;
    Label _localIpLabel;
    
    public InformationPage()
      : base()
    {
      Margin = 10;

      _agentLabel = new Label();
      PackStart(_agentLabel);

      _uptimeLabel = new Label();
      PackStart(_uptimeLabel);

      _globalIpLabel = new Label();
      PackStart(_globalIpLabel);

      _localIpLabel = new Label();
      PackStart(_localIpLabel);
    }

    static string FormatTimeSpan(TimeSpan span)
    {
      String result = "";

      if (result != "" || span.Hours != 0)
        result += $"{span.Hours}時間";
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

    async Task DataView.UpdateAsync(Server server)
    {
      var info = await server.GetVersionInfoAsync();
      _agentLabel.Text = $"ソフトウェア: {info.AgentName}";
      var status = await server.GetStatusAsync();
      _uptimeLabel.Text = $"稼働時間: {FormatTimeSpan(TimeSpan.FromSeconds(status.Uptime))}";
      _globalIpLabel.Text = $"グローバルIP: {FormatEndPoint(status.GlobalRelayEndPoint)}";
      _localIpLabel.Text = $"ローカルIP: {status.LocalRelayEndPoint}";
    }
  }
}
