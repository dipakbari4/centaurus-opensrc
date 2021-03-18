
Option Explicit On

Friend Module RegistryControl
    ''' <summary>
    ''' Imports the registry details to Windows Registry.
    ''' </summary>
    ''' <param name="valueName">Key where value to be placed</param>
    ''' <param name="value">The container for the key</param>
    Public Sub ImportData(valueName As String, value As String)
        Try
            Dim aen As String = New TripleDES("CTLN").Encrypt(valueName)
            Dim cipher As String = New TripleDES(valueName).Encrypt(value)
            My.Computer.Registry.SetValue(CTLN, aen, cipher)

        Catch ex As Exception
            Prompt("Unable to write the data to the system.", "Data Write Failure", 2)
        End Try
    End Sub

    ''' <summary>
    ''' Get the registry details from Windows Registry.
    ''' </summary>
    ''' <param name="valueName">Source key to be obtained</param>
    ''' <returns>String</returns>
    Public Function FetchData(valueName As String) As String
        Try
            Dim aen As String = New TripleDES("CTLN").Encrypt(valueName)
            Dim decryptedData As String = New TripleDES(valueName).
                Decrypt(My.Computer.Registry.GetValue(CTLN, aen, Nothing))

            Return decryptedData

        Catch ex As NullReferenceException
            Prompt("Unable to read data from the system.", "Data Read Failure", 2)
        End Try

        Return Nothing
    End Function

    ''' <summary>
    ''' Counts and increments from registry.
    ''' </summary>
    ''' <param name="registryName">Name of the registry to be incremented</param>
    ''' <returns>String</returns>
    Public Function CountData(registryName As String) As String
        Try
            number = CStr(CInt(FetchData(registryName)) + 1)
            ImportData(registryName, number)

        Catch ex As Exception
            ImportData(registryName, 0.ToString)
        End Try

        Return number
    End Function
End Module
