Option Strict On
Option Explicit On

Public Class SetFeatures
    Private Sub SetFeatures_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CheckBox1.Checked = FetchData("MapDown Feature") = "True"
        CheckBox2.Checked = FetchData("ChatUp Feature") = "True"
        CheckBox3.Checked = FetchData("LiveServer Feature") = "True"
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ImportData("MapDown Feature", CheckBox1.Checked.ToString)
        ImportData("ChatUp Feature", CheckBox2.Checked.ToString)
        ImportData("LiveServer Feature", CheckBox3.Checked.ToString)

        Hide()
        Status("Changes applied successfully.")
        Prompt("Please restart Centaurus to take effects.", "Features Edited")
        Close()
    End Sub
End Class