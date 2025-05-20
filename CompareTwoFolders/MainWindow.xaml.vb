Namespace CompareTwoFolders
    Public Class MainWindow
        Inherits Window
        Public Sub New()
            InitializeComponent()
            DataGridFolders.ItemsSource = FinalFiles
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
        Private Sub ButtonStartSearch_Click(sender As Object, e As RoutedEventArgs)
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

            FilesOfLeft = GetFileCollection(FolderPaths.LeftFolderPath)
            FilesOfRight = GetFileCollection(FolderPaths.RightFolderPath)

            DoCompareTwoFolders()

            ButtonEraseDuplicates.IsEnabled = True

            DataGridFolders.ItemsSource = FinalFiles

        End Sub
        Private Sub ButtonEraseDuplicates_Click(sender As Object, e As RoutedEventArgs)

        End Sub
    End Class
End Namespace
