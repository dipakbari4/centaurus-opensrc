Option Strict On
Option Explicit On

Public Class BannedNotice
    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click
        Clipboard.SetText(Label3.Text)
        Prompt("The email address has been copied!", "Copy Success")
    End Sub
End Class