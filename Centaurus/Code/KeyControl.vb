Option Explicit On

Imports System.Convert

Friend Module KeyControl
    ''' <summary>
    ''' Restricts any other keys to be pressed except the specific keys.
    ''' </summary>
    ''' <param name="sender">sender Object</param>
    ''' <param name="allowed">Key chunks to be permitted</param>
    ''' <param name="e">On activity</param>
    Public Sub DetectKeys(sender As Object, allowed As String, e As KeyPressEventArgs)
        Try
            Select Case e.KeyChar
                Case ToChar(Keys.Back)
                    e.Handled = False

                Case ToChar(Keys.Capital Or Keys.RButton)
                    e.Handled = Not Clipboard.GetText().
                        All(Function(c) allowed.Contains(c))

                Case ToChar(Keys.ControlKey And Keys.A)
                    sender.Select()

                Case ToChar(Keys.ControlKey And Keys.C)
                    sender.Copy()

                Case ToChar(Keys.ControlKey And Keys.V)
                    sender.Text = Clipboard.GetText

                Case Else
                    e.Handled = Not allowed.Contains(e.KeyChar)
            End Select

        Catch ex As Exception
            '
        End Try
    End Sub
End Module
