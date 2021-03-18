Option Strict On
Option Explicit On

Public Class ContactAdmin
    Private issueName As String = ""
    Private address As String = ""

    Private Sub ContactAdmin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        issueName = FetchData("Issue Opened").Replace(" ", "")
        address = server + "/Issues/" + issueName + ".txt"
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        Button1.Enabled = TextBox1.Text.Length >= 200
        Label4.ForeColor = If(Button1.Enabled, Color.Green, Color.Coral)
        Label4.Text = If(Button1.Enabled, "Message eligible to publish!", (200 -
            TextBox1.Text.Length).ToString + " more letters to go...")
    End Sub

    Private Sub ContactAdmin_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Dispose()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim confirm As DialogResult =
            MessageBox.Show(
            Me,
            "Have you reviewed everything you have written above? If yes, then you are" +
            " ready to send this so the admins will reply to you back; otherwise, you are" +
            " recommended to review your message.", "Confirmation Required", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        Select Case confirm
            Case DialogResult.Yes
                ' Go On
            Case Else
                Return
        End Select

        IO.File.WriteAllText(mainPath + issueName, New TripleDES("issue").Encrypt(TextBox1.Text))
        ImportData("Report", TextBox1.Text)

        Try
            pClient.UploadFile(address, mainPath + issueName)
            IO.File.Delete(mainPath + issueName)
            ImportData("Sent", "True")

            Main.ContactAnAdminToolStripMenuItem.Visible = False
            Main.ToolStripSeparator10.Visible = False

        Catch ex As Exception
            IO.File.Delete(mainPath + issueName)
            Prompt("Cannot process the upload stream.", "Upload Failed", 2)

            Return
        End Try

        Prompt("You are all done! Now the Centaurus will automatically show you the response" +
               " sent by an admin.", "Issue Reported")
        Close()
    End Sub

    Private Sub LinkLabel4_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel4.LinkClicked
        GPrompt(My.Resources.exampleIssue, "An Issue Example")
    End Sub
End Class