Imports System.ComponentModel
Namespace CompareTwoFolders
    Public Class ProgressBarPercentClass
        Implements INotifyPropertyChanged
        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Private _Percentage As Double = 0
        Public Property Percentage As Double
            Get
                Return _Percentage
            End Get
            Set(value As Double)
                If _Percentage <> value Then
                    _Percentage = value
                    RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(Percentage)))
                End If
            End Set
        End Property

        Private _SimilarItemsFound As Double = 0
        Public Property SimilarItemsFound As Double
            Get
                Return _SimilarItemsFound
            End Get
            Set(value As Double)
                If _SimilarItemsFound <> value Then
                    _SimilarItemsFound = value
                    RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(SimilarItemsFound)))
                End If
            End Set
        End Property
    End Class
End Namespace
