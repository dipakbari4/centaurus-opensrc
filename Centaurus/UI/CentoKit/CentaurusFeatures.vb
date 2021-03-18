Option Strict On
Option Explicit On

Public Class CentaurusFeatures
    Private Sub CentaurusFeatures_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        About.Closer.Start()
    End Sub

    Private Sub LinkLabel4_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel4.LinkClicked
        TopMost = False
        Main.FeedbackCentaurusToolStripMenuItem.PerformClick()
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Close()
    End Sub
End Class