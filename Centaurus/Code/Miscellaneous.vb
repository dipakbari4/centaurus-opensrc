Option Explicit On
' NOTE:  Option strict is not turned on here because
' the WinMgmts() and its InstancesOf methods prevents
' late binding operation.

Imports System.Management

Friend Module Miscellaneous
    Private counter As Byte = 0
    Private c As Color
    Private ReadOnly colors() As String =
        {"#FF8000", "#052EFF", "#FFD40E",
        "#00D1FF", "#E805FF", "#FF05A0",
        "#00FF00", "#FFFB00", "#FF0000", "#00FFD8"}

    ''' <summary>
    ''' Generates random colors for the heading.
    ''' </summary>
    ''' <returns>Color</returns>
    Public Function GenerateRandomColor() As Color
        If counter = 9 Then
            counter = 0
        End If

        c = ColorTranslator.FromHtml(colors(counter))
        counter += CByte(1)

        Return c
    End Function

    ''' <summary>
    ''' Categorizes the succeeded/failed/canceled map installation attempts.
    ''' </summary>
    ''' <param name="_mapName">Map Name</param>
    ''' <param name="_flag">success/started</param>
    Public Sub LogMapDownStatus(_mapName As String, _flag As String)
        ImportData(
            "Map History", _flag + ": " + _mapName + " installation attempt at " +
            Date.Now.ToString + vbNewLine + FetchData("Map History"))
    End Sub

    ''' <summary>
    ''' Verifies whether the required font is installed in the user system.
    ''' </summary>
    ''' <param name="fontName"></param>
    ''' <returns>Boolean</returns>
    Public Function IsFontInstalled(fontName As String) As Boolean
        ''''' VERSION 1.0
        Using TestFont As Font = New Font(fontName, 12)
            Return String.Compare(fontName,
                TestFont.Name,
                StringComparison.InvariantCultureIgnoreCase) = 0
        End Using

        ''''' VERSION 2.0
        'Using fontT As Font = New Font(fontName, 12, style, GraphicsUnit.Pixel)
        '    Return fontT.Name = FontFamily.Families
        'End Using

        ''''' VERSION 3.0
        'For Each fontFamily In New InstalledFontCollection().Families
        '    Return fontFamily.Name = fontName
        'Next
    End Function

    ''' <summary>
    ''' Gathers the motherboard ID of the device.
    ''' </summary>
    ''' <returns>String</returns>
    Public Function SystemSerialNumber() As String
        Dim mbs = New ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia")
        Dim mbsList As ManagementObjectCollection = mbs.[Get]()
        Dim id As String = ""

        For Each mo As ManagementObject In mbsList
            id = mo("SerialNumber").ToString()
            Exit For
        Next

        Return id
    End Function
End Module
