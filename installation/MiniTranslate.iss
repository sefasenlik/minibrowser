[Setup]
AppName=MiniTranslate
AppVersion=1.1
DefaultDirName={pf}\MiniTranslate
DefaultGroupName=MiniTranslate
UninstallDisplayIcon={app}\icon.ico
OutputDir=.
OutputBaseFilename=MiniTranslateSetup
Compression=lzma
SolidCompression=yes

[Files]
Source: "publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "miniweb.js"; DestDir: "{app}"; Flags: ignoreversion
Source: "translator.html"; DestDir: "{app}"; Flags: ignoreversion
Source: "translation-server.html"; DestDir: "{app}"; Flags: ignoreversion
Source: "icon.ico"; DestDir: "{app}"; Flags: ignoreversion
Source: "README.md"; DestDir: "{app}"; Flags: ignoreversion
Source: "CHATGPT_SETUP.md"; DestDir: "{app}"; Flags: ignoreversion
Source: "CHATGPT_API_SETUP.md"; DestDir: "{app}"; Flags: ignoreversion
Source: "node.exe"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\MiniTranslate"; Filename: "{app}\MiniTranslate.exe"; WorkingDir: "{app}"
Name: "{group}\Uninstall MiniTranslate"; Filename: "{uninstallexe}"

[Run]
Filename: "{app}\MiniTranslate.exe"; Description: "Launch MiniTranslate"; Flags: nowait postinstall skipifsilent