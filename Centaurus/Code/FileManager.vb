Option Strict On
Option Explicit On

Imports System.IO
Imports System.IO.File
Friend Module FileManager
    Private missingFiles As String = ""

    ''' <summary>
    ''' Basic anti-cheat files to be placed (since beginning).
    ''' </summary>
    Public Sub WriteAntiCheat()
        Try
            WriteAllBytes(path(0), My.Resources.igi2)
            WriteAllBytes(path(1), My.Resources.ammo)
            WriteAllBytes(path(2), My.Resources.weapon)
            WriteAllBytes(path(3), My.Resources.weapons)
            WriteAllBytes(path(4), My.Resources.humanplayer)

        Catch ex As UnauthorizedAccessException
            Prompt("Unable to write for backup. Access denied.", "File Write Failure", 3)

            Application.Restart()
            Application.ExitThread()
        Catch ex1 As Exception
            '
        End Try
    End Sub

    ''' <summary>
    ''' Takes backup of the old files.
    ''' </summary>
    Public Sub MakeBackup()
        Try
            Directory.CreateDirectory(mainPath + "\WEAPONS\Backup")
            GetMissingFiles(path)

            For index = 0 To backupLocation.Length - 1
                If Exists(backupLocation(index)) Then
                    Delete(backupLocation(index))
                End If

                Copy(path(index), backupLocation(index)) '
            Next

            'For Index = 0 To path.Length - 1
            '    total += "• " + path(Index) + vbCrLf
            'Next

            'Prompt("The following files have been successfully backed up:" +
            '       vbCrLf + vbCrLf + total, "Backup Success")

            'total = ""

        Catch ex As UnauthorizedAccessException
            Prompt("Unable to delete and copy the files to backup.", "Unauthorized Access", 2)
        Catch ex1 As Exception
            Prompt(ex1.Message, "Error Encountered", 2)
        End Try
    End Sub

    ''' <summary>
    ''' Identifies if a file exists.
    ''' </summary>
    ''' <param name="chunk">Location of file(s)</param>
    Private Sub GetMissingFiles(chunk() As String)
        For index = 0 To chunk.Length - 1
            If Not Exists(chunk(index)) Then
                missingFiles += "• " + chunk(index) + vbCrLf
            End If
        Next

        If missingFiles.Length > 0 Then
            Prompt("The following files appears to be missing:" +
                   vbCrLf + vbCrLf + missingFiles, "File(s) Missing", 2)
        End If
    End Sub
End Module
