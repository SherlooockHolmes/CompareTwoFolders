Imports System.Collections.ObjectModel
Imports System.ComponentModel

Namespace CompareTwoFolders
    Public Module GlobalVariables
        Public Property ProgressBarPercent As New ProgressBarPercentClass
        Public Property FolderPaths As New TheFolderPaths
        Public Property FilesOfLeft As New ObservableCollection(Of FilePropertiesClass)
        Public Property FilesOfRight As New ObservableCollection(Of FilePropertiesClass)
        Public Property FinalFiles As New ObservableCollection(Of ItemSourceOfDataGrid)
        Public Property FinalFilesView As ICollectionView = CollectionViewSource.GetDefaultView(FinalFiles)
        Public Property CommpareBy As String
    End Module
End Namespace
