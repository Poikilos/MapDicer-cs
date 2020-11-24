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

### Running Your Own Build
In "Settings," "Update & Security, "For developers" in Windows 10,
the "Use developer features" setting must be "Sideload Apps" or
**"Developer Mode"** (only Developer Mode is tested).
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


## References
- Brink. (2019, March 15). Cannot turn off Developer mode—"managed by
  your organization" [Reply]. Windows Ten Forums. 
  https://www.tenforums.com/general-support/128943-cannot-turn-off-developer-mode-managed-your-organization.html
