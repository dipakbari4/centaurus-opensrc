Option Strict On
Option Explicit On

Imports System.IO
Imports System.IO.File
Imports System.Net

Friend Module GameJoiner
    Private command As String = ""
    Private sPlayer As String = ""
    Private joinedGame As Integer = 0
    Private countJoiner As Boolean
    Private firstRunCatcher As String = ""
    Private mapName As String = ""

    ''' <summary>
    ''' The most important part of Centaurus, the game joiner system.
    ''' </summary>
    ''' <param name="sIP">IP of the server</param>
    ''' <param name="sPort">Port of the server</param>
    ''' <param name="sPass">Password of the server</param>
    Public Sub JoinGame(sIP As String, sPort As String, sPass As String)
        Main.JoinButton.Enabled = False
        player = If(player = "", 0.ToString, player)

        If CInt(player) < 0 Or player = "" Then
            asker = MessageBox.Show("The server is not connected." +
                " Would you still like to join?", "Server Not Connected",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Information)

            Select Case asker
                Case DialogResult.No
                    Main.JoinButton.Enabled = True
                    Return
                Case Else
                    countJoiner = False
                    Main.JoinButton.Enabled = False
                    GoTo pf_ignoreAntiCheat
            End Select
        Else
            countJoiner = True
        End If

        mapName = GetMapName()

        If mapName = "" Or mapName = Nothing Or mapName = "-1" Then
            Prompt("The map name was not loaded. Make sure you are able to see the current map name in status.",
                   "Map Name Not Loaded", 2)
            Main.JoinButton.Enabled = True
            Return
        End If

        If mapName.Contains("Sandstorm") Then
            WACF("sandstorm", My.Resources.sandstorm)
        ElseIf mapName.Contains("Redstone") Then
            WACF("redstone", My.Resources.redstone)
        ElseIf mapName.Contains("Forestraid") Then
            WACF("forestraid", My.Resources.forestraid)
        ElseIf mapName.Contains("Timberland") Then
            WACF("timberland", My.Resources.timberland)
        ElseIf mapName.Contains("Chinese temple") Then
            WACF("chinesetemple", My.Resources.chinesetemple)
        ElseIf mapName.Contains("Pribois villa") Then
            WACF("mplocation2\LEVEL3", My.Resources.mplocation2)
        ElseIf mapName.Contains("Pribois villa - Future") Then
            WACF("pribois villa_2018", My.Resources.pribois_villa_2018)
        ElseIf mapName.Contains("Pribois Villa-[Night]") Then
            WACF("Pribois_Villa-Night", My.Resources.Pribois_Villa_Night)
        ElseIf mapName.Contains("Homecoming") Then
            WACF("Homecoming", My.Resources.Homecoming)
        ElseIf mapName.Contains("Dockbeta") Then
            WACF("Dockbeta", My.Resources.Dockbeta)
        ElseIf mapName.Contains("Docklands") Then
            WACF("docklands", My.Resources.docklands)
        ElseIf mapName.Contains("Dockside") Then
            WACF("dockside", My.Resources.dockside)
        ElseIf mapName.Contains("Bridge Across the Dnestr") Then
            WACF("mplocation1\level4", My.Resources.mplocation1)
        ElseIf mapName.Contains("Island Assault") Then
            WACF("mplocation3\level1", My.Resources.mplocation3)
        ElseIf mapName.Contains("NO_NAME_CITY") Then
            WACF("NO_NAME_CITY", My.Resources.NO_NAME_CITY)
        ElseIf mapName.Contains("Winterland") Then
            WACF("winterland", My.Resources.winterland)
        ElseIf mapName.Contains("Sunny Chinese Temple") Then
            WACF("chintemple2", My.Resources.chintemple2)
        ElseIf mapName.Contains("Its Raining Men") Then
            WACF("RainingMen", My.Resources.RainingMen)
        ElseIf mapName.Contains("Prison Escape") Then
            WACF("Prison Escape", My.Resources.Prison_Escape)
        ElseIf mapName.Contains("Aim map") Then
            WACF("Aim map", My.Resources.Aim_map)
        ElseIf mapName.Contains("Factory 2") Then
            WACF("Factory2", My.Resources.Factory2)
        ElseIf mapName.Contains("Jungle X") Then
            WACF("JungleX", My.Resources.JungleX)
        ElseIf mapName.Contains("THE_AIRFIELD_2019") Then
            WACF("THE_AIRFIELD_2019", My.Resources.THE_AIRFIELD_2019)
        ElseIf mapName.Contains("Ultimate War") Then
            WACF("Ultimate Warfare", My.Resources.Ultimate_Warfare)
        ElseIf mapName.Contains("White Wood") Then
            WACF("White Wood", My.Resources.White_Wood)
        ElseIf mapName.Contains("Secret Hideout") Then
            WACF("Secret Hideout", My.Resources.Secret_Hideout)
        ElseIf mapName.Contains("Haunted Village") Then
            WACF("Haunted Village", My.Resources.Haunted_Village)
        ElseIf mapName.Contains("Deep in the Mines") Then
            WACF("DeepInMines", My.Resources.DeepInMines)
        ElseIf mapName.Contains("Ghostcity") Then
            WACF("ghostcity", My.Resources.ghostcity)
        ElseIf mapName.Contains("Libyan_Rendezvous_2018") Then
            WACF("Libyan_Rendezvous", My.Resources.Libyan_Rendezvous)
        End If

        If errorOnAntiCheat Then
            Status("Critical error occurred during launch", 2)
            Main.JoinButton.Enabled = True
            Return
        End If

