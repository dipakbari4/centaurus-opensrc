Option Strict On
Option Explicit On

Public Class Login
    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        TextBox1.UseSystemPasswordChar = Not CheckBox1.Checked
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        CheckBox1.Enabled = TextBox1.Text.Length > 0
        If TextBox1.Text = FetchData("Pass Key") Then
            ImportData("Last Login", Date.Now.ToString + vbNewLine + FetchData("Last Login"))
            LoginAwardsSystem(CInt(CountData("Logins")))
            ImportData("Login Status", "True")
            Close()
        End If
    End Sub

    Private Sub Login_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label2.Text = FetchData("Pass Hint")
    End Sub

    Private Sub Login_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If FetchData("Login Status") = "True" Then
        Else
            ImportData("Failed Attempt", Date.Now.ToString)
            End
        End If
    End Sub
End Class