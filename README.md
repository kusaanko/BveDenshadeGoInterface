# BveDenshadeGoInterface
Bve用のPS用電車でGO!コントローラー入力プラグイン  
このプラグインは次の商品に対応しています

* TCPP-20001 電車でGO!コントローラー ワンハンドタイプ
* SLPH-00051 電車でGO!コントローラー ツーハンドタイプ

また、使用するには[JC-PS101U](https://www.elecom.co.jp/products/JC-PS101USV.html)(販売終了)等のコンバーターが必要です。デフォルトではこのJC-PS101Uを使用するように設定されています。  
# 機能
* 電車でGO!コントローラー ワン/ツーハンドタイプをBveで使用可能にする
* Bve起動中にコントローラが切断後、再接続された場合自動復帰します
* 各種ボタンに機能が割り当てられます
* 複数のコントローラーが接続中でも動作します(ただし、同じ名前のコントローラーが複数接続中だとうまくコントローラーを選択できません)
# インストール
[Releases](https://github.com/kusaanko/BveDenshadeGoInterface/releases)ページから最新版をダウンロードします。  
Bve5.8以前なら`C:\Program Files (x86)\mackoy\BveTs5\Input Devices`、Bve6.0以降なら`C:\Program Files\mackoy\BveTs6\Input Devices`を開き、ダウンロードした`Kusaanko.DenshadeGoInterface.dll`を配置します。  
Bveを起動し、設定画面を開き、入力デバイスを開きます。  
BveDenshadeGoInterfaceにチェックを入れ、その他の不要な入力プラグインを無効化します。
# JC-PS101U以外のコンバーターを使用する
Bveの設定画面を開き、入力デバイスを開きます。  
BveDenshadeGoInterfaceを選択してプロパティーをクリックして下さい。  
コントローラーから使いたいコンバーター名を選択して、コントローラーのセットアップを押して、出てきた画面の指示に従って下さい。
# ライセンス
[SlimDX](https://github.com/SlimDX/slimdx) - Copyright (c) 2007-2012 SlimDX Group [MIT License](https://github.com/SlimDX/slimdx/blob/master/License.txt)