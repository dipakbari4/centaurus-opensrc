Option Strict On
Option Explicit On

Imports System.IO
Imports System.Net

Public Class PendingHelp
    Private ReadOnly ftpUrl As String = server + "/Issues/"
    Private report As String = Nothing

    Private Sub PendingHelp_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Dim request As FtpWebRequest = DirectCast(WebRequest.Create(ftpUrl), FtpWebRequest)
            Dim dirList As New List(Of String)

            request.Method = WebRequestMethods.Ftp.ListDirectory
            request.Credentials = New NetworkCredential(us0, us1)

            Dim response As FtpWebResponse = DirectCast(request.GetResponse(), FtpWebResponse)
            Dim responseStream As Stream = response.GetResponseStream

            Using reader As New StreamReader(responseStream)
                Do While reader.Peek <> -1
                    dirList.Add(reader.ReadLine)
                Loop
            End Using

            ListBox1.Items.AddRange(dirList.ToArray)

        Catch ex As Exception
            Prompt(ex.Message, "Unable to Open", 2)
            Dispose() ' CHANGEX
        End Try
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        TextBox1.ReadOnly = ListBox1.SelectedIndex = -1

        If ListBox1.SelectedIndex = -1 Then
            Return
        End If

        Try
            TextBox1.Text = New TripleDES("issue").Decrypt(
                pClient.DownloadString(ftpUrl + ListBox1.SelectedItem.ToString))
            report = TextBox1.Text

            Button1.Enabled = ListBox1.SelectedIndex <> -1 And TextBox1.Text.Length >= 64 And TextBox1.Text <> report

        Catch ex As Exception
            '
        End Try
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Dim confirm As DialogResult =
                MessageBox.Show(Me,
                "Confirm to send this reply?",
                "Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question)

            If Not confirm = DialogResult.Yes Then
                Return
            End If

            pClient.UploadString(
                ftpUrl + ListBox1.SelectedItem.ToString,
                New TripleDES("issue").Encrypt(TextBox1.Text))
            Prompt("The respond has been sent successfully.", "Response Sent to the Message Author")

        Catch ex As Exception
            Prompt(ex.Message, "Respond Failure", 2)
        End Try
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        Label4.ForeColor = If(Button1.Enabled, Color.LightGreen, Color.Coral)

        Label4.Text = If(Button1.Enabled, "Message eligible to publish!", (64 -
            TextBox1.Text.Length).ToString + " more letters to go...")

        Button1.Enabled = TextBox1.Text <> report And TextBox1.Text.Length >= 64
    End Sub

    Private Sub PendingHelp_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Dispose()
    End Sub
End Class