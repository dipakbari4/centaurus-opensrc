Option Strict On
Option Explicit On

Friend Module AwardingSystem
    ''' <summary>
    ''' Updates all the achievable JOIN awards.
    ''' </summary>
    ''' <param name="joins">Number of game joins</param>
    Public Sub JoinAwardsSystem(joins As Integer)
        If joins = 0 Then
            Return
        End If

        With Main
            .Stepper10JoinsToolStripMenuItem.Enabled = joins >= 10
            .Intuitive100JoinsToolStripMenuItem.Enabled = joins >= 100
            .Medium250JoinsToolStripMenuItem.Enabled = joins >= 250
            .DayLy500JoinsToolStripMenuItem.Enabled = joins >= 500
            .Established1KJoinsToolStripMenuItem.Enabled = joins >= 1000
            .Intermediate25KJoinsToolStripMenuItem.Enabled = joins >= 2500
            .Ridiculous5KJoinsToolStripMenuItem.Enabled = joins >= 5000
            .Professional10KJoinsToolStripMenuItem.Enabled = joins >= 10000
            .Senior25KJoinsToolStripMenuItem.Enabled = joins >= 25000
            .Legendary75KJoinsToolStripMenuItem.Enabled = joins >= 75000
        End With

        Main.PictureBox1.Visible = Main.Professional10KJoinsToolStripMenuItem.Enabled

        If FetchData("Achievement") = "False" Then
            If joins = 75000 Or joins = 25000 Or joins = 10000 Or joins = 5000 Or
                joins = 2500 Or joins = 1000 Or joins = 500 Or joins = 250 Or
                joins = 100 Or joins = 10 Then
                AchievedCommons()
            End If
        End If

        If joins >= 10000 Then
            ImportData("Unlimited", "True")
        End If
    End Sub

    ''' <summary>
    ''' Updates all achievable CHATUP awards.
    ''' </summary>
    ''' <param name="chats">Number of chats sent</param>
    Public Sub ChatAwardsSystem(chats As Integer)
        With Main
            .Stepper10MessagesToolStripMenuItem.Enabled = chats >= 10
            .ChatBase50MessagesToolStripMenuItem.Enabled = chats >= 50
            .General100MessagesToolStripMenuItem.Enabled = chats >= 100
            .Explainer500MessagesToolStripMenuItem.Enabled = chats >= 500
            .Established1KMessagesToolStripMenuItem.Enabled = chats >= 1000
            .Marshal25KMessagesToolStripMenuItem.Enabled = chats >= 2500
            .Shogun5KMessagesToolStripMenuItem.Enabled = chats >= 5000
            .Professional10KMessagesToolStripMenuItem.Enabled = chats >= 10000
            .Senior25KJoinsToolStripMenuItem.Enabled = chats >= 25000
            .TheWoodmore75KMessagesToolStripMenuItem.Enabled = chats >= 75000
        End With

        Main.PictureBox2.Visible = Main.Professional10KMessagesToolStripMenuItem.Enabled

        If FetchData("Achievement") = "False" Then
            If chats = 75000 Or chats = 25000 Or chats = 10000 Or
                chats = 5000 Or chats = 2500 Or chats = 1000 Or chats = 500 Or
                chats = 100 Or chats = 50 Or chats = 10 Then
                AchievedCommons()
            End If
        End If
    End Sub

    ''' <summary>
    ''' Updates all the achievable LOGIN awards.
    ''' </summary>
    ''' <param name="logins">Number of logins</param>
    Public Sub LoginAwardsSystem(logins As Integer)
        With Main
            .Stepper10LoginsToolStripMenuItem.Enabled = logins >= 10
            .Dailycomer50LoginsToolStripMenuItem.Enabled = logins >= 50
            .Persistent100LoginsToolStripMenuItem.Enabled = logins >= 100
            .Securified500LoginsToolStripMenuItem.Enabled = logins >= 500
            .BankSafe1KLoginsToolStripMenuItem.Enabled = logins >= 1000
        End With

        Main.PictureBox3.Visible = Main.Persistent100LoginsToolStripMenuItem.Enabled

        If FetchData("Achievement") = "False" Then
            If logins = 1000 Or logins = 500 Or logins = 100 Or logins = 50 Or logins = 10 Then
                AchievedCommons()
            End If
        End If
    End Sub

    ''' <summary>
    ''' Manage records about in-game activeness.
    ''' </summary>
    ''' <param name="d">Total days</param>
    ''' <param name="h">Total hours</param>
    ''' <param name="m">Total minutes</param>
    Public Sub ActivenessRecords(d As Integer, h As Integer, m As Integer)
        Dim minutes As Integer = d * 24 + h * 60 + m
        Dim hours As Integer = CInt(d * 24 + (minutes / 60))

        With Main
            .FirstMinute1MinuteToolStripMenuItem.Enabled = minutes >= 1
            .HalfAnHour30MinutesToolStripMenuItem.Enabled = minutes >= 30
            .HourlyanHourToolStripMenuItem.Enabled = hours >= 1
            .ThOfTheDay3HoursToolStripMenuItem.Enabled = hours >= 3
            .ThOfTheDay6HoursToolStripMenuItem.Enabled = hours >= 6
            .TheDayADayToolStripMenuItem.Enabled = d >= 1
            .FirstWeek7DaysToolStripMenuItem.Enabled = d >= 7
            .ActivePlayer30DaysToolStripMenuItem.Enabled = d >= 30
            .Seasonal4MonthsToolStripMenuItem.Enabled = d >= 120
            .TheOutstanderaYearToolStripMenuItem.Enabled = d >= 365
        End With

        Main.PictureBox4.Visible = Main.ActivePlayer30DaysToolStripMenuItem.Enabled
    End Sub

    Private Sub AchievedCommons()
        ImportData("Achievement", "True")

        For index = 1 To 3
            CountData("Joined Game")
            CountData("Logins")
            CountData("Chats")
            CountData("CMinutes")
        Next

        With Main.AchievementsToolStripMenuItem
            .ToolTipText = "New achievement(s) unlocked!"
            .Font = New Font("Roboto", 9.0!, FontStyle.Bold)
            .ForeColor = Color.LightGreen
        End With
    End Sub
End Module
