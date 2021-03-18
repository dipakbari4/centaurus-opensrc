Option Strict On
Option Explicit On

Public Class MetroDialog
    Private Sub RichTextBox1_LinkClicked(sender As Object, e As LinkClickedEventArgs) Handles RichTextBox1.LinkClicked
        Process.Start(e.LinkText)
    End Sub

    Private Sub MetroDialog_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        RichTextBox1.Text = ""
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Close()
    End Sub
End Class