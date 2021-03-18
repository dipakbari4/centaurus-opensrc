Option Strict On
Option Explicit On

Imports System.IO
Imports System.Net.NetworkInformation
Imports System.Threading

Friend Module Networking
    Private streamText As String

    ''' <summary>
    ''' Network verifier to check the existence of the network of the device.
    ''' </summary>
    Public Sub AuthenticateNetwork()
        Dim serverRet As Short = 0

        Try
            If My.Computer.Network.IsAvailable Then
                Dim player As Integer = 0

                Dim netThread As New Thread(
                    New ThreadStart(
                    Sub()
                        player = CInt(GetPlayers(ServerIP, CInt(ServerPort)))
                    End Sub))

                If player >= 0 And Not player.ToString = "" Then
                    serverRet = 1

                    Main.Reconnect.Visible = False

                    If Not Main.JoinButton.Enabled Then
                        If Not FetchData("First Run") = "True" Then
                            Main.JoinButton.Enabled = True
                        End If
                    End If
                Else
                    ' Main.JoinText.ReadOnly = FetchData("Rename") = "True"
                    Main.JoinButton.Enabled = False

                    serverRet = -1
                    Main.Reconnect.Visible = True
                End If
            Else
                ' Not available
                ' Main.JoinText.ReadOnly = True
                Main.JoinButton.Enabled = False

                Main.Reconnect.Visible = True

                Status("System Offline – No Network", 3)
            End If

            Status(
                If(serverRet = -1, "No connection. Retrying shortly...", Main.Text + " Ready"),
                CShort(If(serverRet = -1, 2, 1)))

        Catch ex As Exception
            Main.Reconnect.Visible = True
            Status("Connection is not established", 3)
        End Try
    End Sub

    ''' <summary>
    ''' Reads the Stream of the given URL.
    ''' </summary>
    ''' <param name="URL">URL to read the stream data from</param>
    ''' <returns>String</returns>
    Public Function StreamReader(URL As String) As String
        Try
            Using stream As New StreamReader _
                (Net.WebRequest.Create(URL).
                    GetResponse().
                        GetResponseStream())

                streamText = stream.ReadToEnd()
            End Using

            Return streamText

        Catch ex As Exception
            Return "ERROR"
        End Try
    End Function

    ''' <summary>
    ''' Obtains the MAC address of the user PC.
    ''' </summary>
    ''' <returns>String</returns>
    Public Function GetMacAddress() As String
        Try
            Dim adapters As NetworkInterface() = NetworkInterface.GetAllNetworkInterfaces()
            Dim adapter As NetworkInterface
            Dim myMac As String = String.Empty

            For Each adapter In adapters
                Select Case adapter.NetworkInterfaceType
                    Case NetworkInterfaceType.Tunnel, NetworkInterfaceType.Loopback, NetworkInterfaceType.Ppp
                    Case Else
                        If Not adapter.GetPhysicalAddress.ToString = String.Empty And
                            Not adapter.GetPhysicalAddress.ToString = "00000000000000E0" Then

                            myMac = adapter.GetPhysicalAddress.ToString
                            Exit For
                        End If

                End Select
            Next adapter

            Return myMac
        Catch ex As Exception
            Return String.Empty
        End Try
    End Function
End Module