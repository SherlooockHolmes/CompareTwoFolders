Namespace CompareTwoFolders
    Public Class MainWindow
        Inherits Window
        Public Sub New()
            InitializeComponent()
            PrepareProgressBar()
            DataGridFolders.ItemsSource = FinalFiles
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
            Dim selectedPath As String = FolderPickerHelper.SelectFolder(windowHandle, "Please select a folder", allowNewFolderButton:=True)
            If Not String.IsNullOrEmpty(selectedPath) Then
                Select Case WhichPanel
                    Case "L" : FolderPaths.LeftFolderPath = selectedPath
                    Case "R" : FolderPaths.RightFolderPath = selectedPath
                End Select
            End If
        End Sub
        Private Async Sub ButtonStartSearch_Click(sender As Object, e As RoutedEventArgs)
            FolderPaths.LeftFolderPath = "D:\1"
            FolderPaths.RightFolderPath = "D:\2"
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
            ButtonStartSearch.IsEnabled = False
            ButtonOpenLeftFolder.IsEnabled = False
            ButtonOpenRightFolder.IsEnabled = False
            Dim TaskGetFileCollectionLEFT = Task.Run(Sub() FilesOfLeft = GetFileCollection(FolderPaths.LeftFolderPath))
            Dim TaskGetFileCollectionRIGHT = Task.Run(Sub() FilesOfRight = GetFileCollection(FolderPaths.RightFolderPath))
            Await TaskGetFileCollectionLEFT
            Await TaskGetFileCollectionRIGHT
            StartCompareTwoFolders()
            ProgressBarPercent.Percentage = 100
            ButtonEraseDuplicates.IsEnabled = FinalFiles.Count > 0
            ButtonStartSearch.IsEnabled = True
            ButtonOpenLeftFolder.IsEnabled = True
            ButtonOpenRightFolder.IsEnabled = True
        End Sub
        Private Sub ButtonEraseDuplicates_Click(sender As Object, e As RoutedEventArgs)

        End Sub
    End Class
End Namespace
