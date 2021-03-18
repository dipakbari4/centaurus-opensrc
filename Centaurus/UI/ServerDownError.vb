Option Strict On
Option Explicit On

Public Class ServerDownError
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Process.Start("https://forms.gle/YuB63pzLoEGpB7919")
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        GPrompt(My.Resources.syncSolution, "Solutions for Connectivity Errors")
    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        FAQ.ShowDialog()
    End Sub

    Private Sub LinkLabel3_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel3.LinkClicked
        MessageBox.Show(My.Resources.syncError, "Why am I seeing this error?",
            MessageBoxButtons.OK, MessageBoxIcon.Question)
    End Sub
End Class