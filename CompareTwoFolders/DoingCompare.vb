Namespace CompareTwoFolders
    Public Module DoingCompare
        Public Sub StartCompareTwoFolders()
            FinalFiles.Clear()
            Dim keySelector As Func(Of FilePropertiesClass, String) = Nothing
            Select Case CommpareBy
                Case "CompareByFileName"
                    keySelector = Function(f) IO.Path.GetFileName(f.FullPathFile)
                Case "CompareByHashCode"
                    keySelector = Function(f) BitConverter.ToString(f.HashCode)
            End Select
            If keySelector IsNot Nothing Then
                Dim rightDict = FilesOfRight.GroupBy(keySelector).ToDictionary(Function(g) g.Key, Function(g) g.ToList())
                For Each leftFile In FilesOfLeft
                    Dim key = keySelector(leftFile)
                    If rightDict.ContainsKey(key) Then
                        For Each rightFile In rightDict(key)
                            FinalFiles.Add(New ItemSourceOfDataGrid With {
                                           .LeftData = leftFile.FullPathFile,
                                           .ButtonEraseLeftData = False,
                                           .ButtonEraseRightData = False,
                                           .RightData = rightFile.FullPathFile
                                           })
                        Next rightFile
                    End If
                Next leftFile
            End If
            If FinalFiles.Count = 0 Then MsgBox("No similar files were found.")
        End Sub
    End Module
End Namespace
