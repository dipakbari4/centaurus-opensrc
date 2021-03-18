Option Strict On
Option Explicit On

Friend Module Definition
    ' ---------- CORE DATA -------------
    Public ReadOnly CTLN As String = "YOUR_PATH"
    Public ReadOnly mainPath As String = Application.StartupPath
    Public ReadOnly token As String = "YOUR_TOKEN"
    Public ReadOnly tempPath As String =
        My.Computer.FileSystem.SpecialDirectories.Temp + "\"

    ' ---------- NETWORK DATA ----------
    Public ReadOnly nCTLN As String = "YOUR_WEB_SERVER"
    Public ReadOnly server As String = "YOUR_DATA_SERVER_FOR_MODERATION"

    Public ReadOnly us0 As String = New TripleDES _
        ("YOUR_SALT").Decrypt("YOUR_HASH")
    Public ReadOnly us1 As String = New TripleDES _
        ("YOUR_SALT").Decrypt("YOUR_PASSHASH")

    Public ReadOnly pClient As Net.WebClient = New Net.WebClient With {
    .Credentials = New Net.NetworkCredential(us0, us1)
    }
    Public mapList() As String = Nothing

    Public player As String = ""
    Public modMode As String = ""
    Public announce As String = ""
    Public latestVersion As String = ""
    Public chatServer As String = "YOUR_UDP_CHAT_SERVER"
    Public chatHostIPS As String = "YOUR_CHAT_IPS_FILE"
    Public mapDownServer As String = ""
    Public whatsNew As String = ""
    Public buttonEnabled As Boolean = True
    Public warnings As Integer = 0

    Public ReadOnly WebLinks() As String = {
        "YOUR_LINK_0",
        "YOUR_LINK_1",
        "YOUR_LINK_2",
        "YOUR_LINK_3",}

    Public cenStatus As String = ""
    Public ServerIP As String = ""
    Public ServerPort As String = ""
    Public ServerPass As String = ""
    Public chatMessage As String = ""

    ' ---------- OFFLINE DATA ----------

    'Private file As New IO.FileInfo(Application.ExecutablePath)
    'Public fileLength As Long = file.Length
    'Public FileSt As IO.FileStream =
    '	New IO.FileStream(Application.ExecutablePath, IO.FileMode.Create, IO.FileAccess.Write)
    Public errorOnAntiCheat As Boolean = False
    Public errorMessage As String = ""
    Public number As String = ""
    Public asker As DialogResult
    Public trigger As Boolean
    Public passwd As String = ""
    Public manualJoin As Boolean = False

    Public ReadOnly allowedKeys As String =
        "abcdefghijklmnopqrstuvwxyz" +
        "ABCDEFGHIJKLMNOPQRSTUVWXYZ, 0123456789" +
        "<>[]{}()`~!@#$%^&?*_+-=/\|.;:'"""

    Public ReadOnly deniedKeys As String =
        "\/:*?""<>|"

    Public ReadOnly path() As String = {
        "igi2.exe", "WEAPONS\ammo.qvm",
        "WEAPONS\weapon.qvm", "WEAPONS\weapons.res",
        "HUMANPLAYER\humanplayer.qvm"}

    Public ReadOnly backupLocation() As String = {
        "backup-igi2.exe", "WEAPONS\Backup\ammo.qvm",
        "WEAPONS\Backup\weapon.qvm", "WEAPONS\Backup\weapons.res",
        "HUMANPLAYER\backup-humanplayer.qvm"}
End Module
