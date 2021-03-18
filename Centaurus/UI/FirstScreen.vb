Option Strict On
Option Explicit On

Public Class FirstScreen
    Private ReadOnly splashQuotes() As String = {
        "Let's Make I.G.I.-2 Great Again!",
        "Together we stand, together we conquer!",
        "It's time to homecoming!",
        "Let's Make I.G.I.-2 Alive!",
        "Today Fallen, Tomorrow Risen!",
        "You Have a Plan, You Have an Enemy" + vbNewLine + "Execute Them Both!",
        "Ready Player? Vroom, Vroom... Let's Go!",
        "Sunrise is on the way!",
        """Centaurus"" name was chosen from a star constellation!",
        "The Ghosts are Real and They are Coming"}

    Private Sub Splash_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Static rand As Random = New Random
        Dim count As Byte = CByte(rand.Next(0, splashQuotes.Length - 1))
        Dim str As String

        Label5.Text = My.Application.Info.Copyright

        str = "Version " + My.Application.Info.Version.Major.ToString +
            "." + My.Application.Info.Version.Minor.ToString

        If My.Application.Info.Version.Build > 0 Then
            str += "." + My.Application.Info.Version.Build.ToString
        End If

        Label6.Text = splashQuotes(count)
        Label4.Text = str
    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click
        '
    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click
        Process.Start(WebLinks(0))
    End Sub

    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click
        Process.Start("https://www.google.com/search?q=26th+january+in+india")
    End Sub

    Private Sub Popup_Tick(sender As Object, e As EventArgs) Handles Popup.Tick
        Popup.Stop()
        Close()
    End Sub
End Class