Option Strict On
Option Explicit On

Imports System.Convert

Public Class Loader
    Private Sub Loader_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim rand As Random = New Random
        Dim count As Byte = CByte(rand.Next(0, My.Resources.didYouKnow.Split(CChar(vbNewLine)).Length - 1))
        Dim facts As String() = My.Resources.didYouKnow.Split(CChar(vbLf))

        Label3.Text = facts(count)

        TopMost = False
    End Sub

    Private Sub Loader_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        e.Cancel = e.CloseReason = CloseReason.UserClosing
    End Sub
End Class