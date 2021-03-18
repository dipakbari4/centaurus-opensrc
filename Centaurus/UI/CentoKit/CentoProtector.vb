Option Strict On
Option Explicit On

Public Class CentoProtector
    Private getBool As Boolean

    Private Sub CentoProtector_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Button2.Visible = FetchData("First Run") = "True"
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        TextBox2.UseSystemPasswordChar = Not CheckBox1.Checked
        TextBox1.UseSystemPasswordChar = Not CheckBox1.Checked
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged, TextBox2.TextChanged, TextBox3.TextChanged
        getBool = TextBox1.Text.Length > 5 And TextBox2.Text =
            TextBox1.Text And TextBox3.Text.Length > 0

        Button1.Enabled = getBool
        CheckBox1.Enabled = TextBox1.Text.Length > 0 Or TextBox2.Text.Length > 0
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ImportData("Pass Key", TextBox1.Text)
        ImportData("Pass Hint", TextBox3.Text)
        ImportData("Login Status", "False")

        Prompt("You are all set. The password has been successfully applied.", "Password Applied")

        If FetchData("First Run") = "True" Then
            Close()
        Else
            Application.Restart()
            Application.ExitThread()
        End If
    End Sub

    Private Sub TextBox3_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox3.KeyDown
        If e.KeyCode = Keys.Enter Then
            Button1.PerformClick()
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Close()
    End Sub
End Class