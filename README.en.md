# osu!taiko Mapping Helper

[日本語](README.md)

osu!taiko Mapping Helper is a Windows tool that supports osu!taiko beatmap editing.
It reads the current beatmap information from a running osu! client and edits `.osu` files, including SV, volume, offset, hitsounds, note positions, tags, and background position.

## Main Features

For the main features and usage instructions, see the following user guides.

- [User Guide (Japanese)](https://calmeel.github.io/osutaiko-mapping-helper/index.html)
- [User Guide (English)](https://calmeel.github.io/osutaiko-mapping-helper/index.en.html)

## Requirements

- Windows
- .NET 8 SDK
- Visual Studio 2022 or later, or .NET CLI
- osu! stable

## Libraries

- [OsuMemoryDataProvider](https://www.nuget.org/packages/OsuMemoryDataProvider) `0.11.1`
- [OsuParsers](https://www.nuget.org/packages/OsuParsers) `1.7.2`

Dependencies are defined in `osu!taiko Mapping Helper.csproj`.

## Setup

Clone the repository.

```powershell
git clone https://github.com/miyagish1m4/osutaiko-mapping-helper.git
cd osutaiko-mapping-helper
```

Restore dependencies.

```powershell
dotnet restore
```

## Build

```powershell
dotnet build
```

To build the Release configuration, run:

```powershell
dotnet build -c Release
```

## Run

```powershell
dotnet run --project "osu!taiko Mapping Helper/osu!taiko Mapping Helper.csproj"
```

Before using the app, start osu! and open the beatmap you want to edit.

## Developer Notes

This project is built with C# and Windows Forms.

- Solution: `osu!taiko Mapping Helper.sln`
- Project: `osu!taiko Mapping Helper/osu!taiko Mapping Helper.csproj`
- Entry point: `osu!taiko Mapping Helper/Program.cs`
- Main form: `osu!taiko Mapping Helper/Views/MainForm.cs`
- SV calculation: `osu!taiko Mapping Helper/Services/SVCalculatorService.cs`
- Resnap: `osu!taiko Mapping Helper/Services/ResnapService.cs`

## License

This project is released under the MIT License.
See [LICENSE](LICENSE) for details.
