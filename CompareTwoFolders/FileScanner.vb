Imports System.Collections.Concurrent
Imports System.Collections.ObjectModel
Imports System.IO
Imports System.Security.Cryptography
Imports System.Threading

Namespace CompareTwoFolders
    Public Module FileScanner

        Private Const MaxConcurrency As Integer = 5
        Private allFilesCount As Integer = 0
        Public Function GetFileCollection(folderPath As String) As ObservableCollection(Of FilePropertiesClass)

            Dim FilesCollectionConcurrent As New ConcurrentBag(Of FilePropertiesClass) 'کنکارنت کالکشن، برای دسترسی های موازی و امن به مجموعه طراحی شده که منم استفاده کردم
            Dim FilesCollection As New ObservableCollection(Of FilePropertiesClass)

            ' Synchronous entry, run async code and wait
            FilesCollectionConcurrent = Task.Run(Async Function() Await GetFileCollectionAsync(folderPath)).Result

            'Import to ObservableCollection
            For Each item In FilesCollectionConcurrent
                FilesCollection.Add(item)
            Next item

            Return FilesCollection

        End Function

        Private Async Function GetFileCollectionAsync(folderPath As String) As Task(Of ConcurrentBag(Of FilePropertiesClass))
            Dim result As New ConcurrentBag(Of FilePropertiesClass)
            Dim semaphore As New SemaphoreSlim(MaxConcurrency)
            Dim allFiles As List(Of String)

            Try
                allFiles = Directory.EnumerateFiles(folderPath, "*.*", SearchOption.AllDirectories).ToList()
            Catch ex As Exception
                MsgBox("Folder access error.")
                Return result
            End Try

            allFilesCount = allFiles.Count

            Dim tasks = allFiles.Select(Function(filePath) ProcessFileAsync(filePath, semaphore, result))
            Await Task.WhenAll(tasks)
            Return result
        End Function

        Private Async Function ProcessFileAsync(filePath As String, semaphore As SemaphoreSlim, result As ConcurrentBag(Of FilePropertiesClass)) As Task
            Await semaphore.WaitAsync(CancelSearching.Token)
            Try
                CancelSearching.Token.ThrowIfCancellationRequested()

                Dim hash = Await ComputeFileHashAsync(filePath)
                Dim fileProps As New FilePropertiesClass With {
                    .FullPathFile = filePath,
                    .HashCode = hash
                }

                SyncLock result
                    result.Add(fileProps)
                End SyncLock

                ProgressBarPercent.Percentage += 50 / allFilesCount

            Catch ex As Exception When TypeOf ex Is OperationCanceledException OrElse True
                MsgBox($"Process canceled.")
            Finally
                semaphore.Release()
            End Try
        End Function

        Private Async Function ComputeFileHashAsync(filePath As String) As Task(Of Byte())
            Using sha256 As SHA256 = SHA256.Create(),
              fs As New FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 8192, useAsync:=True)

                Dim buffer(8191) As Byte
                Dim bytesRead As Integer
                sha256.Initialize()

                Do
                    bytesRead = Await fs.ReadAsync(buffer, 0, buffer.Length, CancelSearching.Token)
                    If bytesRead > 0 Then
                        sha256.TransformBlock(buffer, 0, bytesRead, Nothing, 0)
                    End If
                    CancelSearching.Token.ThrowIfCancellationRequested()
                Loop While bytesRead > 0

                sha256.TransformFinalBlock(Array.Empty(Of Byte)(), 0, 0)
                Return sha256.Hash
            End Using
        End Function

    End Module
End Namespace