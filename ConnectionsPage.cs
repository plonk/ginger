using System;
using Xwt;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ginger
{
  public class ConnectionsPage : HBox, ChannelView
  {
    ListView _listView = new ListView();
    ListStore _listStore;
    DataField<string> _protocolName = new DataField<string>();
    DataField<string> _type = new DataField<string>();
    DataField<string> _status = new DataField<string>();
    DataField<string> _remoteName = new DataField<string>();
    DataField<string> _totalRate = new DataField<string>();

    public ConnectionsPage()
    {
      Margin = 10;

      _listStore = new ListStore(_protocolName, _type, _status, _remoteName, _totalRate);
      _listView.DataSource = _listStore;
      _listView.Columns.Add("プロトコル", _protocolName);
      //_listView.Columns.Add("タイプ", _type);
      _listView.Columns.Add("状態", _status);
      _listView.Columns.Add("リモート", _remoteName);
      _listView.Columns.Add("レート", _totalRate);
        
      var scrollView = new ScrollView(_listView);
      var buttonBox = ButtonBox();

      PackStart(scrollView, true, true); // expand and fill
      PackStart(buttonBox);
    }

    Box ButtonBox()
    {
      var vbox = new VBox() { WidthRequest = 80 };

      var disconnectButton = new Button("切断");
      var reconnectButton = new Button("再接続");

      vbox.PackStart(disconnectButton);
      vbox.PackStart(reconnectButton);

      return vbox;
    }

    int BitrateKbps(Connection connection)
    {
      double recv = connection.RecvRate.HasValue ? connection.RecvRate.Value : 0.0;
      double send = connection.SendRate.HasValue ? connection.SendRate.Value : 0.0;
      return (int)Math.Round((recv + send) * 8 / 1000);
    }

    async Task ChannelView.UpdateAsync(Server server, string channelId)
    {
      if (server == null || channelId == null) {
        _listStore.Clear();
        return;
      }

      var connections = await server.GetChannelConnectionsAsync(channelId);
      int index = _listView.SelectedRow;
      Debug.Print("index={0}", index);
      _listStore.Clear();
      foreach (var connection in connections) {
        int row = _listStore.AddRow();
        int bitrate = BitrateKbps(connection);
        _listStore.SetValues(row,
          _protocolName, connection.ProtocolName,
          _type, connection.Type,
          _status, connection.Status,
          _remoteName, connection.RemoteName,
          _totalRate, $"{bitrate}Kbps");
      }
      if (index >= 0)
        _listView.SelectRow(index);
    }
  }
}

