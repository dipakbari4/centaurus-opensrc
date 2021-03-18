Option Strict On
Option Explicit On

Public Class ChatUp
    Private ReadOnly userName As String = FetchData("Default Name")
    Private starter As Short = 0
    Private ender As Short = 0

    ''' <summary>
    ''' Updates the chat in an interval.
    ''' </summary>
    Public Sub UpdateChat()
        ender = CShort(ChatBox.Text.LastIndexOf(userName))

        With ChatBox
            .SelectionStart = ChatBox.Text.Length
            .ScrollToCaret()
        End With

        Try
            While starter < ender
                ChatBox.Find(userName, starter,
                             ChatBox.TextLength, RichTextBoxFinds.MatchCase)
                ChatBox.SelectionColor = Color.FromArgb(192, 64, 0)

                starter = CShort(ChatBox.Text.IndexOf(userName, starter) + 1)
            End While
        Catch ex As Exception
            ' Suppressed exception
        End Try

        MsgB.Focus()
        starter = CShort(ender = 0)
    End Sub

    Private Sub ChatUp_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        chatMessage = New TripleDES("rtr").Decrypt(StreamReader(chatServer))
        ChatBox.Text = chatMessage
        UpdateChat()
        ImportData("History", chatMessage)
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        GPrompt(My.Resources.chatRules, "ChatUp Rules")
    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        Dim parseTry As Integer = 0

        If Not Integer.TryParse(FetchData("Abusive ChatCount"), parseTry) Then
            ImportData("Abusive ChatCount", "0")
        End If

        If (CInt(FetchData("Abusive ChatCount")) > 5) Then
            Prompt("You do not deserve chatting anymore. Game Ended, Tata!",
                       "Vulgarity Exceeded", 2)
            Return
        End If

        If String.IsNullOrWhiteSpace(MsgB.Text) Or MsgB.Text.Length < 3 Then
            Prompt("A message must contain at least 3 letters.", "Too Short Message", 2)
            Return
        End If

        If Not FilterAbuse(MsgB.ToString) Then
            CountData("Abusive ChatCount")

            Prompt("You cannot use any abusive word in public chat." + vbCrLf + vbCrLf +
                   (5 - CInt(FetchData("Abusive ChatCount"))).ToString + " warnings left.",
                   "Offensive Word Prohibited", 2)
            Return
        End If

        Try
            SendMessage(userName + vbCrLf + "---- " + MsgB.Text + vbCrLf)

            CountData("Chats")
            ChatAwardsSystem(CInt(FetchData("Chats")))

            chatMessage = New TripleDES("YOUR_TOKEN").Decrypt(StreamReader(chatServer))
            ChatBox.Text = chatMessage
            UpdateChat()
            ImportData("History", chatMessage)

            GC.Collect()
            MsgB.Clear()

        Catch ex As Exception
            Prompt("Please ensure your internet is connected properly.", "Message Not Sent", 2)
        End Try
    End Sub

    Private Sub ChatBox_LinkClicked(sender As Object, e As LinkClickedEventArgs) Handles ChatBox.LinkClicked
        Process.Start(e.LinkText)
    End Sub

    Private Sub ChatUp_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        GC.Collect()
        Dispose()
    End Sub

    Private Sub MsgB_KeyDown(sender As Object, e As KeyEventArgs) Handles MsgB.KeyDown
        If e.KeyCode = Keys.Enter Then
            Dim ex As LinkLabelLinkClickedEventArgs = New LinkLabelLinkClickedEventArgs(LinkLabel2.Links(0))
            LinkLabel2_LinkClicked(sender, ex)
        End If
    End Sub

    Private Sub MsgB_KeyPress(sender As Object, e As KeyPressEventArgs) Handles MsgB.KeyPress
        DetectKeys(sender, allowedKeys, e)
    End Sub
End Class