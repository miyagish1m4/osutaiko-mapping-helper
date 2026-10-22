# osu!taiko Mapping Helper

[日本語](README.md)

osu!taiko Mapping Helper is a Windows tool that supports osu!taiko beatmap editing.
It reads the current beatmap information from a running osu! client and edits `.osu` files, including SV, volume, offset, hitsounds, note positions, tags, and background position.

## Main Features

For the main features and usage instructions, see the following user guides.

- [User Guide (English)](https://calmeel.github.io/osutaiko-mapping-helper/index.en.html)

## Requirements

- Windows
- .NET 10 SDK
- Visual Studio 2026 or later, or .NET CLI
- osu! stable

## Libraries

- [OsuMemoryDataProvider](https://www.nuget.org/packages/OsuMemoryDataProvider) `0.12.1`
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

## Tests

Tests are in `osu!taiko Mapping Helper.Tests` (xUnit). The test project is part of the solution, so `dotnet build` also builds it.

```powershell
dotnet test
```

- The tests also use Windows Forms, so run them on Windows.
- The tests run only inside the temporary folder (`%TEMP%`) and do not modify beatmaps in your osu! Songs folder or the Helper settings.
- The following tests run only when the environment variable is set. Otherwise, they do nothing and pass.

| Environment variable | Test |
|---|---|
| `OSU_SONGS_PATH` | Reads and writes every taiko beatmap in the specified Songs folder. Beatmaps are copied to the temporary folder before writing, so the originals are not modified |
| `OSU_HELPER_GITHUB_TEST` | Set to `1` to check that the latest release information can be retrieved from GitHub Releases |

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
- Tests: `osu!taiko Mapping Helper.Tests/`
- Source files use UTF-8 and CRLF line endings

## License

This repository uses separate licenses for program files and image assets.

- Program and documentation: MIT License
- Image assets in `osu!taiko Mapping Helper/Resources/`: GNU Affero General Public License version 3 or later

See [LICENSES.md](LICENSES.md) for details.
