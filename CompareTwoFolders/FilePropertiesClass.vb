Imports System.ComponentModel
Namespace CompareTwoFolders
    Public Class FilePropertiesClass
        Implements INotifyPropertyChanged
        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Private _FullPathFile As String = String.Empty
        Public Property FullPathFile As String
            Get
                Return _FullPathFile
            End Get
            Set(value As String)
                If _FullPathFile <> value Then
                    _FullPathFile = value
                    RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(FullPathFile)))
                End If
            End Set
        End Property

        Private _HashCode As Byte() = New Byte(31) {}
        Public Property HashCode As Byte()
            Get
                Return _HashCode
            End Get
            Set(value As Byte())
                If Not _HashCode?.SequenceEqual(value) Then
                    _HashCode = value
                    RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(HashCode)))
                End If
            End Set
        End Property
    End Class
End Namespace
