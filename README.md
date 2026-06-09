# osu!taiko Mapping Helper

[English](README.en.md)

osu!taiko の譜面編集を補助する Windows 向けツールです。
起動中の osu! から現在の譜面情報を取得し、`.osu` ファイルに対して SV、Volume、オフセット、ヒットサウンド、ノーツ座標、タグ、背景位置などの編集を行います。

## 主な機能

主な機能や使用方法については、以下の使用説明書を参照してください。

- [使用説明書 (日本語)](https://calmeel.github.io/osutaiko-mapping-helper/index.html)

## 動作環境

- Windows
- .NET 8 SDK
- Visual Studio 2022 以降、または .NET CLI
- osu! stable

## 使用ライブラリ

- [OsuMemoryDataProvider](https://www.nuget.org/packages/OsuMemoryDataProvider) `0.11.1`
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

## ライセンス

このプロジェクトは MIT License の下で公開されています。
詳細は [LICENSE](LICENSE) を参照してください。