pf_ignoreAntiCheat:
        Main.Label5.Visible = True
        Main.SlowModeCounter.Start()

        sPlayer = Main.JoinText.Text
        'sPlayer = sPlayer.Replace(" ", " ")

        'MsgBox(sPlayer)

        command = "ip" + sIP + " port" + sPort + " player""" +
            sPlayer + """ password""" + sPass + """"

        If Not FilterAbuse(Main.JoinText.ToString) Then
            CountData("Abusive Count")

            If (CInt(FetchData("Abusive Count")) > 5) Then
                Prompt("You do not deserve gaming anymore. Game End, Tata!",
                       "Vulgarity Exceeded", 2)
                Return
            End If

            Prompt("You cannot continue with an offensive name." +
                   (5 - CInt(FetchData("Abusive Count"))).ToString + " warnings left.",
                   "Offensive Name Prohibited", 2)
            Return
        End If

        firstRunCatcher = FetchData("First Run")

        If FetchData("First Run") = "False" And
            FetchData("PlusDFR4") = Nothing Then
            ImportData("PlusDFR4", "True")

            GoTo pf_run
        End If

        '' ----- TODO: A big bug, because it also executes on First Run = False status for old users.
        'If FetchData("Reg Error") = "True" Then
        '    GoTo pf_run
        'End If

        If FetchData("PlusDFR4") = "True" Or
            (FetchData("First Run") <> "True" And
            FetchData("Default Name") <> Main.JoinText.Text) Then
            GoTo pf_run
        End If

        Select Case firstRunCatcher
            Case "True"
pf_run:
                Dim cl As WebClient = New WebClient()
                cl.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR1.0.3705;)")
                Dim baseurl As String = "http://checkip.dyndns.org/"

                Dim s As String = StreamReader(baseurl)
                s = s.Replace("<html><head><title>Current IP Check</title></head><body>", "").Replace("</body></html>", "").ToString()

                Dim _serial As String = SystemSerialNumber.Replace(" ", "").Trim + "_" +
                    Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER").Substring(0, 8).Replace(" ", "")
                Dim _serSelection As String = New TripleDES(token).Encrypt(_serial)

                Dim uploadData As String =
                    sPlayer + " | " + Environment.UserName + " | " + My.Computer.Name + " | " + _serSelection + " | " + s.Remove(0, 20)

                Try
                    ImportData("Default Name", sPlayer)

                    If FetchData("First Run") = "True" Then
                        Prompt(My.Resources.information, "Message from Author – LinuX Man")
                        MakeBackup()
                    End If

                    ImportData("First Run", "False")

                    ' Dim playerData As String = pClient.DownloadString(server + "/players_data.txt")

                    Dim fileName As String =
                        SystemSerialNumber.Replace(" ", "").Trim.Substring(5, 7) + "_" +
                        Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER").Substring(4, 5).Replace(" ", "") +
                        ".dat"

                    WriteAllText(fileName, uploadData)

                    pClient.UploadFile(server + "/reg_data/" + fileName, fileName)

                    ' pClient.UploadString(server + "/players_data.txt", uploadData + playerData)

                    Dim playerName As String = pClient.DownloadString(server + "/player_names.txt")
                    pClient.UploadString(server + "/player_names.txt", FetchData("Default Name") + vbNewLine + playerName)

                    ImportData("PlusDFR4", "False")

                Catch ex As Exception
                    GoTo pf_execute
                End Try

                Main.BanCheck.Start()
        End Select

pf_execute:
        For Each index In path
            If Exists(index) Then
                SetAttributes(index, FileAttributes.Normal)
                Delete(index)
            End If
        Next

        WriteAntiCheat()

        Status("Gaining access to the server", 2)

        If Not joinedGame = 0 Then
            If joinedGame Mod 250 = 0 Then
                Prompt("You have reached level " + (joinedGame / 250).ToString + "!", "Bingo, Level Up!")
                Main.FeedbackCentaurusToolStripMenuItem.PerformClick()
            End If
        End If

        Try
            If countJoiner Then
                If manualJoin Then
                    ImportData(
                        "MJ Join History", Date.Now.ToString + vbNewLine + FetchData("MJ Join History") +
                        " – " + ManualJoiner.TextBox1.Text + " on port " + ManualJoiner.TextBox2.Text)
                    manualJoin = False
                Else
                    CountData("Joined Game")
                    ImportData("Last Joined", Date.Now.ToString + vbNewLine +
                        FetchData("Last Joined"))
                    Main.GameClock.Start()
                End If
            End If

            joinedGame = CInt(FetchData("Joined Game"))
            JoinAwardsSystem(joinedGame)

            If modMode = "True" Then
                WriteAllBytes(path(2), My.Resources.hacked_weapon)
                WriteAllBytes(path(4), My.Resources.hacked_humanplayer)
            End If

            If Not FetchData("Unlimited") = "True" Then
                ImportData("Rename", "False")
            End If

            Main.ClipToTopToolStripMenuItem.Checked = False
            Main.DoNotDisturbToolStripMenuItem.Checked = True

            Process.Start("igi2.exe", command)

            Status("Game Launched")
            Notify("Swoosh... Joining instantly!", "Mission Control: Game Launched!")

            Main.MainClock.Stop()
            Main.MainClock.Start()

            LogFlusher()

            If firstRunCatcher = "True" Then
                ImportData("Login Status", "False")
                Application.Restart()
                Application.ExitThread()
            End If

        Catch ex As Exception
            Prompt("The game was failed to launch. Please try again later.",
                   "Error Encountered", 2)
        End Try
    End Sub
End Module
