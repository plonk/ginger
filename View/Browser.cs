using System;
using Xwt;

namespace ginger.View
{
  // ブラウザのビュー。Changed イベントに反応して状態を表示する。
  public class Browser
    : Xwt.Window
  {
    AppModel.BrowserModel _model;
    Button _button;
    ComboBox _comboBox;

    public Browser (AppModel.BrowserModel model)
      : base()
    {
      Build ();
      _model = model;
      _model.Changed += Update;
      // 状態の最初の読み込み。
      Update ();
    }

    // ウィジェットの作成とコールバックの設定。
    void Build()
    {
      Width = 500;
      Height = 400;

      _comboBox = new ComboBox ();

      var vbox = new VBox ();
      _button = new Button ();
      _button.Clicked += (sender, e) => _model.Click();
      vbox.PackStart (_comboBox);
      vbox.PackStart (new Label ("↓ボタン"));
      vbox.PackStart (_button);
      Content = vbox;

      Closed += (sender, e) => { _model.Close(); };
      Show ();
    }

    // 状態の反映。
    void Update()
    {
      // コンボボックスにサーバントのリストをロードする。
      foreach (var servent in _model.Servents)
      {
        _comboBox.Items.Add(servent.ToString());
      }

      _button.Label = _model.ButtonLabel;
      Title = _model.Title;

      if (!_model.IsOpen)
        this.Dispose ();
    }

  }
}
