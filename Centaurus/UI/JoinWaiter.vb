Option Strict On
Option Explicit On

Public Class JoinWaiter
    Private Sub JoinWaiter_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label1.Text = My.Resources.waitRecommendation
        Button2.Enabled = Main.JoinButton.Enabled
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        trigger = True
        Main.Hide()
        ShowInTaskbar = False
        Notify("Alright Do your works and I will " +
               "instantly inform you when one joins the game.",
               "Waiting mode is ON")
        Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Main.JoinButton.PerformClick()
        Close()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Close()
    End Sub
End Class