Option Strict On
Option Explicit On
Imports System.Net

Public Class BanPlayer
    Private ReadOnly warningFile As String = server + "/mod_ban.txt"
    Private ReadOnly playerFile As String = server + "/player_names.txt"
    Private ReadOnly logFile As String = server + "/mod_logs.txt"
    Private text1 As String = ""
    Private text2 As String = ""
    Private text3 As String = ""

    Private Sub BanPlayer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Dim pClient As WebClient = New WebClient With {
                .Credentials = New NetworkCredential(us0, us1)
            }

            text1 = pClient.DownloadString(playerFile)
            text2 = pClient.DownloadString(warningFile)
            text3 = pClient.DownloadString(logFile)

        Catch ex As Exception
            Prompt(ex.Message, "Server Connection Failed")
            Close()
        End Try

        Dim arrayText() As String = text1.Split(CType(Environment.NewLine, Char()))
        Dim arrayText2() As String = text2.Split(CType(Environment.NewLine, Char()))

        ListBox1.Items.AddRange(arrayText)
        ListBox2.Items.AddRange(arrayText2)

        For i = ListBox1.Items.Count - 1 To 0 Step -1
            If String.IsNullOrEmpty(CStr(ListBox1.Items(i))) Then
                ListBox1.Items.RemoveAt(i)
            End If
        Next

        For i = ListBox2.Items.Count - 1 To 0 Step -1
            If String.IsNullOrEmpty(CStr(ListBox2.Items(i))) Then
                ListBox2.Items.RemoveAt(i)
            End If
        Next
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Label5.Visible = True
        Cursor = Cursors.WaitCursor

        Select Case text2
            Case ""
                pClient.UploadString(warningFile, ListBox1.SelectedItem.ToString)
            Case Else
                pClient.UploadString(warningFile, ListBox1.SelectedItem.ToString + vbNewLine + text2)
        End Select

        pClient.UploadString(logFile, "Moderator " + FetchData("Default Name") + " has warned" +
                            " the player " + ListBox1.SelectedItem.ToString + " at " +
                            Date.Now.ToString + ". Reason: " + TextBox1.Text.ToString + vbNewLine + text3)

        ListBox2.Items.Add(ListBox1.SelectedItem)

        Label5.Visible = False
        Cursor = Cursors.Default
    End Sub

    Private Sub BanPlayer_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Dispose()
    End Sub

    Private Sub LinkLabel5_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel5.LinkClicked
        Prompt("Warn the players only when they break the Code of Conduct and proven to be the exact violator.",
               "Information for Moderators")
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Dim pClient As WebClient = New WebClient With {
            .Credentials = New NetworkCredential(us0, us1)
        }
        GPrompt(pClient.DownloadString(server + "/mod_logs.txt"), "Moderation Logs")
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        Button1.Enabled = Not String.IsNullOrWhiteSpace(TextBox1.Text)
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        TextBox1.Enabled = ListBox1.SelectedIndex <> -1
    End Sub
End Class