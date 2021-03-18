Option Strict On
Option Explicit On

Public Class PassChanger
    Private Sub PassChanger_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label2.Text = FetchData("Failed Attempt")
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        CentoProtector.ShowDialog()
        ImportData("Failed Attempt", "")
        Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ImportData("Failed Attempt", "")
        Close()
    End Sub
End Class