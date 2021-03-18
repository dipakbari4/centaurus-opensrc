Option Strict On
Option Explicit On

Imports System.Net
Imports System.IO.File

Friend Module StartupInitiator
    ''' <summary>
    ''' Creates all the necessary values if they are not found.
    ''' </summary>
    Private Sub CreateDataObjects()
        ' ----- Agreement -----

        If FetchData("Status3P0") = Nothing Then
            Agreement.ShowDialog()
        End If

        If FetchData("Warnings") = Nothing Then
            CountData("Warnings")
        End If

        ' ----- Edit Features -----

        If FetchData("MapDown Feature") = Nothing Then
            ImportData("MapDown Feature", "True")
        End If

        If FetchData("ChatUp Feature") = Nothing Then
            ImportData("ChatUp Feature", "True")
        End If

        If FetchData("LiveServer Feature") = Nothing Then
            ImportData("LiveServer Feature", "True")
        End If

        ' ----- Achievements -----

        If FetchData("Joined Game") = Nothing Or FetchData("Joined Game") = "" Then
            CountData("Joined Game")
        End If

        If FetchData("Logins") = Nothing Or FetchData("Logins") = "" Then
            CountData("Logins")
        End If

        If FetchData("Chats") = Nothing Or FetchData("Chats") = "" Then
            CountData("Chats")
        End If

        ' ----- Time Records -----

        If FetchData("CDays") = Nothing Then
            ImportData("CDays", "0")
        End If

        If FetchData("CHours") = Nothing Then
            ImportData("CHours", "0")
        End If

        If FetchData("CMinutes") = Nothing Then
            ImportData("CMinutes", "0")
        End If

        If FetchData("CSeconds") = Nothing Then
            ImportData("CSeconds", "0")
        End If

        ' ----- OTHERS -----

        If FetchData("Map History") = Nothing Then
            ImportData("Map History", "")
        End If

        If FetchData("Sent") = Nothing Then
            ImportData("Sent", "False")
        End If

        If FetchData("Admino") = Nothing Then
            ImportData("Admino", "False")
        End If

        If FetchData("OnStartup") = Nothing Then
            ImportData("OnStartup", "False")
        End If

        If FetchData("Last Update") = Nothing Then
            ImportData("Last Update", "Never")
        End If
    End Sub

    ''' <summary>
    ''' Validates whether the environment is optimal for IGI-2 or not.
    ''' </summary>
    Private Sub ValidateIGILocation()
        Load("Validating environment (2 of 10)")

        If IO.Path.GetFileNameWithoutExtension(
            Application.ExecutablePath) <> "Centaurus" Then
            Prompt("You are denied to rename the filename of Centaurus.",
                   "Rename Prohibition", 2)
            End
        End If

        If IO.Directory.Exists(Application.StartupPath + "\MISSIONS\multiplayer\common") And
            Exists(Application.StartupPath + "\" + path(0)) And
            Exists(Application.StartupPath + "\" + path(3)) Then
        Else
            GamePromo.ShowDialog()
            End
        End If
    End Sub

    ''' <summary>
    ''' Verifies if the agreement was correctly accepted by the user.
    ''' </summary>
    Private Sub AgreementSign()
        If FetchData("Status3P0") = Nothing Then
            Agreement.ShowDialog()
        End If

        If FetchData("Status3P0") = "True" Then
        Else
            Agreement.ShowDialog()
        End If
    End Sub

    ''' <summary>
    ''' Forces the user to install the required font.
    ''' </summary>
    Private Sub InstallFont()
        Const font1 As String = "Roboto"
        Const info As String =
            "'Roboto' font is not installed in your device." + vbCrLf + vbCrLf +
            "The Centaurus now uses Roboto font (Android's default). Close this" +
            " message to request you to install it before startup."
        Dim fonts() As String =
            {tempPath + "robo-reg.ttf", tempPath + "robo-bold.ttf"}

        Try
            If Not IsFontInstalled(font1) Then
                Dim proc As New Process()
                proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden

                proc.StartInfo.FileName = "taskkill.exe"

                Try
                    proc.StartInfo.Arguments = "/f /im fontview.exe"
                    proc.Start()
                Catch ex As Exception
                    ' Suppress Exception
                End Try

                Prompt(info, "Font Installation Required")

                WriteAllBytes(fonts(0), My.Resources.robo_regular)
                WriteAllBytes(fonts(1), My.Resources.robo_bold)

                Process.Start("fontview", fonts(0)).WaitForExit()
                Process.Start("fontview", fonts(1)).WaitForExit()

                Delete(fonts(0))
                Delete(fonts(1))

                Prompt("Restart your Centaurus to take effects.", "Centaurus Restart Required")
                End
            End If

        Catch ex As Exception
            Prompt(ex.Message, "Font Installation Error", 2)
            End
        End Try
    End Sub

    ''' <summary>
    ''' Checks whether the computer is connected to a network.
    ''' </summary>
    Private Sub AuthorizeNetwork()
        Const _IP As String = "www.google.com"
        Const _msg As String = "No network cards were found in your device." +
            " Ensure you have a proper adapter to connect to a network."
        Const _netError As String = "Hmm... seems like you have an internet" +
            " connection but it is improperly established. Please retry."

        Load("Checking connectivity (3 of 10)")

        If Not My.Computer.Network.IsAvailable Then
            Prompt(_msg, "No Network Connected", 3)
            Process.Start("ncpa.cpl")
            End
        End If

        Try
            If Not My.Computer.Network.Ping(_IP, 1500) Then
                Throw New Exception
            End If
        Catch ex As Exception
            Prompt(_netError, "You Are Offline", 2)
            End
        End Try

        Status(Main.Text + " Ready")
    End Sub

    ''' <summary>
    ''' Fetches all necessary data from I.G.I.-2 source.
    ''' </summary>
    Public Sub DefineVariables()
        KillSniffers()

        Load("Obtaining launcher data (4 of 10)")

        Try
            Dim _sData As String = StreamReader(nCTLN + "xip.txt")

            If _sData = "ERROR" Then
                Prompt("The fundamental server information was not received. Please try again!",
                       "Ouch! Network Error", 3)
                End
            End If

            ServerIP = New TripleDES(token).Decrypt(_sData)
            ServerPass = New TripleDES(token).Decrypt(StreamReader(nCTLN + "xpass.txt"))
            ServerPort = New TripleDES(token).Decrypt(StreamReader(nCTLN + "xport.txt"))

            If Not My.Computer.Network.Ping(ServerIP) Then
                ServerDownError.ShowDialog()
            End If

            modMode = StreamReader(nCTLN + "isModded.txt")
            whatsNew = StreamReader(nCTLN + "whatsnew.txt")
            cenStatus = StreamReader(nCTLN + "ServiceStatus.txt")
            latestVersion = StreamReader(nCTLN + "Version.txt")
            announce = StreamReader(nCTLN + "Announcement.txt")

        Catch ex As Sockets.SocketException
            Prompt("Unable to obtain the necessary information to start core features.",
                   "Core Features Not Loaded", 2)
            End
        Catch ex1 As WebException
        Catch ex2 As Exception
            Prompt(ex2.Message, "An Error Encountered", 2)
            End
        End Try
    End Sub

    ''' <summary>
    ''' Login security system core-feature.
    ''' </summary>
    Private Sub VerifyLogin()
        Load("Awaiting user authentication (5 of 10)")

        If FetchData("Pass Key").Length > 0 Then
            Main.CentoProtectorToolStripMenuItem.Checked = True
            Main.LinkLabel3.Visible = True

            If FetchData("Login Status") = "True" Then
                Prompt("The Centaurus was forcibly closed and unable to quit safely. " +
                           "Logging out this session.", "Logging Out", 2)
                ImportData("Login Status", "False")
            End If

            If FetchData("Login Status") = "False" Then
                Login.ShowDialog()
            End If

        Else FetchData("Pass Key")
            Main.CentoProtectorToolStripMenuItem.Checked = False
        End If

        If FetchData("Admino") = "True" Then
            Main.CentAdminoToolStripMenuItem.Enabled = True
            Main.CentAdminoToolStripMenuItem.Visible = True
            Main.ToolStripSeparator7.Visible = True
        End If
    End Sub

    ''' <summary>
    ''' Verifies if the user has been banned from the service.
    ''' </summary>
    Private Sub BanCheck()
        Load("Authorizing your access (6 of 10)")
        Dim _data As String =
            SystemSerialNumber.Replace(" ", "").Trim + "_" +
            Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER").Substring(0, 8).Replace(" ", "")
        Dim _phrase As String
        Dim _banText As String

        Try
            _banText = StreamReader(nCTLN + "/ban.txt")
            _phrase = New TripleDES(token).Encrypt(_data)

            warnings = CInt((_banText.Length - _banText.Replace(_phrase, String.Empty).Length) / _phrase.Length)

            If warnings > 0 Then
                BannedNotice.ShowDialog()
                End
            End If

            Return

        Catch ex1 As Exception
            Prompt("Authentication server responded code 400 Bad Request.",
                   "Bad Request – Server Responded", 2)
            End
        End Try

        Try
            If FetchData("First Run") = "True" Then
                Return
            End If

            _phrase = FetchData("Default Name")
            Dim _modBanText As String = pClient.DownloadString(server + "/mod_ban.txt")

            warnings = CInt((_modBanText.Length - _modBanText.Replace(_phrase, String.Empty).Length) / _phrase.Length)

            If warnings = 0 Then
                ImportData("Warnings", 0.ToString)
                Return
            End If

            If warnings > CInt(FetchData("Warnings")) Then
                ImportData("Warnings", warnings.ToString)
                Prompt("You have been warned by a moderator! You have " +
                       (3 - warnings).ToString + " chances left. Make your behavior better.",
                       "Centaurus Alert", 2)
            End If

            If warnings = 3 Then
                BannedNotice.ShowDialog()
                End
            End If

            Return
        Catch ex As Exception

        End Try

        Try
            _banText = pClient.DownloadString(server + "/launcher_ban.txt")
            _phrase = New TripleDES(token).Encrypt(_data)

            warnings = CInt((_banText.Length - _banText.Replace(_phrase, String.Empty).Length) / _phrase.Length)

            If warnings > 0 Then
                BannedNotice.ShowDialog()
                End
            End If

        Catch ex As Exception
            Prompt("Sorry, you are not authenticated to access the Centaurus systems.",
                   "Layer 3 Security Error", 3)
            End
        End Try
    End Sub

    ''' <summary>
    ''' Checks the service status for Centaurus.
    ''' </summary>
    Private Sub GetServiceStatus()
        Dim service As String = StreamReader(nCTLN + "ServiceStatus.txt")

        Select Case service
            Case "maintenance"
                Prompt("This service is under maintenance at the moment." +
                       " It will be back soon. Thank you for your patience.",
                       "Service in Maintenance")
            Case "server down"
                Prompt("The server is down right now by the admin for some reasons. " +
                       "It will be started very soon.", "Service Down Notice")
            Case "ERROR"
                Prompt("Unable to fetch status data. Dismiss to close.",
                       "Status Not Retrieved", 2)
            Case Else
                Return
        End Select

        End
    End Sub

    ''' <summary>
    ''' Shows the admin's response to the issue, issued by the user. DISABLED.
    ''' </summary>
    Private Sub PopResponses()
        Load("Checking inboxes (7 of 10)")

        Try
            If FetchData("First Run") = "False" And FetchData("Default Name").Length <> 0 Then
                Dim playerName As String = FetchData("Default Name").Replace(" ", "")

                For Each letter In deniedKeys
                    playerName = Strings.Replace(playerName, letter, "")
                Next

                If FetchData("Issue Opened") = Nothing Then
                    ImportData("Issue Opened", playerName.Replace(" ", "") + "_" + GetMacAddress())
                End If
            End If

            If FetchData("Sent") = "True" Then
                Dim pClient1 As WebClient = New WebClient With {
                    .Credentials = New NetworkCredential(us0, us1)}
                Dim data = New TripleDES("issue").Decrypt(
                    pClient1.DownloadString(server + "/Issues/" +
                    FetchData("Issue Opened").Replace(" ", "") + ".txt"))

                If Not data = FetchData("Report") Then
                    Prompt(FetchData("Report"), "Issue Sent by You")

                    Prompt(data, "Response From Admin – Issue Closed")
                    ImportData("Sent", "False")

                    With Main.ContactAnAdminToolStripMenuItem
                        .Visible = True
                        .Enabled = True
                    End With
                    Main.ToolStripSeparator10.Visible = True
                End If
            End If
        Catch ex As Exception
            '
        End Try
    End Sub

    ''' <summary>
    ''' Announcement management system.
    ''' </summary>
    Public Sub DisplayAnnounce()
        Load("Fetching announcements (8 of 10)")

        If FetchData("Announce") = announce Then
            Return
        End If

        If FetchData("First Run") = "True" Then
            ImportData("Announce", announce)
        End If

        If Not (announce = "ERROR" Or announce = "") Then
            GPrompt(announce, "Announcement")
            ImportData("Announce", announce)
        End If
    End Sub

    ''' <summary>
    ''' Verifies the existence of MapDown server.
    ''' </summary>
    Private Sub MapDownVerifier()
        Load("Preparing MapDown service (9 of 10)")

        Dim data As String = StreamReader(nCTLN + "MapDownStatus.txt")

        If data = "Disable" Or data = "ERROR" Then
            Return
        End If

        Try
            mapDownServer = "http://" + New TripleDES(token).Decrypt(StreamReader(nCTLN + "mip.txt")) + "/"
            mapList = StreamReader(mapDownServer + "load.txt").Split(CChar(vbCrLf))

            With Main.MapDownToolStripMenuItem
                .Enabled = True
                .Visible = True
            End With

        Catch ex As Exception
            Prompt("Unable to connect to the MapDown service.", "MapDown Error", 2)
        End Try
    End Sub

    ''' <summary>
    ''' Detects if somebody tries a failure authentication attempt in the device.
    ''' </summary>
    Private Sub CheckFailureAttempt()
        If FetchData("Failed Attempt").Length <> 0 Then
            PassChanger.ShowDialog()
        End If
    End Sub

    '''' <summary>
    '''' Locks the file from reading and writing.
    '''' </summary>
    'Private Sub LockFile()
    '	Try
    '		FileSt.Lock(0, fileLength)
    '	Catch Ex As Exception
    '		Prompt("Unable to execute the security statement.", "Security Fatal", 2)
    '		End
    '	End Try
    'End Sub

    '''' <summary>
    '''' Unlocks the file from reading and writing.
    '''' </summary>
    'Public Sub UnlockFile()
    '	Try
    '		FileSt.Unlock(0, fileLength)
    '	Catch Ex As Exception
    '		Prompt("Unable to execute the security statement.", "Security Fatal", 2)
    '		End
    '	End Try
    'End Sub

    ''' <summary>
    ''' Initializes the core-functionality features of the application.
    ''' </summary>
    Public Sub Initialize()
        Main.Hide()
        Loader.Show()

        RestrictOS()
        RestrictVM()
        CreateDataObjects()
        ValidateIGILocation()

        InstallFont()
        AgreementSign()

        AuthorizeNetwork()
        GetServiceStatus()
        DefineVariables()

        VerifyLogin()

        BanCheck()
        PopResponses()
        DisplayAnnounce()

        If FetchData("ChatUp Feature") = "True" Then
            Main.ChatClock.Start()

            Dim e As KeyEventArgs = Nothing
            Main.ChatClock_Tick(Main, e)
        End If

        If FetchData("MapDown Feature") = "True" Then
            MapDownVerifier()
        End If

        CheckForUpdates()
        CheckFailureAttempt()

        Loader.Hide()
        Main.Show()

        Main.Notifier.Visible = True
        Main.MainClock.Start()

        If FetchData("LiveServer Feature") = "True" Then
            Main.ServerData.Start()
        Else
            With Main
                .Label2.Visible = False
                .Label3.Visible = False
            End With
        End If

        Loader.Close()
    End Sub
End Module
