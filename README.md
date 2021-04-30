# BveDenshadeGoInterface
Bve用のPS用電車でGO!コントローラー入力プラグイン  
このプラグインは次の商品に対応しています

* TCPP-20001 電車でGO!コントローラー ワンハンドタイプ
* SLPH-00051 電車でGO!コントローラー ツーハンドタイプ

また、使用するには[JC-PS101U](https://www.elecom.co.jp/products/JC-PS101USV.html)(販売終了)が必要です。もしくは設定ファイルを変更して他のコンバーターを使用することも可能です。他のコンバーターを使用する際は下記項目をご覧ください。  
# インストール
[Releases](https://github.com/kusaanko/BveDenshadeGoInterface/releases)ページから最新版をダウンロードします。  
Bve5.8以前なら`C:\Program Files (x86)\mackoy\BveTs5\Input Devices`、Bve6.0以降なら`C:\Program Files\mackoy\BveTs6\Input Devices`
# JC-PS101U以外のコンバーターを使用する
ドキュメント\BveTs\SettingsにあるKusaanko.DenshadeGoInput.xmlをテキストエディタで開いて下さい。  
中に書いてある`<ControllerName>PutYourAdditionalControllerNameHere</ControllerName>`のPutYourAdditionalControllerNameHereを使用するコントローラー名に変更して下さい。  
コントローラー名は`コントロールパネル>デバイスとプリンター`の中に書いてあるコントローラー名が使用できます。もしくは`USB ゲーム コントローラーのセットアップ`を開いた際に出てくる名前も使用できます。  
また、JC-PS101Uとキーマップが違えば書き換える必要があります。
