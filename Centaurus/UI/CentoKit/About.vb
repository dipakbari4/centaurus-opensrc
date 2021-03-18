Option Strict On
Option Explicit On

Public Class About
    Private seconds As Byte = 10

    Private Sub About_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label7.Text = FirstScreen.Label4.Text
        seconds -= CByte(1)

        Closer.Start()
    End Sub

    Private Sub Closer_Tick(sender As Object, e As EventArgs) Handles Closer.Tick
        If seconds > 0 Then
            Label6.Text = "PROTIP: It will automatically close in " +
            seconds.ToString + " seconds or click anywhere"
            seconds -= CByte(1)
            Return
        End If

        Closer.Stop()
        Close()
    End Sub

    Private Sub LinkLabel3_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel3.LinkClicked
        Closer.Stop()
        CentaurusFeatures.ShowDialog()
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        GPrompt(My.Resources.license, LinkLabel1.Text)
    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        Prompt("Contact the author & developer of the Centaurus: linuxman.linux@gmail.com", "Contact the Author")
    End Sub

    Private Sub About_Click(sender As Object, e As EventArgs) Handles MyBase.Click
        Close()
        Dispose()
    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click
        Process.Start(WebLinks(0))
    End Sub

    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click
        About_Click(sender, e)
    End Sub

    Private Sub About_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Dispose()
    End Sub
End Class