﻿Imports System.Windows.Threading
Namespace CompareTwoFolders
    Public Module DoEventsModule
        Public Sub DoEvents()
            Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Background, New Action(Function()
                                                                                              Return Nothing
                                                                                          End Function))
        End Sub
    End Module
End Namespace
