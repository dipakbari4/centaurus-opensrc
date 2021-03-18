Option Strict On
Option Explicit On

Public Class ManualJoiner
    Private ReadOnly keys() As String = {"1234567890", "1234567890."}

    Private Sub ManualJoiner_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If FetchData("MJ Join History") = Nothing Then
            ImportData("MJ Join History", "")
        End If
    End Sub

    Private Sub TextBox_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged, TextBox2.TextChanged, TextBox3.TextChanged
        If TextBox1.Text.Length > 8 And TextBox2.Text.Length >= 3 Then
            CheckBox1.Enabled = True

            If CheckBox1.Checked Then
                TextBox3.Enabled = True
                CheckBox2.Enabled = True
                Button1.Enabled = TextBox3.Text.Length > 0
            Else
                Button1.Enabled = True
                TextBox3.Enabled = False
            End If
        Else
            CheckBox1.Enabled = False
            TextBox3.Enabled = False
            CheckBox2.Enabled = False
            Button1.Enabled = False
        End If
    End Sub

    Private Sub TextBox2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox2.KeyPress
        DetectKeys(TextBox2, keys(0), e)
    End Sub

    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress
        DetectKeys(TextBox1, keys(1), e)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Dim NewIP As Net.IPAddress = Net.IPAddress.Parse(TextBox1.Text)
        Catch ex As Exception
            Prompt("Incorrect IP address was entered. Please try again.", "Incorrect IP Address", 2)
            Return
        End Try

        manualJoin = True

        Main.JoinButton.Enabled = False
        JoinGame(TextBox1.Text, TextBox2.Text, TextBox3.Text)
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        TextBox3.Enabled = True
        TextBox3.Enabled = CheckBox1.Checked
        Button1.Enabled = Not CheckBox1.Checked OrElse TextBox3.Text.Length > 0
    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        TextBox3.UseSystemPasswordChar = Not CheckBox2.Checked
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        GPrompt(FetchData("MJ Join History"), "Manual Join History")
    End Sub
End Class