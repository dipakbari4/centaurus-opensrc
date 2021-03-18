Option Strict On
Option Explicit On

Imports System.IO.Compression

Public Class ScoreBank
    ' -------------------- EXPORT DATA --------------------
    Private headData As String = ""
    Private playerName As String = ""
    Private achLog As String = ""
    Private ReadOnly saveLoc As String = mainPath + "\centaurus-saves\"
    Private ReadOnly bodyData(3) As String
    Private ReadOnly mac As String = Environment.UserName + Environment.MachineName

    ' -------------------- IMPORT DATA --------------------
    Private ReadOnly RbodyData(3) As String

    Private Sub ScoreBank_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If FetchData("Ach History") = Nothing Then
            ImportData("Ach History", "Beginning to log history for: " +
                FetchData("Default Name"))
        End If

        playerName = FetchData("Default Name").Replace(" ", "")
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        Button1.Enabled = CheckBox1.Checked
        Button2.Enabled = Button1.Enabled
        Button3.Enabled = Button2.Enabled
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        headData =
            FetchData("Joined Game") + vbCrLf +
            FetchData("Chats") + vbCrLf +
            FetchData("Logins") + vbCrLf +
            FetchData("CDays") + vbCrLf +
            FetchData("CHours") + vbCrLf +
            FetchData("CMinutes") + vbCrLf +
            FetchData("CSeconds")

        bodyData(0) = FetchData("Last Joined")
        bodyData(1) = FetchData("Map History")
        bodyData(2) = FetchData("Last Login")
        bodyData(3) = FetchData("Ach History")

        headData = New TripleDES("head" + mac).Encrypt(headData)
        bodyData(0) = New TripleDES("body0" + mac).Encrypt(bodyData(0))
        bodyData(1) = New TripleDES("body1" + mac).Encrypt(bodyData(1))
        bodyData(2) = New TripleDES("body2" + mac).Encrypt(bodyData(2))

        Try
            IO.Directory.CreateDirectory(saveLoc)
            IO.File.WriteAllText(saveLoc + "\head.ctln", headData)
            IO.File.WriteAllText(saveLoc + "\body1.ctln", bodyData(0))
            IO.File.WriteAllText(saveLoc + "\body2.ctln", bodyData(1))
            IO.File.WriteAllText(saveLoc + "\body3.ctln", bodyData(2))

            For Each letter In deniedKeys
                playerName = Replace(playerName, letter, "")
            Next

            ZipFile.CreateFromDirectory(saveLoc, playerName + "-centaurus-data.zip")

            'X Joins, Y Chats and Z Logins exported at -Local format time-.
            achLog = "= " + FetchData("Joined Game") + " Joins, " +
                FetchData("Chats") + " Chats and " + FetchData("Logins") +
                " Logins exported at " + Date.Now.ToString + "." + vbNewLine

            ImportData("Ach History", achLog + FetchData("Ach History"))

            IO.Directory.Delete(saveLoc, True)

        Catch ex As Exception
            If IO.File.Exists(playerName + "-centaurus-data.zip") Then
                Prompt("Please remove the existed saved file before creating this one.",
                       "File Already Exists", 2)
            End If

            If IO.Directory.Exists(saveLoc) Then
                IO.Directory.Delete(saveLoc, True)
            End If
            Return
        Finally
            Prompt("The data of:" + vbCrLf + achLog + vbCrLf +
                   " ••• has been successfully exported in '" +
                   playerName + "-centaurus-data.zip' file. " +
               "Click OK to launch the save folder.", "Export Success")

            Process.Start("explorer", Application.StartupPath)
            Close()
        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If OpenSave.ShowDialog() = DialogResult.OK Then
            Dim path As String = IO.Path.GetFileNameWithoutExtension(OpenSave.FileName)

            Try
                ZipFile.ExtractToDirectory(OpenSave.FileName, path)

                Dim str As String = New TripleDES("head" + mac).
                Decrypt(IO.File.ReadAllText(path + "\head.ctln"))
                Dim lines() As String = str.Split(CChar(vbCrLf))

                RbodyData(0) = New TripleDES("body0" + mac).Decrypt(IO.File.ReadAllText(path + "\body1.ctln"))
                RbodyData(1) = New TripleDES("body1" + mac).Decrypt(IO.File.ReadAllText(path + "\body2.ctln"))
                RbodyData(2) = New TripleDES("body2" + mac).Decrypt(IO.File.ReadAllText(path + "\body3.ctln"))

                ImportData("Joined Game", lines(0).Replace(vbCr, "").Replace(vbLf, ""))
                ImportData("Chats", lines(1).Replace(vbCrLf, "").Replace(vbLf, ""))
                ImportData("Logins", lines(2).Replace(vbCrLf, "").Replace(vbLf, ""))
                ImportData("CDays", lines(3).Replace(vbCrLf, "").Replace(vbLf, ""))
                ImportData("CHours", lines(4).Replace(vbCrLf, "").Replace(vbLf, ""))
                ImportData("CMinutes", lines(5).Replace(vbCrLf, "").Replace(vbLf, ""))
                ImportData("CSeconds", lines(6).Replace(vbCrLf, "").Replace(vbLf, ""))

                ImportData("Last Joined", RbodyData(0))
                ImportData("Map History", RbodyData(1))
                ImportData("Last Login", RbodyData(2))

                IO.Directory.Delete(path, True)

            Catch ex As Exception
                IO.Directory.Delete(path, True)

                Prompt("The restore point was unable to perform the action." + vbNewLine +
                       "Reasons may be:" + vbNewLine + vbNewLine +
                       "  1. The restore point was invalid." + vbNewLine +
                       "  2. Corrupted/damaged file." + vbNewLine +
                       "  3. The extraction already exists." + vbNewLine +
                       "  4. The destination has no writing permission.", "Restore Failed", 3)
                Return
            End Try

            'X Joins, Y Chats and Z Logins imported at -Local format time-.
            achLog = "= " + FetchData("Joined Game") + " Joins, " +
                FetchData("Chats") + " Chats and " + FetchData("Logins") +
                " Logins imported at " + Date.Now.ToString + "." + vbNewLine

            ImportData("Ach History", achLog + FetchData("Ach History"))

            Prompt("Your data has been successfully restored.", "Welcome Back!")
        End If
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        GPrompt(FetchData("Ach History"), "Achievements' Import/Export History")
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        ResetProgress.ShowDialog()
    End Sub
End Class