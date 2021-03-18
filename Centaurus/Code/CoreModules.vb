Option Strict On
Option Explicit On

Friend Module CoreModules
    Private msgStyle As MsgBoxStyle
    Private color As Color
    Private notifier As ToolTipIcon = Main.Notifier.BalloonTipIcon
    Private counter As Byte = 0

    ''' <summary>
    ''' Creates a message box and prompts the text.
    ''' </summary>
    ''' <param name="message">Required message</param>
    ''' <param name="heading">Heading of the prompt</param>
    ''' <param name="type">Type of the box</param>
    Public Sub Prompt(message As String, heading As String, Optional type As Byte = 1)
        Select Case type
            Case 1
                msgStyle = MsgBoxStyle.Information
            Case 2
                msgStyle = MsgBoxStyle.Exclamation
            Case 3
                msgStyle = MsgBoxStyle.Critical
        End Select

        MsgBox(message, msgStyle, heading)
    End Sub

    ''' <summary>
    ''' Creates a MetroDialog form (custom dialog).
    ''' </summary>
    ''' <param name="body">Text containing for the dialog</param>
    ''' <param name="head">Header for the dialog</param>
    Public Sub GPrompt(body As String, head As String)
        With MetroDialog
            .Size = New Size(525, 360)
            .Label1.Text = head
            .RichTextBox1.Text = body
            .ShowDialog()
        End With
    End Sub

    ''' <summary>
    ''' Sets the Status Text message located at bottom of the app.
    ''' </summary>
    ''' <param name="statusMessage">Sets the Status Message for StatusLabel</param>
    ''' <param name="statusType">Sets some specific colors for StatusLabel</param>
    Public Sub Status(statusMessage As String, Optional statusType As Short = 1)
        Select Case statusType
            Case 1
                ' Normal - Default
                color = Color.FromArgb(192, 64, 0)
            Case 2
                ' Warning - Orange
                color = Color.Orange
            Case 3
                ' Error - Red
                color = Color.Firebrick
        End Select

        Main.StatusLabel.Text = statusMessage
        Main.StatusBar.BackColor = color
    End Sub

    ''' <summary>
    ''' Notification module of Windows.
    ''' </summary>
    ''' <param name="message">Message for the notification</param>
    ''' <param name="head">Head or title</param>
    ''' <param name="type">Type of the notification (optional)</param>
    Public Sub Notify(message As String, head As String, Optional type As Byte = 1)
        Select Case type
            Case 1
                notifier = ToolTipIcon.Info
            Case 2
                notifier = ToolTipIcon.Warning
            Case 3
                notifier = ToolTipIcon.Error
        End Select

        Main.Notifier.ShowBalloonTip(3000, head, message, notifier)
    End Sub

    ''' <summary>
    ''' Sets the loader text of the specified string.
    ''' </summary>
    ''' <param name="text">Text to be shown on load</param>
    Public Sub Load(text As String)
        Loader.LoaderText.Text = text
        counter += CByte(1)
        Loader.ProgressBar1.Value += 10

        For i As Integer = 0 To 10
            Threading.Thread.Sleep(10)
            Application.DoEvents()
        Next
    End Sub
End Module