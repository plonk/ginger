using System;
using Xwt;
using System.Threading.Tasks;

namespace ginger
{
  public class SettingsPage
  : VBox, ServerView
  {
    BrowserContext _context;

    SpinButton _maxRelays;
    SpinButton _maxRelaysPerChannel;
    SpinButton _maxDirects;
    SpinButton _maxDirectsPerChannel;
    SpinButton _maxUpstreamRate;
    SpinButton _maxUpstreamRatePerChannel;

    public SettingsPage(BrowserContext context)
    {
      _context = context;

      Margin = 10;

      var table = new Table();
      table.SetColumnSpacing(1, 10);

      table.Add(new Label("リレー数上限"), 0, 0, 1, 1, false, false, WidgetPlacement.End);
      table.Add(new Label("ダイレクト数上限"), 0, 1, 1, 1, false, false, WidgetPlacement.End);
      table.Add(new Label("上り帯域上限"), 0, 2, 1, 1, false, false, WidgetPlacement.End);

      table.Add(new Label("Ch毎"), 2, 0, 1, 1, false, false, WidgetPlacement.End);
      table.Add(new Label("Ch毎"), 2, 1, 1, 1, false, false, WidgetPlacement.End);
      table.Add(new Label("Ch毎"), 2, 2, 1, 1, false, false, WidgetPlacement.End);

      _maxRelays = IntegerSpinButton();
      _maxDirects = IntegerSpinButton();
      _maxUpstreamRate = IntegerSpinButton();

      _maxRelaysPerChannel = IntegerSpinButton();
      _maxDirectsPerChannel = IntegerSpinButton();
      _maxUpstreamRatePerChannel = IntegerSpinButton();

      table.Add(_maxRelays, 1, 0, 1, 1, false, false, WidgetPlacement.Start);
      table.Add(_maxDirects, 1, 1, 1, 1, false, false, WidgetPlacement.Start);
      table.Add(_maxUpstreamRate, 1, 2, 1, 1, false, false, WidgetPlacement.Start);

      table.Add(_maxRelaysPerChannel, 3, 0, 1, 1, true, false, WidgetPlacement.Start);
      table.Add(_maxDirectsPerChannel, 3, 1, 1, 1, true, false, WidgetPlacement.Start);
      table.Add(_maxUpstreamRatePerChannel, 3, 2, 1, 1, true, false, WidgetPlacement.Start);

      PackStart(table, true, true);

      var applyButton = new Button("適用") { WidthRequest = 80 };
      applyButton.Clicked += async (sender, e) => {
        await ApplySettingsAsync();
      };
      PackStart(applyButton, false, WidgetPlacement.Start, WidgetPlacement.End);
    }

    async Task ApplySettingsAsync()
    {
      var settings = new Settings();

      settings.maxRelays       = (int)_maxRelays.Value;
      settings.maxDirects      = (int)_maxDirects.Value;
      settings.maxUpstreamRate = (int)_maxUpstreamRate.Value;

      settings.maxRelaysPerChannel       = (int)_maxRelaysPerChannel.Value;
      settings.maxDirectsPerChannel      = (int)_maxDirectsPerChannel.Value;
      settings.maxUpstreamRatePerChannel = (int)_maxUpstreamRatePerChannel.Value;

      await _context.Server.SetSettingsAsync(settings);
      MessageDialog.ShowMessage(_context.Window, "設定を適用しました。");
      await (this as ServerView).UpdateAsync();
    }

    SpinButton IntegerSpinButton()
    {
      var button = new SpinButton { Digits = 0, IncrementValue = 1, MaximumValue = Double.MaxValue };
      return button;
    }

    async Task ServerView.UpdateAsync()
    {
      var settings = await _context.Server.GetSettingsAsync();

      _maxRelays.Value = settings.maxRelays;
      _maxDirects.Value = settings.maxDirects;
      _maxUpstreamRate.Value = settings.maxUpstreamRate;

      _maxRelaysPerChannel.Value = settings.maxRelaysPerChannel;
      _maxDirectsPerChannel.Value = settings.maxDirectsPerChannel;
      _maxUpstreamRatePerChannel.Value = settings.maxUpstreamRatePerChannel;

    }
  }
}

