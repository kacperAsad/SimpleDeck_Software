[Setup]
AppName=SimpleDeck
AppVersion=1.0
DefaultDirName={autopf}\SimpleDeck
DefaultGroupName=SimpleDeck
OutputDir=.\installer_output
OutputBaseFilename=SimpleDeck_Setup
Compression=lzma
SolidCompression=yes
# Wymagane, aby móc dodawać wpisy do rejestru użytkownika bez uprawnień admina (opcjonalnie)
PrivilegesRequired=lowest 

[Tasks]
# Definiujemy zadanie "autostart", które domyślnie jest zaznaczone (Flags: checkedonce)
Name: "autostart"; Description: "Uruchamiaj SimpleDeck automatycznie przy starcie systemu"; GroupDescription: "Opcje dodatkowe:"; Flags: checkedonce

[Files]
Source: "SimpleDeck_Windows_App\bin\Release\net10.0\win-x64\publish\SimpleDeck_Windows_App.exe"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\SimpleDeck"; Filename: "{app}\SimpleDeck_Windows_App.exe"
Name: "{autodesktop}\SimpleDeck"; Filename: "{app}\SimpleDeck_Windows_App.exe"

[Registry]
# Ten wpis dodaje aplikację do autostartu w rejestrze Windows (HKCU - tylko dla obecnego użytkownika)
Root: HKCU; Subkey: "Software\Microsoft\Windows\CurrentVersion\Run"; \
    ValueType: string; ValueName: "SimpleDeck"; \
    ValueData: """{app}\SimpleDeck_Windows_App.exe"""; \
    Tasks: autostart; Flags: uninsdeletevalue

[Run]
# Opcja uruchomienia aplikacji od razu po zakończeniu instalacji
Filename: "{app}\SimpleDeck_Windows_App.exe"; Description: "Uruchom SimpleDeck teraz"; Flags: nowait postinstall skipifsilent