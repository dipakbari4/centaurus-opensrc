Option Strict On
Option Explicit On

Friend Module UpdateService
    ''' <summary>
    ''' Checks for any application updates and informs when available.
    ''' </summary>
    Public Sub CheckForUpdates()
        Load("Looking for app updates (10 of 10)")

        Select Case latestVersion
            Case "ERROR"
                Prompt("Unable to sync to the update server. Please try back later.",
                       "Update Check Failed", 2)
                End
            Case "0.0.0.0"
                Prompt(
                    "Centaurus systems has shut down permanently." +
                    " Thank you for using the service.",
                    "Service Support Over")
                End
        End Select

        If latestVersion <> Application.ProductVersion Then
            Updater.ShowDialog()
        End If
    End Sub
End Module
