Imports System.Collections.ObjectModel
Imports System.IO
Namespace CompareTwoFolders
    Public Module FilesAndFolderScanning
        Public Function GetFileCollection(folderPath As String) As ObservableCollection(Of FilePropertiesClass)
            If Directory.Exists(folderPath) Then
                Dim FilesCollection As New ObservableCollection(Of FilePropertiesClass)
                Dim allFiles As String() = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories)
                For Each filePath In allFiles
                    FilesCollection.Add(New FilePropertiesClass With {.FullPathFile = filePath, .HashCode = GetFileHash(filePath)})
                    ProgressBarPercent.Percentage += 50 / allFiles.Length
                Next filePath
                Return FilesCollection
            Else
                MsgBox("Folder not found: " & folderPath)
                Return Nothing
            End If
        End Function
    End Module
End Namespace
