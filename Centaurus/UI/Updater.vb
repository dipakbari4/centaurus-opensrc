Option Strict On
Option Explicit On

Imports System.IO.Compression
Imports System.IO.File
Imports System.Net

Public Class Updater
    Private WithEvents CWC As New WebClient
    Private ReadOnly archive As String = "Centaurus.zip"
    Private ReadOnly UpdateLink As String = nCTLN + archive
    Private ReadOnly downloadLocation As String = tempPath + archive

    Private Sub Updater_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim size As Long = 0L
        Dim r As WebRequest = WebRequest.Create(UpdateLink)
        r.Method = WebRequestMethods.Http.Head

        Label1.Text = latestVersion
        Label5.Text = Application.ProductVersion
        Label6.Text = FetchData("Last Update")
        RichTextBox1.Text = whatsNew

        Using rsp = r.GetResponse()
            size = rsp.ContentLength
        End Using

        Label10.Text = (size / (1024 ^ 2)).ToString("0.00") + " MB"
    End Sub

    Private Sub Updater_FormClose(sender As Object, e As EventArgs) Handles MyBase.FormClosing
        End
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Button1.Enabled = False

        If IO.Directory.Exists(mainPath + "\Updated") Then
            IO.Directory.Delete(mainPath + "\Updated", True)
        End If

        Try
            CWC.DownloadFileAsync(New Uri(UpdateLink), downloadLocation)
            AddHandler CWC.DownloadFileCompleted, AddressOf DownloadedUpdatePatch

        Catch ex As Exception
            Prompt("Unable to download the update.", "Update Failed", 2)
            CWC.CancelAsync()
        End Try
    End Sub

    Private Sub DownloadUpdatePatch(sender As Object, e As DownloadProgressChangedEventArgs) _
        Handles CWC.DownloadProgressChanged
        ' During download
        ProgressBar1.Value = e.ProgressPercentage

        If e.ProgressPercentage > 50 Then
            Label5.ForeColor = Color.Silver
            Label1.ForeColor = Color.ForestGreen
        End If

        Label4.Text = Format(e.BytesReceived / 1024D / 1024D, "0.00") + " MB / " +
            Format(e.TotalBytesToReceive / 1024D / 1024D, "0.00") + " MB" + " (" +
            e.ProgressPercentage.ToString + "%)"
    End Sub

    ''' <summary>
    ''' Ability to download and extract patch to a given location.
    ''' </summary>
    ''' <param name="sender">Sender object</param>
    ''' <param name="e">Arguments</param>
    Private Sub DownloadedUpdatePatch(sender As Object, e As ComponentModel.AsyncCompletedEventArgs)
        ' After completion
        Try
            Label4.Text = "Downloaded / Total (%)"

            ZipFile.ExtractToDirectory(downloadLocation, mainPath + "\Updated\")

            Notify("The update was successfully downloaded. Please replace it " +
                   "with the current Centaurus.", "Update Downloaded")

            If Exists(downloadLocation) Then
                Delete(downloadLocation)
            End If

            Process.Start("explorer", mainPath + "\Updated\")
            ImportData("Last Update", Format(Date.Now, "Short Date"))
            End

        Catch ex As Exception
        End Try
    End Sub

    Private Sub RichTextBox1_LinkClicked(sender As Object, e As LinkClickedEventArgs) Handles RichTextBox1.LinkClicked
        Process.Start(e.LinkText)
    End Sub
End Class