Imports System.Runtime.InteropServices
Imports System.Text
Public Class FolderPickerHelper
    <DllImport("shell32.dll", CharSet:=CharSet.Auto)>
    Private Shared Function SHBrowseForFolder(ByRef bi As BROWSEINFO) As IntPtr
    End Function

    <DllImport("shell32.dll", CharSet:=CharSet.Auto)>
    Private Shared Function SHGetPathFromIDList(ByVal pidl As IntPtr, ByVal pszPath As StringBuilder) As Boolean
    End Function

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)>
    Private Structure BROWSEINFO
        Public hwndOwner As IntPtr
        Public pidlRoot As IntPtr
        <MarshalAs(UnmanagedType.LPTStr)>
        Public pszDisplayName As String
        <MarshalAs(UnmanagedType.LPTStr)>
        Public lpszTitle As String
        Public ulFlags As UInteger
        Public lpfn As IntPtr
        Public lParam As IntPtr
        Public iImage As Integer
    End Structure

    Private Const BIF_RETURNONLYFSDIRS As UInteger = &H1
    Private Const BIF_NEWDIALOGSTYLE As UInteger = &H40

    Public Shared Function SelectFolder(owner As IntPtr, Optional title As String = "Select a folder...", Optional allowNewFolderButton As Boolean = True) As String
        Dim buffer As New StringBuilder(260)

        Dim flags As UInteger = BIF_RETURNONLYFSDIRS
        If allowNewFolderButton Then
            flags = flags Or BIF_NEWDIALOGSTYLE
        End If

        Dim bi As New BROWSEINFO With {
            .hwndOwner = owner,
            .pszDisplayName = New String(" "c, 260),
            .lpszTitle = title,
            .ulFlags = flags
        }

        Dim pidl As IntPtr = SHBrowseForFolder(bi)

        If pidl <> IntPtr.Zero Then
            If SHGetPathFromIDList(pidl, buffer) Then
                Return buffer.ToString()
            End If
        End If

        Return Nothing
    End Function
End Class
