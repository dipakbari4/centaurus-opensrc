Option Strict On
Option Explicit On

Public Class ResetProgress
    Private Sub ResetProgress_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        GroupBox1.Enabled = CheckBox1.Checked
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim information As String =
            "Still I am recommending not to delete your data and show your " +
            "community how much valuable you are! Please do not come to this region " +
            "ever if you have just lost your confidence. Bad days is seen by everyone." +
            vbCrLf + vbCrLf +
            "Still wanting to proceed? Note that it is irreversible!"

        asker = MessageBox.Show(information, "Final Confirmation",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Warning)

        Select Case asker
            Case DialogResult.No
                Dispose()
                Return
            Case Else
        End Select

        ImportData("Joined Game", "0")
        ImportData("Chats", "0")
        ImportData("Logins", "0")

        ImportData("Join History", "0")
        ImportData("Last Joined", "")
        ImportData("Last Login", "")

        ImportData("CDays", "0")
        ImportData("CHours", "0")
        ImportData("CMinutes", "0")
        ImportData("CSeconds", "0")

        ImportData("Pass Key", "")
        ImportData("Pass Hint", "")
        ImportData("Login Status", "True")

        Prompt("Your data has been reset. Dismiss to restart.", "Reset Finished")

        Application.Restart()
        Application.ExitThread()
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        Dim playerName As String = FetchData("Default Name")
        Button1.Enabled = playerName = TextBox1.Text
    End Sub
End Class