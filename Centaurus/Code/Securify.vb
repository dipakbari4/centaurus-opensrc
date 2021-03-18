Option Strict On
Option Explicit On

Imports System.IO
Imports System.Text
Imports System.Security.Cryptography

Friend Module Securify
    Private m_mainWmi As Object
    Private m_deviceLists As Collection

    ' -------------------- RESTRICT NO-VIRTUAL-MACHINE --------------------

    ''' <summary>
    ''' Detects and prevent user from using a virtual machine.
    ''' </summary>
    Public Sub RestrictVM()
        Try
            Dim BIOS As String =
            GetWmiDeviceSingleValue("Win32_BIOS", "SerialNumber")

            Dim s As String = "VM"
            Dim index As Short = CShort(BIOS.IndexOf(s))

            If index = -1 Then
            Else
                Prompt("This program is not designed to run in Virtual Machines.",
                       "Virtual Machines Prohibited", 2)
                End
            End If
        Catch ex As Exception
            '
        End Try
    End Sub

    Private Function GetMainWMIObject() As Object
        On Error GoTo GetMainWMI

        If m_mainWmi Is Nothing Then
            m_mainWmi = GetObject("WinMgmts:")
        End If

        GetMainWMIObject = m_mainWmi
        Exit Function

GetMainWMI:
        GetMainWMIObject = Nothing
    End Function

    Public ReadOnly Property WmiIsAvailable As Boolean
        Get
            WmiIsAvailable = GetMainWMIObject() IsNot Nothing
        End Get
    End Property

    Public Function GetWmiDeviceSingleValue(WmiClass As String, WmiProperty As String) As String
        On Error GoTo GetWmiDevice
        Dim result As String = ""
        Dim wmiclassObjList As IEnumerable

        wmiclassObjList = CType(GetWmiDeviceList(WmiClass), IEnumerable)
        Dim wmiclassObj As Object

        For Each wmiclassObj In wmiclassObjList
            result = CType(CallByName(wmiclassObj, WmiProperty, vbGet), String)
            Exit For
        Next
GetWmiDevice:
        GetWmiDeviceSingleValue = Trim(result)
    End Function

    Public Function GetWmiDeviceList(WmiClass As String) As Object
        If m_deviceLists Is Nothing Then
            m_deviceLists = New Collection
        End If
        On Error GoTo fetchNew
        GetWmiDeviceList = m_deviceLists.Item(WmiClass)

        Exit Function

fetchNew:
        Dim devList As Object
        devList = GetWmiDeviceListInternal()

        If devList IsNot Nothing Then
            Call m_deviceLists.Add(devList, WmiClass)
        End If
        GetWmiDeviceList = devList
    End Function

    Private Function GetWmiDeviceListInternal() As Object
        Try
            Return GetMainWMIObject()

        Catch ex As Exception
        End Try

        Return Nothing
    End Function

    ''' <summary>
    ''' Detects and prevent user from running a non-Windows.
    ''' </summary>
    Public Sub RestrictOS()
        Try
            Dim OS As String = My.Computer.Info.OSFullName

            If Not OS.Contains("Windows") Then
                Prompt("This program is only supported in Windows platform.",
                       "Operating System Unsupported", 2)
                End
            End If
        Catch ex As Exception
            ' 
        End Try
    End Sub

    ''' <summary>
    ''' Applies the anti-cheat system for the specified map.
    ''' </summary>
    ''' <param name="mapName">Name of the specified map</param>
    ''' <param name="resource">Anti-cheat data</param>
    Public Sub WACF(mapName As String, resource As Byte())
        Dim mName As String = mapName
        Dim mLocation As String = tempPath + mName + ".zip"
        Dim mInstall As String = mainPath + "\MISSIONS\multiplayer\" + mName + "\"

        Try
            File.SetAttributes(mLocation, FileAttributes.Normal)

            If File.Exists(mLocation) Then
                File.Delete(mLocation)
            End If
        Catch ex As Exception
            ' Suppressed Exception
        End Try

        If File.Exists(mInstall + "objects.qvm") And File.Exists(mInstall + "mission.qvm") Then
            File.SetAttributes(mInstall + "objects.qvm", FileAttributes.Normal)
            File.SetAttributes(mInstall + "mission.qvm", FileAttributes.Normal)

            File.Delete(mInstall + "objects.qvm")
            File.Delete(mInstall + "mission.qvm")
        End If

        Try
            If mapName = "mplocation2\LEVEL3" Then
                mLocation = tempPath + "mplocation2.zip"
            ElseIf mapName = "Island Assault\mplocation3" Then
                mLocation = tempPath + "mplocation3.zip"
            End If

            File.WriteAllBytes(mLocation, resource)
            File.SetAttributes(mLocation, FileAttributes.Hidden)

            Dim t1 As New Threading.Thread(
                New Threading.ThreadStart(
                    Sub()
                        Compression.ZipFile.ExtractToDirectory(mLocation, mInstall)
                    End Sub
                )
            )
            t1.Start()
            t1.Join()

            File.Delete(mLocation)

        Catch ex1 As DirectoryNotFoundException
        Catch ex As Exception
            errorOnAntiCheat = True
            Prompt(ex.Message, "Anti-Cheat Write Failure", 3)
        End Try
    End Sub

    ''' <summary>
    ''' Checks if the input in the text box contains an offensive name.
    ''' </summary>r
    ''' <param name="TB">The textbox to be verified</param>
    ''' <returns>Boolean</returns>
    Public Function FilterAbuse(TB As String) As Boolean
        Dim keywords() As String =
            {"motherfucker", "fuck", "bitch", "shit", "asshole", "bastard",
            "jackass", "dick", "penis", "dumb", "nigga", "cunt",
            "bsdk", "kamina", "kutta", "suar", "madarchod", "chod",
            "chodu", "gaandu", "chutiya", "chut", "vagina", "fucker",
            "muchchar", "maaka", "chaman", "ass", "badass",
            "lund", "Bose D.K.", "fk", "lawda", "lassan", "lesadiya",
            "benchod", "bencho", "bakchod", "randi", "rndi", "bhosdik",
            "F*ck", "Shit", "Piss", "Dick", "bollocks",
            "Bloody", "Choad", "Crikey", "suck", "sex", "bellend",
            "Balls", "Hilao", "Hilalo", "Hillao", "hilao", "lallu",
            "lallua", "heck", "stfu", "Hek you",
            "Mierda", "Váyase a la Mierda", "Que te Folle un Pez",
            "Puto", "Verga", "Cazzo", "Palle", "Che Palle", "Tette",
            "Stronzo", "Fanculo", "Vaffanculo", "Pompinara", "Cocksucker",
            "Osti de Calisse de Tabarnak", "Arschgesicht", "Scheißkopf",
            "Küss meinen Arsch", "Verpiss Dich!", "Zur Hölle mit…",
            "Wichser", "Wanker", "Arschgeige", "Dickhead", "Cabra",
            "Cabrão", "Monte de Merda", "Caralho",
            "Vai Para o Caralho", "Rego Do Cu", "Puta Que Pariu",
            "Chupa-mos", "Holy Shit", "Xуй", "Хуй тебе́!", "Cучка",
            "Oбосра́ться", "Pants", "Не будь жо́пой!", "блядь",
            "Whore", "Да ебал я это!", "王八蛋", "混蛋", "Dog",
            "Fart", "狗屁", "混帳", "他妈的", "去你的", "Shut the",
            "我肏", "What the ****", "f*ck", "fu*k", "fuc*",
            "くそ", "やりまん", "くそったれ", "ぶす", "死ねえ", "くたばれ、ボケ",
            "아, 씨발", "씨방새", "개새", "년", "boob", "좆", "좆 같은 놈/년",
            "bastar*", "아, 좆같네", "fuc", "fck", "fuk", "fak", "b*st*rd",
            "basta*d", "b**tard", "fukiest", "goti", "tatti", "patthe",
            "Damm", "D*mm", "*astard*", "Badir", "Badirchand", "Bakland",
            "Bhandava", "bhutnike", "Chinaal", "Chup kar", "Chutia",
            "Chutiya", "choo-tia", "chutan", "Ghasti", "Gashti", "gasti",
            "ghassad", "Haram", "Hijda", "Hijra", "jaanwar", "Kuttiya", "Khota", "Hijra",
            "Kutte", "Najayaz", "Saala", "Saali", "Soover", "gaand", "gandu",
            "Jhaat ka", "fat gayi", "bahenchod", "Bahen Ch*d", "Bahen ke",
            "Bahen ka", "Beti ke", "Beti ka", "Bhains ki aulad", "jhalla",
            "jhant", "jhaat", "Maadher chod", "Raand ka", "Randhwa",
            "Rundi", "ullu", "chod", "Chut", "Gaand", "Chipkali ke",
            "chut", "Choot marani ka", "Choot", "Chunni", "Chudai", "Chodu",
            "Cuntmama", "Backar", "Bhand khau", "Bhandwe", "Bhosad", "chodela",
            "Gaandu", "gaandmasti", "Maa ke", "Muth", "Parichod", "Pucchi",
            "Raandi", "Rundi", "Sada hua", "Sadi hui gand",
            "Tera adha Nirodh main rah gaya", "apna land", "Muthi",
            "Apni ma ko", "Chullu bhar muth", "Jaa Apni Bajaa",
            "Ma chudi", "Tor mai ke", "Teri ma ki", "Tera ma Ki", "Betio ke",
            "biwi", "Teri behan ka", "Teri gand"}

        For Each word In keywords
            If TB.Contains(word) Then
                Return False
            End If
        Next

        Return True
    End Function

    ' -------------------- ENCRYPTION ALGORITHM - TRIPLE DES -------------------

    Public NotInheritable Class TripleDES
        Private ReadOnly TripleDes As New TripleDESCryptoServiceProvider
        Private Function TruncateHash(key As String, length As Short) As Byte()
            Dim sha1 As New SHA1CryptoServiceProvider

            Dim keyBytes() As Byte = Encoding.Unicode.GetBytes(key)
            Dim hash() As Byte = sha1.ComputeHash(keyBytes)

            ReDim Preserve hash(length - 1)
            Return hash
        End Function

        Sub New(key As String)
            TripleDes.Key = TruncateHash(key, CShort(TripleDes.KeySize \ 8))
            TripleDes.IV = TruncateHash("", CShort(TripleDes.BlockSize \ 8))
        End Sub

        ''' <summary>
        ''' Encrypts the text to a valid cipher text.
        ''' </summary>
        ''' <param name="plainText">Text to be encrypted</param>
        ''' <returns>String</returns>
        Public Function Encrypt(plainText As String) As String
            Try
                Dim plaintextBytes() As Byte =
                Encoding.Unicode.GetBytes(plainText)

                Dim ms As New MemoryStream
                Dim encStream As New _
                    CryptoStream(ms, TripleDes.CreateEncryptor(),
                    CryptoStreamMode.Write)

                encStream.Write(plaintextBytes, 0, plaintextBytes.Length)
                encStream.FlushFinalBlock()

                Return Convert.ToBase64String(ms.ToArray)

            Catch ex As Exception
                ' Silent
            End Try

            Return ""
        End Function

        ''' <summary>
        ''' Decrypts the data encrypted into a plain text.
        ''' </summary>
        ''' <param name="eText">The cipher text</param>
        ''' <returns>String</returns>
        Public Function Decrypt(eText As String) As String
            Try
                Dim encryptedBytes() As Byte = Convert.FromBase64String(eText)
                Dim ms As New MemoryStream
                Dim decStream As New _
                    CryptoStream(ms, TripleDes.CreateDecryptor(),
                    CryptoStreamMode.Write)

                decStream.Write(encryptedBytes, 0, encryptedBytes.Length)
                decStream.FlushFinalBlock()

                Return Encoding.Unicode.GetString(ms.ToArray)

            Catch ex As Exception
                ' Silent
            End Try

            Return ""
        End Function
    End Class

    ' -------------------- NETWORK SECURITY --------------------

    ''' <summary>
    ''' Kills all the known sniffing programs.
    ''' </summary>
    Public Sub KillSniffers()
        Dim _proc As Process()
        Dim arrSniffers() As String = {
        "Wireshark", "procexp64",
        "procexp", "dumpcap", "tcpdump",
        "Ettercap", "dSniff", "WinDump",
        "Ntop", "NetStumbler", "kismet",
        "ngrep", "etterape", "TShark", "HPA Launcher"}

        Try
            For Index = 0 To arrSniffers.Length - 1
                _proc = Process.GetProcessesByName(arrSniffers(Index))

                If _proc.Length > 0 Then
                    _proc(0).Kill()
                    CountData("Trapped")
                End If
            Next

        Catch ex As Exception
            '
        End Try

        Dim _commands() As String = {"java.exe", "cmd.exe", "python.exe"}

        Dim proc As New Process()
        proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden

        proc.StartInfo.FileName = "taskkill.exe"

        For Each cmdIn In _commands
            Try
                proc.StartInfo.Arguments = "/f /im " + cmdIn
                proc.Start()
            Catch ex As Exception
                ' Suppress Exception
            End Try
        Next
    End Sub

    ''' <summary>
    ''' Flushes the server log data secretly.
    ''' </summary>
    Public Sub LogFlusher()
        Try
            Dim logPath As String = mainPath + "\igi2.RPT"
            File.WriteAllText(logPath, "igi2.exe RPT mon")

        Catch ex As Exception
            '
        End Try
    End Sub
End Module