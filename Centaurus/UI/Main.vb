Option Strict On
Option Explicit On

Imports System.Net
Imports System.Threading

Public Class Main
    Private mapName As String
    Private ign As String
    Private chats As Integer
    Private slowModer As Byte = 30
    Private firstRun As String
    Private gamePlayed As Boolean = False
    Private gameFlag As Boolean = False

    Private days As Integer = 0
    Private hours As Integer = 0
    Private minutes As Integer = 0
    Private seconds As Integer = 0

    Public P_days As Integer = 0
    Public P_hours As Integer = 0
    Public P_minutes As Integer = 0
    Public P_seconds As Integer = 0

    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If FetchData("First Run") = "True" Then
            Text += " (First Run)"
            BanCheck.Stop()
        End If

        Label1.Focus()

        Initialize()

        firstRun = FetchData("First Run")

        JoinText.ReadOnly = Not firstRun = "True"
        DoNotDisturbToolStripMenuItem.Visible = Not firstRun = "True"

        With EditFeaturesToolStripMenuItem
            .Visible = Not firstRun = "True"
            .Enabled = Not .Visible
        End With
        StartOnStartupToolStripMenuItem.Visible = firstRun = "False"
        ToolStripSeparator15.Visible = firstRun = "False"

        StartOnStartupToolStripMenuItem.Checked = FetchData("OnStartup") = "True"

        Dim expr As Boolean = FetchData("Default Name").Length > 0 And FetchData("ChatUp Feature") = "True"

        ChatUpToolStripMenuItem.Visible = expr
        ChatUpToolStripMenuItem.Enabled = expr
        DoNotDisturbToolStripMenuItem.Enabled = FetchData("ChatUp Feature") = "True"

        LinkLabel2.Visible = Not firstRun = "True"

        If firstRun = "True" Then
            ContactAnAdminToolStripMenuItem.Visible = False
            ToolStripSeparator10.Visible = False

            MyTrackToolStripMenuItem.Visible = False
            ToolStripSeparator6.Visible = False
            GoTo skip
        End If

        If FetchData("Sent") = "True" Then
            ToolStripSeparator10.Visible = False
            ContactAnAdminToolStripMenuItem.Visible = False
        End If

        Try
            If CBool(FetchData("Chats")) Then
                chats = CInt(FetchData("Chats"))
            End If
        Catch ex As Exception
            chats = 0
            ImportData("Chats", chats.ToString)
        End Try

        JoinAwardsSystem(CInt(FetchData("Joined Game")))
        ChatAwardsSystem(CInt(FetchData("Chats")))
        LoginAwardsSystem(CInt(FetchData("Logins")))

        If FetchData("Achievement") = "True" Then
            AchievementsToolStripMenuItem.Font =
                        New Font("Roboto", 9.0!, FontStyle.Bold)
            AchievementsToolStripMenuItem.ForeColor = Color.LightGreen
            AchievementsToolStripMenuItem.ToolTipText = "New achievement(s) unlocked!"
        End If

        Label7.Text = "Warmcome " + FetchData("Default Name")
        JoinText.Text = FetchData("Default Name")
