Imports System.Security.Cryptography
Imports System.IO
Namespace CompareTwoFolders
    Public Module GettingFileHash
        Function GetFileHash(filePath As String) As Byte()
            Using stream As FileStream = File.OpenRead(filePath)
                Using sha256 As SHA256 = SHA256.Create()
                    Return sha256.ComputeHash(stream)
                End Using
            End Using
        End Function
    End Module
End Namespace
