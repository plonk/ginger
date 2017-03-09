using System;
using Xwt;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ginger
{
  public class ConnectionsPage : HBox, ChannelView
  {
    BrowserContext _context;
    ListView _listView = new ListView();
    ListStore _listStore;
    DataField<string> _protocolName = new DataField<string>();
    DataField<string> _type = new DataField<string>();
    DataField<string> _status = new DataField<string>();
    DataField<string> _remoteName = new DataField<string>();
    DataField<string> _totalRate = new DataField<string>();
    DataField<Connection> _connection = new DataField<Connection>();

    public ConnectionsPage(BrowserContext context)
    {
      _context = context;

      Margin = 10;

      _listStore = new ListStore(_protocolName, _type, _status, _remoteName, _totalRate, _connection);
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
      var vbox = new VBox { WidthRequest = 80 };

      var disconnectButton = new Button("切断");
      var reconnectButton = new Button("再接続");

      disconnectButton.Clicked += async (sender, e) => {
        var conn = _listStore.GetValue(_listView.SelectedRow, _connection);
        if (conn != null) {
          var success = await _context.Server.StopChannelConnectionAsync(_context.Channel.ChannelId, conn.ConnectionId);
          if (!success)
            MessageDialog.ShowError(_context.Window, "エラー", "切断に失敗しました。");
          await UpdateAsync();
        }
      };
      reconnectButton.Clicked += async (sender, e) => {
        var conn = _listStore.GetValue(_listView.SelectedRow, _connection);
        if (conn != null) {
          try {
            await _context.Server.RestartChannelConnectionAsync(_context.Channel.ChannelId, conn.ConnectionId);
          }
          catch (Exception ex) {
            MessageDialog.ShowError(_context.Window, "エラー", ex.Message);
            throw;
          }
          await UpdateAsync();
        }
      };

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

    public async Task UpdateAsync()
    {
      if (_context.Server == null || _context.Channel == null) {
        _listStore.Clear();
        return;
      }

      var connections = await _context.Server.GetChannelConnectionsAsync(_context.Channel.ChannelId);
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
          _totalRate, $"{bitrate}Kbps",
          _connection, connection);
      }
      if (index >= 0 && index < _listView.DataSource.RowCount)
        _listView.SelectRow(index);
    }
  }
}

