Imports System.ComponentModel
Namespace CompareTwoFolders
    Public Class ItemSourceOfDataGrid
        Implements INotifyPropertyChanged
        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Private _LeftData As String = String.Empty
        Public Property LeftData As String
            Get
                Return _LeftData
            End Get
            Set(value As String)
                If _LeftData <> value Then
                    _LeftData = value
                    RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(LeftData)))
                End If
            End Set
        End Property

        Private _ButtonEraseLeftData As Boolean = False
        Public Property ButtonEraseLeftData As Boolean
            Get
                Return _ButtonEraseLeftData
            End Get
            Set(value As Boolean)
                If _ButtonEraseLeftData <> value Then
                    _ButtonEraseLeftData = value
                    RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(ButtonEraseLeftData)))
                End If
            End Set
        End Property

        Private _ButtonEraseRightData As Boolean = False
        Public Property ButtonEraseRightData As Boolean
            Get
                Return _ButtonEraseRightData
            End Get
            Set(value As Boolean)
                If _ButtonEraseRightData <> value Then
                    _ButtonEraseRightData = value
                    RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(ButtonEraseRightData)))
                End If
            End Set
        End Property

        Private _RightData As String = String.Empty
        Public Property RightData As String
            Get
                Return _RightData
            End Get
            Set(value As String)
                If _RightData <> value Then
                    _RightData = value
                    RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(RightData)))
                End If
            End Set
        End Property
    End Class
End Namespace
