Option Strict On
Option Explicit On

Imports System.Net
Imports System.Text.Encoding
Imports System.Net.Sockets

Friend Module ServerDataFetcher
    Private receivedBytes As String = ""
    Private ReadOnly count As Byte
    Private ReadOnly _netClient As New UdpClient(1111)

    ''' <summary>
    ''' Players information obtainer from the server.
    ''' </summary>
    ''' <returns>String</returns>
    Public Function GetPlayers(sIP As String, sPort As Integer) As String
        Try
            With _netClient
                .Client.ReceiveTimeout = 2000
                .Connect(sIP, sPort)
            End With

            Dim getBytes() As Byte = ASCII.GetBytes("\status\")
            _netClient.Send(getBytes, getBytes.Length)

            Dim endPoint As IPEndPoint = New IPEndPoint(IPAddress.Any, 0)
            Dim received() As Byte = _netClient.Receive(endPoint)
            receivedBytes = ASCII.GetString(received)

            Dim count As String = receivedBytes(receivedBytes.IndexOf("numplayers") + 11)
            Dim cp1 As String = receivedBytes(receivedBytes.IndexOf("numplayers") + 12)

            If cp1 = "\" Then
            Else
                count += cp1
            End If

            Return count

        Catch ex1 As NullReferenceException
            Return "-2"
        Catch ex2 As SocketException
            Return "-2"
        Catch ex3 As Exception
            Return "-2"
        End Try
    End Function

    ''' <summary>
    ''' Obtains the information of active map.
    ''' </summary>
    ''' <returns>String</returns>
    Public Function GetMapName() As String
        Try
            Dim as5 As Integer = 0
            If receivedBytes.IndexOf("mapname") = Nothing Then
                as5 = 8
            Else
                as5 = receivedBytes.IndexOf("mapname") + 8
            End If

            Dim cha As Char = receivedBytes(as5)
            Dim chb As Char = receivedBytes(as5 + 1)
            Dim chc As Char = receivedBytes(as5 + 2)
            Dim chd As Char = receivedBytes(as5 + 3)
            Dim che As Char = receivedBytes(as5 + 4)
            Dim chf As Char = receivedBytes(as5 + 5)
            Dim chg As Char = receivedBytes(as5 + 6)
            Dim chh As Char = receivedBytes(as5 + 7)

            If chh = "\" Then
                chh = CChar("")
                GoTo halla
            End If

            Dim chi As Char = receivedBytes(as5 + 8)
            If chi = "\" Then
                chi = CChar("")
                GoTo halla
            End If

            Dim chj As Char = receivedBytes(as5 + 9)
            If chj = "\" Then
                chj = CChar("")
                GoTo halla
            End If

            Dim chk As Char = receivedBytes(as5 + 10)
            If chk = "\" Then
                chk = CChar("")
                GoTo halla
            End If

            Dim chl As Char = receivedBytes(as5 + 11)
            If chl = "\" Then
                chl = CChar("")
                GoTo halla
            End If
            Dim chm As Char = receivedBytes(as5 + 12)
            If chm = "\" Then
                chm = CChar("")
                GoTo halla
            End If
            Dim chn As Char = receivedBytes(as5 + 13)
            If chn = "\" Then
                chn = CChar("")
                GoTo halla
            End If

            Dim cho As Char = receivedBytes(as5 + 14)
            If cho = "\" Then
                cho = CChar("")
                GoTo halla
            End If

            Dim chp As Char = receivedBytes(as5 + 15)
            If chp = "\" Then
                chp = CChar("")
                GoTo halla
            End If

            Dim chq As Char = receivedBytes(as5 + 16)
            If chq = "\" Then
                chq = CChar("")
                GoTo halla
            End If

            Dim chr As Char = receivedBytes(as5 + 17)
            If chr = "\" Then
                chr = CChar("")
                GoTo halla
            End If

            Dim chs As Char = receivedBytes(as5 + 18)
            If chs = "\" Then
                chs = CChar("")
                GoTo halla
            End If

            Dim cht As Char = receivedBytes(as5 + 19)
            If cht = "\" Then
                cht = CChar("")
                GoTo halla
            End If

            Dim chu As Char = receivedBytes(as5 + 20)
            If chu = "\" Then
                chu = CChar("")
                GoTo halla
            End If

            Dim chv As Char = receivedBytes(as5 + 21)
            If chv = "\" Then
                chv = CChar("")
                GoTo halla
            End If

            Dim chw As Char = receivedBytes(as5 + 22)
            If chw = "\" Then
                chw = CChar("")
                GoTo halla
            End If
            Dim chx As Char = receivedBytes(as5 + 23)
            If chx = "\" Then
                chx = CChar("")
                GoTo halla
            End If

            Dim chy As Char = receivedBytes(as5 + 24)
            If chy = "\" Then
                chy = CChar("")
                GoTo halla
            End If

            Dim chz As Char = receivedBytes(as5 + 25)
            If chz = "\" Then
                chz = CChar("")
                GoTo halla
            End If
halla:

            Dim mapa As String = cha + chb + chc + chd + che + chf +
                chg + chh + chi + chj + chk + chl + chm + chn + cho +
                chp + chq + chr + chs + cht + chu + chv + chw + chx +
                chy + chz

            Return mapa
        Catch ex1 As Exception
        End Try

        ' The AFOREMENTIONED CODE was provided by Mr. Sudhanshu Kumar

        '		Try
        '			Dim as5 As Integer = receivedBytes.IndexOf("mapname") + 8
        '			Dim map() As Char = ""

        '			For index = 0 To 7
        '				map(index) = receivedBytes(as5 + index)
        '			Next

        '			If map(7) = "\" Then
        '				map(7) = ""
        '				GoTo halla
        '			End If

        '			For index = 8 To 25
        '				map(index) = receivedBytes(as5 + index)

        '				If map(index) = "\" Then
        '					map(index) = ""
        '					GoTo halla
        '				End If
        '			Next

        'halla:
        '			For index = 0 To 25
        '				mapName += map(index)
        '			Next

        '			Return mapName

        '		Catch ex As Exception

        '		End Try
        Return ""
    End Function
End Module
