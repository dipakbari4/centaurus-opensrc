Option Strict On
Option Explicit On

Public Class MyTrack
    Private ReadOnly leftJoins As String = (250 - CInt(FetchData("Joined Game")) Mod 250).ToString
    Private ReadOnly leftJoinsPercentage As Integer = CInt(100 - CInt(leftJoins) * 100 / 250)

    Private Sub MyTrack_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label3.Text = FetchData("Joined Game").ToString
        Label5.Text = FetchData("Chats").ToString
        Label7.Text = FetchData("Logins").ToString

        Label9.Text = Math.Round(CInt(FetchData("Joined Game")) / 250).ToString
        Label11.Text = If(FetchData("Warnings") = "0", "None (click to know)", FetchData("Warnings"))

        Label15.Text = FetchData("CDays") + "d " + FetchData("CHours") +
            "h " + FetchData("CMinutes") + "m " + FetchData("CSeconds") + "s"
        Label18.Text = leftJoinsPercentage.ToString + "% of 250 joins"

        ProgressBar1.Value = leftJoinsPercentage
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ScoreBank.ShowDialog()
    End Sub

    Private Sub Label9_Click(sender As Object, e As EventArgs) Handles Label11.Click
        Prompt("Moderator Warnings tells the number of warnings given to you." +
               " If it exceeds 3 or more, you will be banned.",
               "What is a Moderator Warning?")
    End Sub
End Class