; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "CalcPro"
#define MyAppVersion "1.1"
#define MyAppPublisher "Categis Software"
#define MyAppURL "http://www.softwaretogo.de/"
#define MyAppExeName "CalcPro.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{E3E92DFD-156F-4ADF-8A5F-9E680E2A7G78}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={win}\CalcProd
; DisableDirPage=yes
AlwaysShowDirOnReadyPage=yes
DefaultGroupName={#MyAppName}
AllowNoIcons=yes
OutputBaseFilename=CalcPro
Compression=lzma
SolidCompression=yes
DisableProgramGroupPage=yes
UsePreviousTasks=Yes

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "french"; MessagesFile: "compiler:Languages\French.isl"
Name: "german"; MessagesFile: "compiler:Languages\German.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
source: "E:\CalcPro\trunk\05_Release\CalcPro_Setup\Input\*"; destdir: "{win}\CalcPro"; flags: ignoreversion recursesubdirs createallsubdirs

[Dirs]

[Icons]

Name: "{group}\{#MyAppName}"; Filename: "{win}\CalcPro\{#MyAppExeName}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{win}\CalcPro\{uninstallexe}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{win}\CalcPro\{#MyAppExeName}"; Tasks: desktopicon
[Run]
Filename: "{win}\CalcPro\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: shellexec postinstall skipifsilent




