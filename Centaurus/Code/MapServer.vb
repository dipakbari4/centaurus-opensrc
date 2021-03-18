Option Strict On
Option Explicit On

Imports System.IO.Compression
Imports System.IO.Path
Imports System.IO.File
Imports System.Net

Friend Module MapServer
    Private WithEvents MdWClient As New WebClient
    Private ReadOnly path As String = mainPath + "\MISSIONS\multiplayer\"
    Private count As Integer = 0
    Private remaining As Integer = 0

    Public Property CManip As Integer
        Get
            Return count
        End Get
        Set(value As Integer)
            count = value
        End Set
    End Property

    ''' <summary>
    ''' Retrives map data information.
    ''' </summary>
    Public Sub FetchMaps()
        Try
            MapDown.MapBox.Items.Clear()

            For Each map In mapList
                MapDown.MapBox.Items.Add(map.Replace(vbLf, "").Replace(".zip", ""))
            Next

            MapDown.MapBox.SelectedIndex = 0

        Catch ex As Exception
            Prompt("Unable to fetch the map list data.", "Maps Not Received", 2)
            MapDown.Close()
        End Try
    End Sub

    ''' <summary>
    ''' Downloads the map from the server to a specified location.
    ''' </summary>
    ''' <param name="param">Map name to be downloaded</param>
    Public Sub MapDownloader(param As String)
        Dim selected As String = mapDownServer + param

        Try
            If MdWClient.IsBusy Then
                Prompt("MapDown is performing another task currently.",
                       "Already Running", 2)
                Return
            End If

            With MapDown
                .Button1.Enabled = False
                .MapBox.Enabled = False
            End With

            MapDown.LinkLabel4.Visible = True
            MapDown.Label4.Visible = True

            LogMapDownStatus(param.Replace(".zip", ""), "started")

            MapDown.TLap.Start()

            MdWClient.DownloadFileAsync(New Uri(selected), tempPath + param)
            AddHandler MdWClient.DownloadFileCompleted, AddressOf AsyncCompleteEventRaiser

            MapDown.Label3.Visible = True

        Catch ex As Exception
            Prompt("The map could not be downloaded. Please try again later.",
                   "Map Download Failure", 2)
        End Try
    End Sub

    Private Sub ProgressChanged(sender As Object, e As DownloadProgressChangedEventArgs) _
        Handles MdWClient.DownloadProgressChanged

        With MapDown
            .ProgressBar1.Value = e.ProgressPercentage

            .Label4.Text = Format(e.BytesReceived / 1024D / 1024D, "0.00") + " MB / " +
                Format(e.TotalBytesToReceive / 1024D / 1024D, "0.00") + " MB" + " (" +
                e.ProgressPercentage.ToString + "%)"
        End With

        If e.ProgressPercentage = 0 Then
            Return
        End If

        remaining = CInt((100 - e.ProgressPercentage) * CManip / e.ProgressPercentage)

        If remaining = 60 Then
            MapDown.Label3.Text = "Just a minute left"
        ElseIf remaining < 60 Then
            MapDown.Label3.Text = remaining.ToString + " seconds left"
        ElseIf remaining > 60 Then
            MapDown.Label3.Text = (CInt(remaining / 60)).ToString + " minute(s) and " + (remaining Mod 60).ToString + " seconds left"
        End If
    End Sub

    Private Sub AsyncCompleteEventRaiser(sender As Object, e As ComponentModel.AsyncCompletedEventArgs)
        With MapDown
            .Button1.Enabled = True
            .Label4.Visible = False
            .Label3.Visible = False

            .ProgressBar1.Value = 0
            .Label4.Text = "Status"
            .LinkLabel4.Visible = False
        End With

        MapDown.TLap.Stop()
        CManip = 0

        MdWClient.CancelAsync()
        MdWClient.Dispose()

        Decompressor(tempPath + MapDown.MapBox.SelectedItem.ToString + ".zip")
    End Sub

    ''' <summary>
    ''' Cancels the download process of MapDown.
    ''' </summary>
    Public Sub CancelDownload()
        Try
            MdWClient.CancelAsync()
            MdWClient.Dispose()

            With MapDown
                .Label4.Text = "Status"
                .Label4.Visible = False
                .Label3.Text = "Calculating remaining time..."
                .Label3.Visible = False
                .Button1.Enabled = True
                .LinkLabel4.Visible = False
            End With

            MapDown.TLap.Stop()
            CManip = 0

            Delete(tempPath + MapDown.MapBox.SelectedItem.ToString + ".zip")

        Catch ex As Exception
            ' Prompt(ex.Message, "MapDown Unable to Stop Download", 2)
        End Try
    End Sub

    ''' <summary>
    ''' Extracts the files of the zipped map and installs it.
    ''' </summary>
    ''' <param name="mapPath">Location of the zipped map</param>
    Public Sub Decompressor(mapPath As String)
        Dim mapName As String = GetFileNameWithoutExtension(mapPath)
        Dim targetExtension As String = GetExtension(mapPath)

        MapDown.MapBox.Enabled = True

        Try
            ZipFile.ExtractToDirectory(mapPath, path)
            Notify("The map " + mapName + " was successfully installed.", "Map Installed")

            LogMapDownStatus(MapDown.MapBox.SelectedItem.ToString, "success")

        Catch ex As Exception
            MdWClient.Dispose()
        End Try

        If Exists(mapPath) Then
            Delete(mapPath)
        End If
    End Sub
End Module
