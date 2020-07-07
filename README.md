# TaikoOS

TaikoOSは新時代のオリジナルシミュを目指して開発を進めています

## 追加命令について
Testフォルダ内の「[追加命令について.md](https://github.com/Mr-Ojii/TJAPlayer3-f/blob/master/Test/追加命令について.md)」で説明いたします。

## 複数文字コードの対応について
Shift-JIS, UTF-8 BOMあり/なし は読み込めることを確認しました。  
読み込みに失敗して、文字化けするファイルがありましたら、Discord鯖に投げてもらえると助かります。  
GitHub Issuesでもいいですが、Issuesが増えすぎても嫌なので。

## 推奨環境
Windows7以降  
まぁ、Windows10で動作確認をしているので、Windows10が一番安定してます


フォーク元より

>Ver.1.5.8.0 : より本家っぽく。
>
>Ver.1.5.8.1 : 王冠機能の搭載(かんたん～おに & Edit(実質裏鬼))
>
>Ver.1.5.8.2 : .NET Framework 4.0にフレームワークをアップデート
>
>Ver.1.5.8.3 : 譜面分岐について・JPOSSCROLLの連打についての既知のバグを修正
>
>Ver.1.5.9.0 : 複数の文字コードに対応
>
>Ver.1.5.9.1 : WASAPI共有に対応
>
>Ver.1.5.9.2 : .NET Framework 4.8にフレームワークをアップデート
>
>Ver.1.5.9.3 : スコアが保存されないバグを修正 & songs.dbを軽量化
>
>Ver.1.6.0.0 : 難易度選択画面を追加 & メンテモード追加(タイトル画面でCtrl+Aを押しながら、演奏ゲームを選択)
>
>Ver.1.6.0.1 : Open Taiko Chartへの対応(β)
>
>Ver.1.6.0.2 : 片開き(仮)実装

## Discord鯖
こちらで配布の予定です.

[公式サーバー](https://discord.gg/uBsmQuB)

## 開発環境
Windows 10(Ver.1909)  
VisualStudio Community 2019

## バグ報告のお願い

バグを見つけたらIssuesで報告してもらえると、  はかどるのでよろしくお願いします。

## 謝辞
このTaikoOSのもととなるソフトウェアを作成・メンテナンスしてきた中でも  
有名な方々に感謝の意を表し、お名前を上げさせていただきたいと思います。

- Mr.Ojii様
- FROM/yyagi様
- kairera0467様
- AioiLight様

また、他のTJAPlayer関係のソースコードを参考にさせてもらっている箇所があります。  
ありがとうございます。

## ライセンス関係
以下のライブラリを追加いたしました。
* ReadJEnc
* SharpDX
* Newtonsoft.Json

ライセンスは「Test/Licenses」に追加いたしました。
