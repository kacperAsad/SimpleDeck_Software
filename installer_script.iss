[Setup]
AppName=SimpleDeck
AppVersion=1.0
DefaultDirName={autopf}\SimpleDeck
DefaultGroupName=SimpleDeck
OutputDir=.\installer_output
OutputBaseFilename=SimpleDeck_Setup
Compression=lzma
SolidCompression=yes

[Files]
Source: "SimpleDeck_Windows_App\bin\Release\net10.0\win-x64\publish\SimpleDeck_Windows_App.exe"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\SimpleDeck"; Filename: "{app}\SimpleDeck_Windows_App.exe"