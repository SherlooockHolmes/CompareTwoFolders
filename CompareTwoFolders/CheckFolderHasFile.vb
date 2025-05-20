Imports System.IO
Namespace CompareTwoFolders
    Public Module CheckFolderHasFile
        Public Function HasAnyFile(folderPath As String) As Boolean
            Try
                If Directory.EnumerateFiles(folderPath).Any() Then Return True

                For Each subfolder In Directory.EnumerateDirectories(folderPath)
                    If HasAnyFile(subfolder) Then Return True
                Next subfolder

                Return False
            Catch ex As Exception
                Return False
            End Try
        End Function
    End Module
End Namespace
