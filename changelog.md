# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).


## [git] - 2020-12-16
### Added
- SharpVectors NuGet package to display SVG files in XAML
  (See <https://github.com/ElinamLLC/SharpVectors>).

### Changed
- Switch to WPF to avoid problems with UWP limiting external files and
  database access libraries. See 
  <https://sqliteefcore-wasm.platform.uno/>. You must import and 
  install EntityFrameworkCore via Console Core not Universal 
  Windows--see <https://github.com/dotnet/efcore/issues/9666> cited by 
  <https://stackoverflow.com/questions/59444014/entity-framework-tools-not-working-with-uwp-apps-c-sharp>. 
  That can allow it to work but it is unweildy and the problem of 
  loading and saving arbitrary paths remains. A cartography application 
  utilizing, transforming, and saving a large amount of external data 
  isn't well-suited for sandboxed environment nor for a platform with
  arbitrary missing pieces that may make additional useful libraries not
  work even if EntityFrameworkCore could work using the workaround.

### Fixed
- Remove width and height from SVG files to allow SharpVectors to work
  (crashes with disallowed width or height format otherwise).


## [git] - 2020-12-16
### Changed
- Switch from Page to ContentDialog for modal dialogs.
- Install Uno.SQLitePCLRaw.wasm (See readme).


## [git] - 2020-11-22
### Added
- Add Win10 developer mode troubleshooting steps.
