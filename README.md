# osu!taiko Mapping Helper

[English](README.en.md)

osu!taiko の譜面編集を補助する Windows 向けツールです。
起動中の osu! から現在の譜面情報を取得し、`.osu` ファイルに対して SV、Volume、オフセット、ヒットサウンド、ノーツ座標、タグ、背景位置などの編集を行います。

## 主な機能

主な機能や使用方法については、以下の使用説明書を参照してください。

- [使用説明書 (日本語)](https://calmeel.github.io/osutaiko-mapping-helper/index.html)

## 動作環境

- Windows
- .NET 10 SDK
- Visual Studio 2026 以降、または .NET CLI
- osu! stable

## 使用ライブラリ

- [OsuMemoryDataProvider](https://www.nuget.org/packages/OsuMemoryDataProvider) `0.12.1`
- [OsuParsers](https://www.nuget.org/packages/OsuParsers) `1.7.2`

依存パッケージは `osu!taiko Mapping Helper.csproj` に定義されています。

## セットアップ

リポジトリをクローンします。

```powershell
git clone https://github.com/miyagish1m4/osutaiko-mapping-helper.git
cd osutaiko-mapping-helper
```

依存関係を復元します。

```powershell
dotnet restore
```

## ビルド

```powershell
dotnet build
```

Release 構成でビルドする場合は次のコマンドを使用します。

```powershell
dotnet build -c Release
```

## テスト

テストは `osu!taiko Mapping Helper.Tests`（xUnit）にあります。ソリューションに含まれているため、`dotnet build` でテストのプロジェクトも一緒にビルドされます。

```powershell
dotnet test
```

- テストは Windows Forms の画面も使うため、Windows で実行してください。
- テストは一時フォルダ（`%TEMP%`）の中だけで動き、osu! の Songs フォルダの譜面や Helper の設定は書き換えません。
- 次のテストは、環境変数を設定したときだけ動きます。設定しない場合は、何もせずに成功になります。

| 環境変数 | テストの内容 |
|---|---|
| `OSU_SONGS_PATH` | 指定した Songs フォルダの全譜面（太鼓）で、読み込みと書き出しを確かめます。譜面は一時フォルダにコピーしてから書き出すため、元の譜面は書き換えません |
| `OSU_HELPER_GITHUB_TEST` | `1` を指定すると、GitHub の Releases から最新版の情報を取得できるかを確かめます |

## 実行

```powershell
dotnet run --project "osu!taiko Mapping Helper/osu!taiko Mapping Helper.csproj"
```

アプリを使用する前に、osu! を起動して編集対象の譜面を開いてください。

## 開発者向けメモ

プロジェクトは C# / Windows Forms で構成されています。

- ソリューション: `osu!taiko Mapping Helper.sln`
- プロジェクト: `osu!taiko Mapping Helper/osu!taiko Mapping Helper.csproj`
- エントリーポイント: `osu!taiko Mapping Helper/Program.cs`
- メイン画面: `osu!taiko Mapping Helper/Views/MainForm.cs`
- SV 計算: `osu!taiko Mapping Helper/Services/SVCalculatorService.cs`
- 再スナップ: `osu!taiko Mapping Helper/Services/ResnapService.cs`
- テスト: `osu!taiko Mapping Helper.Tests/`
- ソースファイルの文字コードは UTF-8、改行は CRLF です（Shift_JIS などで保存しないでください）

## ライセンス

このリポジトリでは、プログラムと画像データでライセンスを分けています。

- プログラム本体およびドキュメント: MIT License
- `osu!taiko Mapping Helper/Resources/` 配下の画像データ: GNU Affero General Public License version 3 or later

詳細は [LICENSES.md](LICENSES.md) を参照してください。