skip:
        If FetchData("LiveServer Feature") = "True" Then
            ServerData_Tick(sender, e)
        End If

        P_days = CInt(FetchData("CDays"))
        P_hours = CInt(FetchData("CHours"))
        P_minutes = CInt(FetchData("CMinutes"))
        P_seconds = CInt(FetchData("CSeconds"))

        ActivenessRecords(P_days, P_hours, P_minutes)
    End Sub

    Private Sub Notifier_BalloonClick(sender As Object, e As EventArgs) Handles Notifier.BalloonTipClicked, Notifier.DoubleClick, Notifier.MouseDoubleClick
        If firstRun = "True" Then
            Return
        End If

        If FetchData("Pass Key") = Nothing Or
            FetchData("Login Status") = "True" Then

            Show()
            ShowInTaskbar = True
            WindowState = FormWindowState.Normal
        End If

        If trigger Then
            Status("Waiting mode was turned OFF")
            trigger = False
        End If
    End Sub

    Private Sub MainClock_Tick(sender As Object, e As EventArgs) Handles MainClock.Tick
        AuthenticateNetwork()

        Try
            GC.Collect()
        Catch ex As ArgumentOutOfRangeException
            ' Suppressed Exception
        End Try

        LogFlusher()

        Try
            Process.GetProcessesByName("java.exe")(0).Kill()
        Catch ex As Exception
            '
        End Try
    End Sub

    Friend Sub ServerData_Tick(sender As Object, e As EventArgs) Handles ServerData.Tick
        KillSniffers()
        Clipboard.Clear()

        Try
            player = GetPlayers(ServerIP, CInt(ServerPort))
            mapName = GetMapName()

        Catch ex As Exception
        End Try

        LinkLabel1.Visible = player = "0" Or player = ""

        Select Case player
            Case ""
                Label2.Text = "Updating players number"
                LinkLabel1.Visible = False
            Case "0"
                Label2.Text = "None is playing at the moment"
            Case "1"
                Label2.Text = "Someone is playing alone"

                If Not trigger Then
                    Exit Select
                End If

                Notify("Somebody has joined the game! Join immediately.",
                       "Waiting mode is OFF")
                trigger = False

                Show()
                ShowInTaskbar = True

            Case "2"
                Label2.Text = "Two players are in right now"
            Case "-2"
                Label2.Text = "Updating players number"
                LinkLabel1.Visible = False
            Case Else
                Label2.Text = "Currently " + player + " players are playing"
        End Select

        If mapName = "" Then
            Label3.Text = "Getting current map"
        Else
            Label3.Text = "Current Map: " + mapName
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles JoinButton.Click
        'Try
        Try
            Process.GetProcessesByName("igi2")(0).Kill()
        Catch ex As Exception
        End Try

        JoinGame(ServerIP, ServerPort, ServerPass)

        'Catch ex As Exception
        '    Prompt(ex.Message, "Join Failed", 3)
        'End Try
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles JoinText.TextChanged
        JoinButton.Enabled = Not String.IsNullOrWhiteSpace(JoinText.Text)

        Select Case firstRun
            Case "False"
                If String.IsNullOrWhiteSpace(JoinText.Text) Then
                    Label7.Text = "Warmcome! Let's Play..."
                ElseIf JoinText.Text = "" And FetchData("First Run") = "False" Then
                    Label7.Text = "Warmcome to the Centaurus"
                Else
                    Label7.Text = "Warmcome " + JoinText.Text
                End If
        End Select
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        JoinWaiter.ShowDialog()
    End Sub

    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles JoinText.KeyPress
        DetectKeys(sender, allowedKeys, e)
    End Sub

    Private Sub ChatUpToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ChatUpToolStripMenuItem.Click
        Try
            If FetchData("Default Name").Length > 0 Then
                ChatUp.ShowDialog()
            End If
        Catch ex As Exception
            '
        End Try
    End Sub

    Private Sub MapDownToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MapDownToolStripMenuItem.Click
        MapDown.ShowDialog()
    End Sub

    Private Sub JoinAServerManuallyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ManSerJoinerToolStripMenuItem.Click
        ManualJoiner.ShowDialog()
    End Sub

    Private Sub CentoProtectorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CentoProtectorToolStripMenuItem.Click
        If Not CentoProtectorToolStripMenuItem.Checked Then
            PasswordVerifier.ShowDialog()

            If passwd = FetchData("Pass Key") Then
                ImportData("Pass Key", "")
                ImportData("Pass Hint", "")
                ImportData("Login Status", "False")
                ImportData("Last Login", FetchData("Last Login") + Environment.NewLine)

                Prompt("Password was reset successful.", "Reset Complete")
                Application.Restart()
                Application.ExitThread()
            Else
                Prompt("Password verification failed. Aborted password reset.",
                       "Password Mismatched", 2)
                CentoProtectorToolStripMenuItem.Checked = True
            End If
        Else
            CentoProtectorToolStripMenuItem.Checked = False
            CentoProtector.ShowDialog()
        End If
    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutCentaurusToolStripMenuItem.Click
        About.ShowDialog()
    End Sub

    Private Sub AnnouncementToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AnnouncementToolStripMenuItem.Click
        If announce = "ERROR" Then
            Prompt("Unable to fetch announcement data.", "Data Not Received", 2)
        ElseIf announce.Length > 5 Then
            GPrompt(announce, "Announcement")
        Else
            Prompt("There are no announcement. But stay tuned!", "No Announces Yet")
        End If
    End Sub

    Private Sub CodeOfConductToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CodeOfConductToolStripMenuItem.Click
        GPrompt(My.Resources.agreement, "Code of Conduct")
    End Sub

    Private Sub ReportACheaterOrHackerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReportToolStripMenuItem.Click
        Process.Start(WebLinks(2))
    End Sub

    Private Sub FeedbackCentaurusToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FeedbackCentaurusToolStripMenuItem.Click
        Process.Start(WebLinks(3))
    End Sub

    Private Sub VisitOurWebsiteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VisitOurWebsiteToolStripMenuItem.Click
        Process.Start(WebLinks(0))
    End Sub

    Private Sub JoinUsOnFacebookToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles JoinUsOnFacebookToolStripMenuItem.Click
        Process.Start(WebLinks(1))
    End Sub

    Private Sub Main_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Try
            If FetchData("Pass Key") = Nothing Then
                Process.GetProcessesByName("igi2")(0).Kill()
            Else
                If FetchData("Login Status") = "True" Then
                    asker =
                        MessageBox.Show("Do you wish to sign out and quit?", "Sign Out Confirmation",
                               MessageBoxButtons.YesNo, MessageBoxIcon.Information)

                    Select Case asker
                        Case DialogResult.Yes
                            Notifier.Visible = False
                            ImportData("Login Status", "False")

                            IO.File.WriteAllText(mainPath + "\igi2.RPT", "")
                            LogFlusher()

                            Process.GetProcessesByName("igi2")(0).Kill()
                        Case Else
                            e.Cancel = True
                            Notifier.Visible = True
                    End Select
                End If
            End If
        Catch ex As Exception
            '
        End Try
    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        GPrompt(FetchData("Last Joined"), LinkLabel2.Text +
                " – Joined " + FetchData("Joined Game") + " time(s)")
    End Sub

    Private Sub LinkLabel3_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel3.LinkClicked
        GPrompt(FetchData("Last Login"), LinkLabel3.Text +
                " – Logged in " + FetchData("Logins") + " time(s)")
    End Sub

    Private Sub LinkLabel7_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles Reconnect.LinkClicked
        MainClock.Stop()
        ServerData.Stop()
        Status("Services stopped, retrying to connect", 2)

        MainClock.Start()
        GC.Collect()

        ServerData.Start()
    End Sub

    Public Sub ChatClock_Tick(sender As Object, e As EventArgs) Handles ChatClock.Tick
        Dim _rawChat As String = StreamReader(chatServer)

        If _rawChat = "ERROR" Or _rawChat = "" Then
            Return
        End If

        Dim chatMessage As String = New TripleDES("rtr").Decrypt(_rawChat)

        If Not chatMessage = FetchData("History") Then
            ImportData("History", chatMessage)
            ChatUp.ChatBox.Text = chatMessage

            If Application.OpenForms().OfType(Of ChatUp).Any Then
                ChatUp.UpdateChat()
                Return
            End If

            Notify("You have new unreads in ChatUp.", "New messages")
        End If
    End Sub

    Private Sub CurrentStatusToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MyTrackToolStripMenuItem.Click
        MyTrack.ShowDialog()
    End Sub

    Private Sub Label7_Click(sender As Object, e As EventArgs) Handles Label7.Click
        Label7.ForeColor = GenerateRandomColor()
    End Sub

    Private Sub Label7_DoubleClick(sender As Object, e As EventArgs) Handles Label7.DoubleClick
        Label7.ForeColor = Color.FromArgb(255, 153, 51)
    End Sub

    Private Sub AwardsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AchievementsToolStripMenuItem.Click
        Try
            ImportData("Achievement", "False")
            AchievementsToolStripMenuItem.Font = New Font("Roboto", 9.0!, FontStyle.Regular)
            AchievementsToolStripMenuItem.ForeColor = Color.FromArgb(255, 192, 128)
            AchievementsToolStripMenuItem.ToolTipText =
                "Unlock new achievements, honors and medals to gain reputation."
        Catch ex As Exception
        Finally
            ImportData("Achievement", "False")
        End Try
    End Sub

    Private Sub TextBox1_Enter(sender As Object, e As EventArgs) Handles JoinText.Enter
        If FetchData("Admino") = "True" Or FetchData("Default Name") = "" Then
            JoinText.ReadOnly = False
            Return
        End If

        If CInt(FetchData("Joined Game")) Mod 250 = 0 Then
            ImportData("Rename", "True")
            Prompt("You may change your name once now!", "Name Changing Policy")
        End If

        If FetchData("Rename") = "False" Then
            JoinText.ReadOnly = True
            Prompt("You can change your name every 250 joins.", "Name Changing Policy")
        Else
            JoinText.ReadOnly = False
        End If
    End Sub

    Private Sub SlowModeCounter_Tick(sender As Object, e As EventArgs) Handles SlowModeCounter.Tick
        slowModer -= CByte(1)
        Label5.Text = "Wait " + slowModer.ToString + " seconds"

        If Not slowModer = 0 Then
            Return
        End If

        Label5.Visible = False
        SlowModeCounter.Stop()
        JoinButton.Enabled = True
        Label5.Text = "Slowmode Active!"
        slowModer = 30
    End Sub

    Private Sub HelpForAchievementsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AchievementHelpToolStripMenuItem.Click
        GPrompt(My.Resources.achievementHelp, "Achievement Help")
    End Sub

    Private Sub RestartCentaurusToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RestartCentaurusToolStripMenuItem.Click
        ImportData("Login Status", "False")

        Application.Restart()
        Application.ExitThread()
    End Sub

    Private Sub CentAdminoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CentAdminoToolStripMenuItem.Click
        If FetchData("Admino") = "True" Then
            CenAdmino.ShowDialog()
        End If
    End Sub

    Private Sub BanCheck_Tick(sender As Object, e As EventArgs) Handles BanCheck.Tick
        Dim pClient As WebClient = New WebClient With {
            .Credentials = New NetworkCredential(us0, us1)
        }

        Dim localT As New Thread(
            New ThreadStart(
            Sub()
                Try
                    Dim text As String = pClient.DownloadString(server + "/mod_ban.txt")

                Catch ex As Exception
                    GC.Collect()
                    Return
                End Try

                Dim phrase As String = FetchData("Default Name")
                warnings = CInt((Text.Length - Text.Replace(phrase, String.Empty).Length) / phrase.Length)

                If warnings >= 3 Then
                    MainClock.Stop()
                    ChatClock.Stop()
                    ServerData.Stop()

                    Hide()
                    BannedNotice.ShowDialog()
                    End
                End If
            End Sub)
            )
        localT.Start()
    End Sub

    Private Sub WallpaperToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles WallpaperToolStripMenuItem.Click
        If WallpaperToolStripMenuItem.Checked Then
            Dim resources As ComponentModel.ComponentResourceManager =
                New ComponentModel.ComponentResourceManager(GetType(Main))
            BackgroundImage = CType(resources.GetObject("$this.BackgroundImage"), Image)
        Else
            BackgroundImage = Nothing
        End If

        Status("Wallpaper was toggled successfully")
    End Sub

    Private Sub DoNotDisturbToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DoNotDisturbToolStripMenuItem.Click
        GC.Collect()

        If DoNotDisturbToolStripMenuItem.Checked Then
            ChatUpToolStripMenuItem.Visible = False
            ChatUpToolStripMenuItem.Enabled = False
            ChatClock.Stop()
        Else
            ChatUpToolStripMenuItem.Visible = True
            ChatUpToolStripMenuItem.Enabled = True
            ChatClock.Start()
        End If

        Status("Do Not Disturb mode has been " +
               If(DoNotDisturbToolStripMenuItem.Checked, "activated", "turned OFF"))
    End Sub

    Private Sub ClipToTopToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ClipToTopToolStripMenuItem.Click
        TopMost = ClipToTopToolStripMenuItem.Checked
        Status("Clip to Top has turned " + If(TopMost, "ON", "OFF"))
    End Sub

    Private Sub GameClock_Tick(sender As Object, e As EventArgs) Handles GameClock.Tick
        If Not gameFlag Then
            ign = Label7.Text
            gameFlag = True
        End If

        If Not Process.GetProcessesByName("igi2").Count > 0 Then
            If gamePlayed Then
                Label7.Text =
                    "Recorded uptime: " + days.ToString + "d " + hours.ToString + "h " +
                    minutes.ToString + "m " + seconds.ToString + "s"

                ImportData("CDays", P_days.ToString)
                ImportData("CHours", P_hours.ToString)
                ImportData("CMinutes", P_minutes.ToString)
                ImportData("CSeconds", P_seconds.ToString)

                ActivenessRecords(
                    CInt(FetchData("CDays")),
                        CInt(FetchData("CHours")),
                            CInt(FetchData("CMinutes")))

                gamePlayed = False
            End If

            days = 0
            hours = 0
            minutes = 0
            seconds = 0

            GameClock.Stop()

            Label7.Text = ign
            Return
        Else
            gamePlayed = True
        End If

        seconds += 1
        P_seconds += 1

        If P_seconds = 60 Then
            P_seconds = 0
            P_minutes += 1
        End If

        If P_minutes = 60 Then
            P_minutes = 0
            P_hours += 1
        End If

        If P_hours = 24 Then
            P_hours = 0
            P_days += 1
        End If

        ' ---

        If seconds = 60 Then
            seconds = 0
            minutes += 1
        End If

        If minutes = 60 Then
            minutes = 0
            hours += 1
        End If

        If hours = 24 Then
            hours = 0
            days += 1
        End If

        Label7.Text = If(ShowTotalGameplayTimeToolStripMenuItem.Checked,
            days.ToString + "d " + hours.ToString + "h " +
            minutes.ToString + "m " + seconds.ToString + "s", ign)
    End Sub

    Private Sub FAQToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FAQToolStripMenuItem.Click
        FAQ.ShowDialog()
    End Sub

    Private Sub ContactAnAdminToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ContactAnAdminToolStripMenuItem.Click
        ContactAdmin.ShowDialog()
    End Sub

    Private Sub SetFeaturesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EditFeaturesToolStripMenuItem.Click
        SetFeatures.ShowDialog()
    End Sub

    Private Sub StartOnStartupToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StartOnStartupToolStripMenuItem.Click
        Dim path As String = "SOFTWARE\Microsoft\Windows\CurrentVersion\Run"

        If StartOnStartupToolStripMenuItem.Checked Then
            My.Computer.Registry.LocalMachine.OpenSubKey(path, True).
                SetValue(Application.ProductName, Application.ExecutablePath)
            ImportData("OnStartup", "True")
        Else
            My.Computer.Registry.LocalMachine.OpenSubKey(path, True).
                DeleteValue(Application.ProductName)
            ImportData("OnStartup", "False")
        End If

        Status(If(StartOnStartupToolStripMenuItem.Checked, "Marked", "Unmarked") + " to initiate on startup")
    End Sub
End Class
