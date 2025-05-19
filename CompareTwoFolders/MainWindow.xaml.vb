Namespace CompareTwoFolders
    Public Class MainWindow
        Inherits System.Windows.Window
        Private Sub ButtonSelectLeftFolder_Click(sender As Object, e As RoutedEventArgs)
            SelectFolder("L")
            TextBoxOpenLeftFolder.Text = FolderPaths.LeftFolderPath
        End Sub
        Private Sub ButtonSelectRightFolder_Click(sender As Object, e As RoutedEventArgs)
            SelectFolder("R")
            TextBoxOpenRightFolder.Text = FolderPaths.RightFolderPath
        End Sub
        Private Sub SelectFolder(WhichPanel As Char)
            Dim windowHandle = New Interop.WindowInteropHelper(Me).Handle
            Dim selectedPath As String = FolderPickerHelper.SelectFolder(windowHandle, "Please select a folder", allowNewFolderButton:=True)
            If Not String.IsNullOrEmpty(selectedPath) Then
                Select Case WhichPanel
                    Case "L" : FolderPaths.LeftFolderPath = selectedPath
                    Case "R" : FolderPaths.RightFolderPath = selectedPath
                End Select
            End If
        End Sub
    End Class
End Namespace
