# MapDicer
With MapDicer, cartographers will be able to add a level of detail 
(LOD) on a per-region basis and import from various scales and formats.


## System Requirements
### Windows
#### Recommended
- Windows 10, version 1903 (10.0, Build 18362)

#### Minimum
- Windows 10, version 1809 (10.0, Build 17763)


## Compiling

To allow the Visual Studio build to see the assets (such as vector 
icons), first run vs-prebuild.bat (makes symbolic links in the Debug 
and Release directories to the Assets directory).

### Compiling the UWP Branch
The UWP Branch is deprecated and does not do anything. See changelog 
for 2020-12-16 for why. In "Settings," "Update & Security, "For 
developers" in Windows 10, the "Use developer features" setting must be 
"Sideload Apps" or **"Developer Mode"** (only Developer Mode is 
tested).
- If the settings are grayed out, you must **delete** the registry DWORD:
  ```
Windows Registry Editor Version 5.00

[HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Appx]
"AllowAllTrustedApps"=-
```
  (or use `MapDicer/doc/development/assets/developer_mode-ALLOW.reg`)
  as per Brink's instructions (2019).
  - Undo this step using
    `MapDicer/doc/development/assets/developer_mode-UNDO.reg`.
  - Then restart Windows 10.


## Developer Notes

### SVG
- Inside of the SVG file, the `width` and `height` must be set to
  `"auto"` for it to scale at all (the `viewBox` must also have all of
  the geometry inside of it or it may not scale as expected).
  
### Colors
- See https://plantuml.com/color for a UML color palette.

### Dependencies
- Uno.SQLitePCLRaw.wasm for cross-platform SQLite support.
  - SQLitePCLRaw.core.2.0.3
  - SQLitePCLRaw.provider.e_sqlite3.2.0.3
  - System.Memory.4.5.3
  - System.Runtime.CompilerServices.Unsafe.4.5.2
  - Uno.sqlite-wasm.1.1.0-dev.16828
  - Uno.SQLitePCLRaw.provider.wasm.3.0.14
- Microsoft.EntityFrameworkCore.5.0.1
  - Apache 2.0 license, by Microsoft


### Diagrams
See ./doc/development.
Check the Changelog for changes if noted there.

#### Dia
To open dia files on Windows, Dia 0.97.3 or later is recommended--you
can get an unofficial build here: <https://portableapps.com/node/59981>.

Dia is now maintained at the Gnome project, not at SourceForge.
See the issue ["Windows distribution &
installer"](https://gitlab.gnome.org/neduard/dia/-/issues/3) on
the Gnome project for the status of Windows builds.


## Authors & License
- The MapDicer logo is [Creative Commons
  Attribution-NonCommercial-NoDerivatives
  4.0](http://creativecommons.org/licenses/by-nc-nd/4.0/)
- gear-simple.svg is Public Domain by Openclipart via
  https://publicdomainvectors.org/en/free-clipart/Simple-gear/62928.html


## References
- Brink. (2019, March 15). Cannot turn off Developer mode—"managed by
  your organization" [Reply]. Windows Ten Forums. 
  https://www.tenforums.com/general-support/128943-cannot-turn-off-developer-mode-managed-your-organization.html
