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


### Compiling the Uno Branch
- Install Visual Studio 2019
- Right-click Start
- Apps & Features
- Choose "Microsoft Visual Studio installer"
- Click "Change"
- Select "Visual Studio Community 2019"
- Click "Modify" and install the necessary workloads
  as per <https://platform.uno/docs/articles/get-started.html>:
  - Universal Windows Platform
  - Mobile development with .NET (Xamarin)
  - "ASP.NET and web workload installed,"
  - "along with .NET Core 2.2 or later (for WASM development)"
    (available in the same menu or at
    [.NET Core](https://dotnet.microsoft.com/download/dotnet-core) as
    cited by the Uno get-started page)


## Developer Notes


### SVG
- Inside of the SVG file, the `width` and `height` must be set to
  `"auto"` for it to scale at all (the `viewBox` must also have all of
  the geometry inside of it or it may not scale as expected).
- Binding the Width and Height to something will only work if the
  element is declared previously. Therefore, placing it inside of a 
  button may work better, then binding `height` or `width` and `height`
  to that button's (where button has
  `Padding="0" BorderThickness="0" Margin="0"`).

### Colors
- See https://plantuml.com/color for a UML color palette.

### Dependencies (NuGet)
- SharpVectors (SharpVectors.Reloaded says to use the SharpVectors
  package instead). See <https://github.com/ElinamLLC/SharpVectors>.

### UWP (deprecated) Dependencies (NuGet)
- Uno.SQLitePCLRaw.wasm for cross-platform SQLite support.
  - SQLitePCLRaw.core.2.0.3
  - SQLitePCLRaw.provider.e_sqlite3.2.0.3
  - System.Memory.4.5.3
  - System.Runtime.CompilerServices.Unsafe.4.5.2
  - Uno.sqlite-wasm.1.1.0-dev.16828
  - Uno.SQLitePCLRaw.provider.wasm.3.0.14
- Microsoft.EntityFrameworkCore.5.0.1
  - Apache 2.0 license, by Microsoft


### SharpVectors Notes
#### Building Samples
Upon trying to build SharpVectors samples, the following message appears
(multiple times):
```
Severity	Code	Description	Project	File	Line	Suppression State
Error	MSB3644	The reference assemblies for .NETFramework,Version=v4.7 were not found. To resolve this, install the Developer Pack (SDK/Targeting Pack) for this framework version or retarget your application. You can download .NET Framework Developer Packs at https://aka.ms/msbuild/developerpacks	SharpVectors.Core	C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\Microsoft.Common.CurrentVersion.targets	1180	
```
- Installing the developer pack solves it. Others such as
  WpfTestSvgControl require 4.8, but compiling from git says:
  ```
Severity	Code	Description	Project	File	Line	Suppression State
Error	MC3074	The tag 'TextEditor' does not exist in XML namespace 'http://icsharpcode.net/sharpdevelop/avalonedit'. Line 11 Position 10.	WpfTestSvgControl	C:\Users\Jatlivecom\source\repos\SharpVectors\Samples\WpfTestSvgControl\DebugPage.xaml	11	
Error	MC3074	The tag 'TextEditor' does not exist in XML namespace 'http://icsharpcode.net/sharpdevelop/avalonedit'. Line 175 Position 22.	WpfTestSvgControl	C:\Users\Jatlivecom\source\repos\SharpVectors\Samples\WpfTestSvgControl\DrawingPage.xaml	175	
Error	MC3074	The tag 'TextEditor' does not exist in XML namespace 'http://icsharpcode.net/sharpdevelop/avalonedit'. Line 76 Position 10.	WpfTestSvgControl	C:\Users\Jatlivecom\source\repos\SharpVectors\Samples\WpfTestSvgControl\SvgPage.xaml	76	
Error	MC3050	Cannot find the type 'HighlightingManager'. Note that type names are case sensitive.	WpfTestSvgControl	C:\Users\Jatlivecom\source\repos\SharpVectors\Samples\WpfTestSvgControl\XamlPage.xaml	56	
```
#### Usage
- SVG `width` or `height` cannot be `"auto"` (Removing width and height
  from the svg works).

##### Examples
From SharpVectors\Samples\WpfTestSvgSample\SvgPage.xaml:
```
<Button Click="OnSaveFileClick" ToolTip="Save Svg File">
    <Image Source="{svgc:SvgImage Source=/Images/Save.svg, AppName=WpfTestSvgSample}" Height="24" Width="24"/>
</Button>
```
- As per <https://github.com/ElinamLLC/SharpVectors/blob/master/Docs/Usage.md#Controls>
  add to window:
    - `xmlns:svgc="http://sharpvectors.codeplex.com/svgc/"`
`<Image Source="{svgc:SvgImage Test2.svg, TextAsGeometry=True}"/>`
  - add to Window:
- from same:
  `<Image Source="{svgc:SvgImage {StaticResource WebFile}}"/>`
    - ```
<Window.Resources>
	<ResourceDictionary> 
	    <sys:String x:Key="WebFile">
		http://upload.wikimedia.org/wikipedia/commons/c/c7/SVG.svg
	    </sys:String>
	</ResourceDictionary>
    </Window.Resources>
```

Problem:
```
Severity	Code	Description	Project	File	Line	Suppression State
Error	XDG0006	The namespace prefix "svgc" is not defined.	MapDicer	C:\Users\Jatlivecom\GitHub\MapDicer\MapDicer\MainWindow.xaml	2	
Error	XDG0008	SvgImage is not supported in a Windows Presentation Foundation (WPF) project.	MapDicer	C:\Users\Jatlivecom\GitHub\MapDicer\MapDicer\MainWindow.xaml	36	
Error	XLS0414	The type 'svgc:SvgImage' was not found. Verify that you are not missing an assembly reference and that all referenced assemblies have been built.	MapDicer	C:\Users\Jatlivecom\GitHub\MapDicer\MapDicer\MainWindow.xaml	36	
```

Solution:


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
