Imports System.Collections.ObjectModel
Namespace CompareTwoFolders
    Public Module GlobalVariables
        Public Property FolderPaths As New TheFolderPaths
        Public Property FinalFiles As New ObservableCollection(Of ItemSourceOfDataGrid)
        Public Property FilesOfLeft As New ObservableCollection(Of FilePropertiesClass)
        Public Property FilesOfRight As New ObservableCollection(Of FilePropertiesClass)
    End Module
End Namespace
