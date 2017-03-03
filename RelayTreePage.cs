using System;
using Xwt;
using System.Threading.Tasks;

namespace ginger
{
  public class RelayTreePage : ScrollView, ChannelView
  {
    DataField<string> _ipDataField;
    TreeView _treeView;
    TreeStore _treeStore;

    public RelayTreePage() : base()
    {
      Margin = 10;

      _ipDataField = new DataField<string>();
      _treeStore = new TreeStore(_ipDataField);
      _treeView = new TreeView() { DataSource = _treeStore };
      _treeView.Columns.Add("IP", _ipDataField);
      Content = _treeView;
    }

    void SetSubtree(TreeNavigator nav, RelayNode host)
    {
      nav.SetValue(_ipDataField, host.Address);
      foreach (var childHost in host.Children) {
        var childNav = nav.Clone().AddChild(); // nav は固定したいので Clone する。
        SetSubtree(childNav, childHost);
      }
    }

    async Task ChannelView.UpdateAsync(Server server, string channelId)
    {
      if (server == null || channelId == null) {
        _treeStore.Clear();
        return;
      }

      var rootNodes = await server.GetChannelRelayTreeAsync(channelId);
      _treeStore.Clear();
      foreach (var rootNode in rootNodes) {
        var nav = _treeStore.AddNode();
        SetSubtree(nav, rootNode);
      }
    }
  }
}

