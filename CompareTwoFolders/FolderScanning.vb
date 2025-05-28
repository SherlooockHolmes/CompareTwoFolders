Imports System.Collections.ObjectModel
Imports System.IO
Namespace CompareTwoFolders
    Public Module FilesAndFolderScanning
        Public Function GetFileCollection(folderPath As String) As ObservableCollection(Of FilePropertiesClass)
            If Directory.Exists(folderPath) Then
                Dim FilesCollection As New ObservableCollection(Of FilePropertiesClass)
                Dim allFiles As String() = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories)
                For Each filePath In allFiles
                    If IsCancelRequestedInFileSeeking Then Exit For
                    FilesCollection.Add(New FilePropertiesClass With {.FullPathFile = filePath, .HashCode = If(CommpareBy = "CompareByHashCode", GetFileHash(filePath), New Byte(31) {})})
                    ProgressBarPercent.Percentage += 50 / allFiles.Length
                    DoEvents()
                Next filePath
                Return FilesCollection
            Else
                MsgBox("Folder not found: " & folderPath)
                Return Nothing
            End If
        End Function
    End Module
End Namespace
