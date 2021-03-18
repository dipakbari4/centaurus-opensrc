Option Strict On
Option Explicit On

Imports System.Net
Imports System.Net.Sockets
Imports System.Text

Friend Module ChatUpActivity
    Private GLOIP As IPAddress
    Private commandBytes As Byte() = New Byte() {}
    Private ReadOnly udpClientR As New UdpClient

    ''' <summary>
    ''' Attempts to send a message to the server.
    ''' </summary>
    ''' <param name="CTRL">Control name used for sending message</param>
    Public Sub SendMessage(CTRL As Object)
        If String.IsNullOrWhiteSpace(CTRL.ToString) Then
        Else
            GLOIP = IPAddress.Parse(New TripleDES(token).Decrypt(StreamReader(chatHostIPS)))
            udpClientR.Connect(GLOIP, 26001)

            commandBytes = Encoding.ASCII.GetBytes(CTRL.ToString)
            udpClientR.Send(commandBytes, commandBytes.Length)
        End If
    End Sub
End Module
