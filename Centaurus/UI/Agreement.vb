Option Strict On
Option Explicit On

Public Class Agreement
    Private regValue As String

    Private Sub Agreement_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Main.BanCheck.Stop()
        Label1.Text = My.Resources.agreement
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        regValue = "The agreement was ACCEPTED by [" + Environment.UserName +
            "] from [" + My.Computer.Name + "] at [" +
            Format(Date.Now, "Short Time") + "] on " + "[" +
            Format(Date.Now, "Short Date") + "]"

        Try
            ImportData("Agreement Status", regValue)
            ImportData("Status3P0", "True")

            If FetchData("First Run") = Nothing Then
                ImportData("First Run", "True")
            End If

            If FetchData("First Run") = "False" Then
                Prompt("Once again, let's make I.G.I.-2 great again!", "All Set and Ready!")
                Close()
                Return
            End If

            ImportData("Announce", "")
            ImportData("Joined Game", "0")
            ImportData("Default Name", "")
            ImportData("Logins", "0")
            ImportData("Pass Key", "")

            Prompt("Now let's make I.G.I.-2 great again!", "Centaurus Ready!")

            Hide()

            Prompt(My.Resources.security, "Security Recommended")
            CentoProtector.ShowDialog()
            Close()

        Catch ex As Exception
            Prompt("Unable to write Agreement details.", "Error Encountered", 2)
            End
        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        regValue = "The agreement was DECLINED by [" + Environment.UserName +
            "] from [" + My.Computer.Name + "] at [" +
            Format(Date.Now, "Short Time") + "] on " + "[" +
            Format(Date.Now, "Short Date") + "]"

        Try
            ImportData("Agreement Status", regValue)
            ImportData("Status3P0", "False")

            Prompt("Sorry, you cannot use the Centaurus unless you" +
                   " agree the Code of Conduct.", "COC Declined", 3)

        Catch ex As Exception
            Prompt("Unable to write Agreement details.", "Error Encountered", 2)
        Finally
            End
        End Try
    End Sub

    Private Sub Agreement_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If Not FetchData("Status3P0") = Nothing Then
            If FetchData("Status3P0") = "False" Then
                End
            End If
        Else
            End
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged_1(sender As Object, e As EventArgs) _
        Handles CheckBox1.CheckedChanged, CheckBox2.CheckedChanged, CheckBox3.CheckedChanged,
        CheckBox4.CheckedChanged, CheckBox5.CheckedChanged, CheckBox6.CheckedChanged

        If CheckBox1.Checked And CheckBox2.Checked And CheckBox3.Checked And CheckBox4.Checked _
            And CheckBox5.Checked And CheckBox6.Checked Then
            Button1.Enabled = True
            Button2.Enabled = False
        Else
            Button1.Enabled = False
            Button2.Enabled = True
        End If
    End Sub
End Class