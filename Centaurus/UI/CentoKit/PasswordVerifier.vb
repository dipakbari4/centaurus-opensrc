Option Strict On
Option Explicit On

Public Class PasswordVerifier
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        passwd = TextBox1.Text
        Close()
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        Button1.Enabled = Not String.IsNullOrWhiteSpace(TextBox1.Text)
        CheckBox1.Enabled = TextBox1.Text.Length > 0
    End Sub

    Private Sub TextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyDown
        If e.KeyCode = Keys.Enter Then
            Button1.PerformClick()
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        TextBox1.UseSystemPasswordChar = Not CheckBox1.Checked
    End Sub
End Class