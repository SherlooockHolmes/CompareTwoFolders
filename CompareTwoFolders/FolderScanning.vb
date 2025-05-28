Imports System.Collections.ObjectModel
Imports System.IO
Namespace CompareTwoFolders
    Public Module FilesAndFolderScanning
        Public Function GetFileCollection(folderPath As String) As ObservableCollection(Of FilePropertiesClass)
            If Directory.Exists(folderPath) Then
                Dim FilesCollection As New ObservableCollection(Of FilePropertiesClass)
                Dim FilesCollectionConcurrent As New Concurrent.ConcurrentBag(Of FilePropertiesClass)'این نوع کالکشن، برای دسترسی های موازی و امن به مجموعه طراحی شده که منم استفاده کردم
                Dim allFiles As String() = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories)
                Try
                    Parallel.ForEach(allFiles, New ParallelOptions With {.CancellationToken = CancelSearching.Token}, Sub(filePath, state)
                                                                                                                          CancelSearching.Token.ThrowIfCancellationRequested()
                                                                                                                          If IsCancelRequestedInFileSeeking Then Return
                                                                                                                          FilesCollectionConcurrent.Add(New FilePropertiesClass With {.FullPathFile = filePath, .HashCode = If(CommpareBy = "CompareByHashCode", GetFileHash(filePath), New Byte(31) {})})
                                                                                                                          ProgressBarPercent.Percentage += 50 / allFiles.Length
                                                                                                                          ProgressBarPercent.SimilarItemsFound = FilesCollectionConcurrent.Count
                                                                                                                      End Sub)
                Catch ex As Exception

                End Try

                For Each item In FilesCollectionConcurrent
                    FilesCollection.Add(item)
                Next item
                Return FilesCollection
            Else
                MsgBox("Folder not found: " & folderPath)
                Return Nothing
            End If
        End Function
    End Module
End Namespace
