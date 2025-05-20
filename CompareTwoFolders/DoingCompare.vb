Namespace CompareTwoFolders
    Public Module DoingCompare
        Public Sub StartCompareTwoFolders()
            FinalFiles.Clear()
            Dim rightHashDict = FilesOfRight.GroupBy(Function(f) BitConverter.ToString(f.HashCode)).ToDictionary(Function(g) g.Key, Function(g) g.ToList())
            For Each leftFile In FilesOfLeft
                Dim leftHashKey As String = BitConverter.ToString(leftFile.HashCode)
                If rightHashDict.ContainsKey(leftHashKey) Then
                    Dim matchingRightFiles = rightHashDict(leftHashKey)
                    For Each rightFile In matchingRightFiles
                        FinalFiles.Add(New ItemSourceOfDataGrid With {
                                       .LeftData = leftFile.FullPathFile,
                                       .ButtonEraseLeftData = False,
                                       .ButtonEraseRightData = False,
                                       .RightData = rightFile.FullPathFile
                                       })
                    Next rightFile
                End If
            Next leftFile
            If FinalFiles.Count = 0 Then MsgBox("No similar files were found.")
        End Sub
    End Module
End Namespace
