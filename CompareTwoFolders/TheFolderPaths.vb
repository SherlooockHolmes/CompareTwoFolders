Imports System.ComponentModel
Namespace CompareTwoFolders
    Public Class TheFolderPaths
        Implements INotifyPropertyChanged
        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Private _LeftFolderPath As String = String.Empty
        Public Property LeftFolderPath As String
            Get
                Return _LeftFolderPath
            End Get
            Set(value As String)
                If _LeftFolderPath <> value Then
                    _LeftFolderPath = value
                    RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(LeftFolderPath)))
                End If
            End Set
        End Property

        Private _RightFolderPath As String = String.Empty
        Public Property RightFolderPath As String
            Get
                Return _RightFolderPath
            End Get
            Set(value As String)
                If _RightFolderPath <> value Then
                    _RightFolderPath = value
                    RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(RightFolderPath)))
                End If
            End Set
        End Property
    End Class
End Namespace
