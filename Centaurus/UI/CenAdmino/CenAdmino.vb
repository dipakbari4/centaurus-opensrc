Option Strict On
Option Explicit On

Imports System.Net
Imports System.Text.Encoding

Public Class CenAdmino
    Private receivedBytes As String = ""
    Private ReadOnly IP As IPAddress = IPAddress.Parse(ServerIP)

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        BanPlayer.ShowDialog()
    End Sub

    Private Sub LinkLabel4_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel4.LinkClicked
        asker =
            MessageBox.Show(
                "Are you sure to swipe the chat history?" +
                " It is only recommended when some trash players makes it trash.",
                "Sure to Clean Chat?",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If asker = DialogResult.No Then
            Return
        End If

        Dim message As String = chatMessage
        Dim pattern As String = "----"
        Dim ex As New Text.RegularExpressions.Regex(pattern)
        Dim m As Text.RegularExpressions.MatchCollection
        m = ex.Matches(message)

        Dim left = 50 - m.Count

        For index = 1 To left
            SendMessage("Clearing...")
        Next

        Prompt("The ChatUp has been cleared successfully.", "ChatUp Cleared")
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Try
            PendingHelp.ShowDialog()

        Catch ex As Exception
            Prompt("The page has been disposed to save memory.", "Memory Saved")
        End Try
    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        Dim client2 As New Sockets.UdpClient(2222)

        With client2
            .Client.ReceiveTimeout = 1500
            .Connect(ServerIP, CInt(ServerPort))
        End With

        Dim getBytes() As Byte = ASCII.GetBytes("\players\")
        client2.Send(getBytes, getBytes.Length)

        Dim endPoint As IPEndPoint = New IPEndPoint(IPAddress.Any, 0)
        Dim received() As Byte = client2.Receive(endPoint)
        receivedBytes = ASCII.GetString(received)

        client2.Close()

        GPrompt(receivedBytes, "Players Data")
    End Sub

    Private Sub LinkLabel3_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel3.LinkClicked
        If Not IO.File.Exists(mainPath + "\Console.exe") Then
            IO.File.WriteAllBytes(mainPath + "\Console.exe", My.Resources.Console)
        End If

        Process.Start($"{mainPath}\Console.exe", ServerIP + " " + ServerPort)
    End Sub
End Class