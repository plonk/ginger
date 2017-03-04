using System;
using Xwt;
using System.Threading.Tasks;

namespace ginger
{
  public class RelayTreePage : ScrollView, ChannelView
  {
    BrowserContext _context;
    DataField<string> _ipDataField;
    TreeView _treeView;
    TreeStore _treeStore;

    public RelayTreePage(BrowserContext context) : base()
    {
      _context = context;

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

    async Task ChannelView.UpdateAsync()
    {
      if (_context.Server == null || _context.Channel == null) {
        _treeStore.Clear();
        return;
      }

      var rootNodes = await _context.Server.GetChannelRelayTreeAsync(_context.Channel.ChannelId);
      _treeStore.Clear();
      foreach (var rootNode in rootNodes) {
        var nav = _treeStore.AddNode();
        SetSubtree(nav, rootNode);
      }

      _treeView.ExpandAll();
    }
  }
}

