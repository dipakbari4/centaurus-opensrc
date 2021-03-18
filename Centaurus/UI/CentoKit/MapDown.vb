Option Strict On
Option Explicit On

Public Class MapDown
    Private Sub MapDown_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FetchMaps()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Not MapBox.Items.Contains(MapBox.Text) Then
            Prompt("No such map found. Please double-check the name.",
                       "Incorrect Map Chosen", 2)
            Return
        End If

        MapBox.Enabled = False
        MapDownloader(MapBox.SelectedItem.ToString + ".zip")
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start("http://shooter.unaux.com")
    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        Process.Start("https://i-g-i-2.weebly.com/download-igi-2-map.html")
    End Sub

    Private Sub LinkLabel3_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel3.LinkClicked
        Process.Start("http://igi2multiplayermapsfree.weebly.com/downloads.html")
    End Sub

    Private Sub LinkLabel4_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel4.LinkClicked
        CancelDownload()
    End Sub

    Private Sub LinkLabel5_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel5.LinkClicked
        GPrompt(FetchData("Map History"), "Map History")
    End Sub

    Private Sub LinkLabel6_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel6.LinkClicked
        If IO.Directory.Exists(mainPath + "\MISSIONS\multiplayer\") Then
            Process.Start("explorer", mainPath + "\MISSIONS\multiplayer\")
        End If
    End Sub

    Private Sub TLap_Tick(sender As Object, e As EventArgs) Handles TLap.Tick
        CManip += 1
    End Sub
End Class