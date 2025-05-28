Imports System.IO
Imports Microsoft.VisualBasic.FileIO
Namespace CompareTwoFolders
    Public Class MainWindow
        Inherits Window
        Public Sub New()
            InitializeComponent()
            PrepareProgressBar()
        End Sub
        Private Sub PrepareProgressBar()
            Dim binding As New Binding With {
                .Path = New PropertyPath(NameOf(ProgressBarPercent.Percentage)),
                .Source = ProgressBarPercent,
                .Mode = BindingMode.OneWay,
                .UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                .StringFormat = "{0:F2}"
            }
            BindingOperations.SetBinding(TextBlockProgressBar, TextBlock.TextProperty, binding)
        End Sub
        Private Sub ButtonSelectLeftFolder_Click(sender As Object, e As RoutedEventArgs)
            SelectFolder("L")
            TextBoxOpenLeftFolder.Text = FolderPaths.LeftFolderPath
        End Sub
        Private Sub ButtonSelectRightFolder_Click(sender As Object, e As RoutedEventArgs)
            SelectFolder("R")
            TextBoxOpenRightFolder.Text = FolderPaths.RightFolderPath
        End Sub
        Private Sub SelectFolder(WhichPanel As Char)
            Dim windowHandle = New Interop.WindowInteropHelper(Me).Handle
            Dim selectedPath As String = FolderPickerHelper.SelectFolder(windowHandle, "Please select a folder", allowNewFolderButton:=False)
            If Not String.IsNullOrEmpty(selectedPath) Then
                Select Case WhichPanel
                    Case "L" : FolderPaths.LeftFolderPath = selectedPath
                    Case "R" : FolderPaths.RightFolderPath = selectedPath
                End Select
            End If
        End Sub
        Private Async Sub ButtonStartStopSearch_Click(sender As Object, e As RoutedEventArgs)
            If TextBlockStartStopSearch.Text = "Stop Search" Then IsCancelRequestedInFileSeeking = True : Exit Sub
            If String.IsNullOrWhiteSpace(FolderPaths.LeftFolderPath) Or String.IsNullOrWhiteSpace(FolderPaths.RightFolderPath) Then
                MsgBox("Please set both left and right comparing folders.")
                Exit Sub
            ElseIf Not IO.Directory.Exists(FolderPaths.LeftFolderPath) Then
                MsgBox("Folder does not exist: " & FolderPaths.LeftFolderPath)
                Exit Sub
            ElseIf Not IO.Directory.Exists(FolderPaths.RightFolderPath) Then
                MsgBox("Folder does not exist: " & FolderPaths.LeftFolderPath)
                Exit Sub
            ElseIf Not HasAnyFile(FolderPaths.LeftFolderPath) Then
                MsgBox("Folder does not have any file: " & FolderPaths.LeftFolderPath)
                Exit Sub
            ElseIf Not HasAnyFile(FolderPaths.RightFolderPath) Then
                MsgBox("Folder does not have any file: " & FolderPaths.RightFolderPath)
                Exit Sub
            End If
            Select Case True'Sets CompareBy that indicates what protocol to compare.
                Case RadioButtonCompareByFileName.IsChecked
                    CommpareBy = "CompareByFileName"
                Case RadioButtonCompareByHashCode.IsChecked
                    CommpareBy = "CompareByHashCode"
            End Select
            DataGridFolders.ItemsSource = Nothing
            ButtonEraseDuplicates.IsEnabled = False
            RadioButtonCompareByFileName.IsEnabled = False
            RadioButtonCompareByHashCode.IsEnabled = False
            TextBlockStartStopSearch.Text = "Stop Search"
            ButtonOpenLeftFolder.IsEnabled = False
            ButtonOpenRightFolder.IsEnabled = False
            ProgressBarPercent.Percentage = 0
            DoEvents()
            Dim TaskGetFileCollectionLEFT = Task.Run(Sub() FilesOfLeft = GetFileCollection(FolderPaths.LeftFolderPath))
            Dim TaskGetFileCollectionRIGHT = Task.Run(Sub() FilesOfRight = GetFileCollection(FolderPaths.RightFolderPath))
            Await TaskGetFileCollectionLEFT
            Await TaskGetFileCollectionRIGHT
            DoEvents()
            Windows.Application.Current.Dispatcher.Invoke(Sub() StartCompareTwoFolders())
            DoEvents()
            DataGridFolders.ItemsSource = FinalFilesView
            ProgressBarPercent.Percentage = If(Not IsCancelRequestedInFileSeeking, 100, 0)
            IsCancelRequestedInFileSeeking = False
            ButtonEraseDuplicates.IsEnabled = FinalFiles.Count > 0
            RadioButtonCompareByFileName.IsEnabled = True
            RadioButtonCompareByHashCode.IsEnabled = True
            TextBlockStartStopSearch.Text = "Start Search"
            ButtonOpenLeftFolder.IsEnabled = True
            ButtonOpenRightFolder.IsEnabled = True
        End Sub
        Private Sub ButtonEraseDuplicates_Click(sender As Object, e As RoutedEventArgs)
            If MessageBox.Show($"Are you sure to erase?{vbCrLf}Files will move to Recycle.Bin.", "Warning", MessageBoxButton.YesNo) = 7 Then Exit Sub
            For i As Integer = FinalFiles.Count - 1 To 0 Step -1
                Dim BtnL As Boolean = FinalFiles(i).ButtonEraseLeftData
                Dim BtnR As Boolean = FinalFiles(i).ButtonEraseRightData
                Dim DelL As String = FinalFiles(i).LeftData
                Dim DelR As String = FinalFiles(i).RightData
                If BtnL AndAlso File.Exists(DelL) Then FileSystem.DeleteFile(DelL,
                                                                             UIOption.OnlyErrorDialogs,
                                                                             RecycleOption.SendToRecycleBin)
                If BtnR AndAlso File.Exists(DelR) Then FileSystem.DeleteFile(DelR,
                                                                             UIOption.OnlyErrorDialogs,
                                                                             RecycleOption.SendToRecycleBin)
                If BtnL Or BtnR Then FinalFiles.RemoveAt(i)
            Next i
        End Sub
        Private Sub DataGridFolders_PreviewMouseDoubleClick(sender As Object, e As MouseButtonEventArgs)
            Dim depObj As DependencyObject = CType(e.OriginalSource, DependencyObject)

            ' Traverse the visual tree upward until we find a DataGridCell
            While depObj IsNot Nothing AndAlso TypeOf depObj IsNot DataGridCell
                depObj = VisualTreeHelper.GetParent(depObj)
            End While

            If depObj IsNot Nothing Then
                Dim cell As DataGridCell = CType(depObj, DataGridCell)

                ' Check if the cell content is a TextBlock (i.e. a normal text cell)
                If TypeOf cell.Content Is TextBlock Then
                    Dim textBlock As TextBlock = CType(cell.Content, TextBlock)
                    If File.Exists(textBlock.Text) Then
                        Dim psi As New ProcessStartInfo(textBlock.Text)
                        psi.UseShellExecute = True
                        Process.Start(psi)
                    End If
                End If
            End If
        End Sub
        Private Sub ToggleButtonFilterExt_Click(sender As Object, e As RoutedEventArgs)
            Dim blockedExtensions As List(Of String) =
                TextBoxFilterExt.Text.Split(";"c).
                Select(Function(ext)
                           Dim trimmedExt = ext.Trim().ToLower().TrimStart("*"c).Trim()
                           If Not trimmedExt.StartsWith(".") Then trimmedExt = "." & trimmedExt
                           Return Path.GetExtension(trimmedExt)
                       End Function).ToList()
            If ToggleButtonFilterExt.IsChecked Then
                FinalFilesView.Filter = Function(item)
                                            Dim file = TryCast(item, ItemSourceOfDataGrid)
                                            If file Is Nothing Then Return False
                                            Dim leftBlocked = blockedExtensions.Any(Function(ext) Path.GetExtension(file.LeftData).ToLower() = ext)
                                            Dim rightBlocked = blockedExtensions.Any(Function(ext) Path.GetExtension(file.RightData).ToLower() = ext)
                                            Return Not (leftBlocked Or rightBlocked)
                                        End Function
            Else
                FinalFilesView.Filter = Nothing
            End If
        End Sub
    End Class
End Namespace
