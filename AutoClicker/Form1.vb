Imports System.Runtime.InteropServices
Public Class Form1
    '143 x 255
    ' Roughly after 1 test termined software takes around 15ms per click.
    ' "Importing" Functions
    Public Declare Function SetCursorPos Lib "User32.dll" (ByVal x As Integer, ByVal y As Integer) As Long                                                      ' Used for setting the mouse position.
    Public Declare Auto Function GetCursorPos Lib "User32.dll" (ByRef p As Point) As Long                                                                       ' Used for getting the current mouse position.
    Private Declare Sub mouse_event Lib "user32" (ByVal dwflags As Long, ByVal dx As Long, ByVal cbuttons As Long, ByVal dy As Long, ByVal dwExtraInfo As Long) ' Using mouse_event function to handle mouse clikcs.
    Public Declare Function GetAsyncKeyState Lib "user32" (ByVal vKey As Long) As Integer                                                                       ' Used for listening for key presses.

    ' Declare the global keyboard hook function()
    Private Declare Function SetWindowsHookEx Lib "user32" Alias "SetWindowsHookExA" (ByVal idHook As Integer, ByVal lpfn As KeyboardHookDelegate, ByVal hmod As IntPtr, ByVal dwThreadId As Integer) As IntPtr

    ' Declare the keyboard hook delegate
    Private Delegate Function KeyboardHookDelegate(ByVal nCode As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr

    ' Declare the keyboard hook ID
    Private Const WH_KEYBOARD_LL As Integer = 13

    ' Declare the F6 key code
    Private Const VK_F6 As Integer = &H75

    ' Declare the keyboard hook handle
    Private keyboardHookHandle As IntPtr

    ' Create an instance of the keyboard hook delegate - need to remove and insert new gloabl keyboard hook code.
    'Private keyboardHookDelegate As KeyboardHookDelegate = New KeyboardHookDelegate(AddressOf KeyboardHookCallback)

    ' Declaring constants for interfacing with mouse_event.
    Private Const mouseclickup = &H4
    Private Const mouseclickdown = &H2
    Private Const mouserightdown = &H8
    Private Const mouserightup = &H10

    ' Declaring variables for the program.
    ' Handles the current status of the autoclicker.
    Dim running As Boolean = False
    Dim switch As Integer = 1

    ' Used to store the current interval between clicks.
    Dim milli As Integer
    Dim sec As Integer
    Dim mins As Integer
    Dim total As Integer

    ' Poorly named but used to keep track of clicks for the test click button.
    Dim test As Integer


    Dim clickCount As Integer = 0       ' Keeps track of the amount of clicks that have been performed so far.
    Dim clickAmount As Integer = 1      ' Used to store the amount of clicks the user wants to stop the autoclicker at. [Starts at 1 so textbox is not empty]

    Dim delay As Integer                ' Determins the delay between starting the autoclicker and the first click.
    Dim selectedItem As String          ' Determines whether left or right click.
    Dim selectedItem2 As String         ' Determines whether single or double click.

    ' PROCESSING DRIVER FUNCTIONS

    ' Handles actual Clicking functionality.
    Sub leftClick(x As Integer)
        For i = 1 To x
            mouse_event(mouseclickdown, 0, 0, 0, 0)
            mouse_event(mouseclickup, 0, 0, 0, 0)
        Next
    End Sub
    Sub rightClick(x As Integer)
        For i = 1 To x
            mouse_event(mouserightdown, 0, 0, 0, 0)
            mouse_event(mouserightup, 0, 0, 0, 0)
        Next
    End Sub

    ' PROCESSING EVENT LISTENERS

    ' Used for the delay timer to start the autoclicker.
    Private Sub delayTimer_Tick(sender As Object, e As EventArgs) Handles delayTimer.Tick
        ' As soon as this timer ticks, start the main timer for autoclicking.
        Timer1.Enabled = True
        delayTimer.Enabled = False  ' Stop the delay timer as it is not needed anymore.
    End Sub

    ' Handling the clicking when running [using clock ticks]
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        ' If the user wants to move the mouse to a specific location.
        If RadioButton4.Checked = True Then
            Dim x As Integer
            Dim y As Integer

            Try
                x = CInt(TextBox6.Text)
                y = CInt(TextBox7.Text)

                SetCursorPos(x, y)

            Catch ex As Exception
                MsgBox(ex.Message)
                Timer1.Enabled = False  ' Quit processing if there is an error.
            End Try

        End If

        ' Based of the users selection, the program will click the mouse.
        If selectedItem = "Left" Then

            If selectedItem2 = "Double" Then leftClick(2)
            If selectedItem2 = "Single" Then leftClick(1)

        ElseIf selectedItem = "Right" Then
            If selectedItem2 = "Double" Then rightClick(2)
            If selectedItem2 = "Single" Then rightClick(1)
        End If

        ' Handling if the user wants to stop the autoclicker after a certain amount of clicks.
        clickCount += 1
        If RadioButton1.Checked = True Then
            ' Disabling the clickign
            If clickAmount = clickCount Then
                ' Resetting variables.
                running = False

                ' Disabling timers and sorting start & stop buttons.
                Button2.Enabled = False
                Button1.Enabled = True
                Button1.BackColor = Color.WhiteSmoke
                Button2.BackColor = Color.DarkGray
                switch = 1                          ' Mode switch.
                Timer1.Enabled = False

            End If
        End If
    End Sub

    ' Start Button. [Used for starting the autoclicker]
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If running = False Then
            ' Check Valid Data Input - interval, click amount, delay.
            ' Checking valid interval data.
            Try
                milli = CInt(TextBox1.Text)
                sec = CInt(TextBox2.Text)
                mins = CInt(TextBox3.Text)

                total = milli + (sec * 1000) + (mins * 60 * 1000) / 100

                If total < 1 Then
                    MsgBox("Please enter a valid interval [ > 1millisecond")
                    Exit Sub
                End If

                Timer1.Interval = total
            Catch ex As Exception
                MsgBox(ex.Message)
                Exit Sub
            End Try
            ' Checking click amount data.
            If RadioButton1.Checked = True Then
                Try
                    clickAmount = CInt(TextBox4.Text)
                    If clickAmount < 1 Then
                        MsgBox("Please enter a valid click amount")
                        Exit Sub
                    End If
                Catch ex As Exception
                    MsgBox(ex.Message)
                    Exit Sub
                End Try
            End If
            ' Checking delay data. - ensure integer & >= 0
            Try
                delay = CInt(TextBox5.Text)
                If delay < 0 Then
                    MsgBox("Please enter a valid delay")
                    Exit Sub
                End If
                delayTimer.Interval = (delay * 1000) + 1
            Catch ex As Exception
                MsgBox(ex.Message)
                Exit Sub
            End Try



            ' Resetting the click count.
            clickCount = 0                          ' Resetting the click count.
            delayTimer.Enabled = True               ' Start the delay timer if there is a delay [if not will tick instantly and will start the autoclicker]
            ' Setting the mode to running.
            running = True
            switch = 2
            ' Handle buttons
            Button1.Enabled = False
            Button2.Enabled = True
            Button1.BackColor = Color.DarkGray
            Button2.BackColor = Color.WhiteSmoke

        End If
    End Sub

    ' Stop Button. [Used for stopping the autoclicker]
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If running = True Then

            Timer1.Enabled = False                  ' Stop the interval timer.
            ' Setting the mode to stopped.
            running = False
            switch = 1
            ' Handle buttons
            Button2.Enabled = False
            Button1.Enabled = True
            Button1.BackColor = Color.WhiteSmoke
            Button2.BackColor = Color.DarkGray

        End If
    End Sub

    ' Used for testing if the autoclicker is working. [Updates label based on the amount of clicks]
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        test += 1
        Dim text As String = CInt(test)
        Label5.Text = text
    End Sub


    ' SETTING UP FOR PROCESSING 
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

        ' Program minimizes, instructs user to click on the location they want the mouse to move to after closing the message box to reopen the program.
        ' Instruct user on what to do.
        MsgBox("After closing this message box, please click the location you want the mouse to move to.")
        ' Minimize the program.
        Me.WindowState = FormWindowState.Minimized

        ' Wait for the user to click & record the coordinates in p.
        Dim p As Point
        While True
            If GetAsyncKeyState(Keys.LButton) Then
                Exit While
            End If
        End While

        GetCursorPos(p)

        ' Update the UI to reflect the new coordinates.
        TextBox6.Text = p.X
        TextBox7.Text = p.Y

        ' Restore the program.
        Me.WindowState = FormWindowState.Normal
    End Sub

    ' Handles the click type selection. [Left or right]
    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        selectedItem = ComboBox1.Items(ComboBox1.SelectedIndex)
    End Sub

    ' Handles the click type selection. [Single or double]
    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        selectedItem2 = ComboBox2.Items(ComboBox2.SelectedIndex)
    End Sub

    ' Keyboard hook callback function
    Private Function KeyboardHookCallback(ByVal nCode As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
        If nCode >= 0 AndAlso wParam = IntPtr.Zero Then
            Dim vkCode As Integer = Marshal.ReadInt32(lParam)

            ' Check if the F6 key is pressed and if the process is currently running
            If vkCode = VK_F6 AndAlso switch = 1 Then
                Button1_Click(Nothing, Nothing)         ' Treat it like the user has just pressed the start button.
                switch = 2                              ' Set the mode to running.
            ElseIf vkCode = VK_F6 AndAlso switch = 2 Then
                Button2_Click(Nothing, Nothing)         ' Treat it like the user has just pressed the stop button.
                switch = 1                              ' Set the mode to stopped.
            End If
        End If

        Return CallNextHookEx(keyboardHookHandle, nCode, wParam, lParam)
    End Function

    ' Declare the CallNextHookEx function
    Private Declare Function CallNextHookEx Lib "user32" (ByVal hhk As IntPtr, ByVal nCode As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr

    ' Declare the UnhookWindowsHookEx function
    Private Declare Function UnhookWindowsHookEx Lib "user32" (ByVal hhk As IntPtr) As Boolean

    ' Start the keyboard hook
    Private Sub StartKeyboardHook()
        keyboardHookHandle = SetWindowsHookEx(WH_KEYBOARD_LL, keyboardHookDelegate, IntPtr.Zero, 0)
    End Sub

    ' Stop the keyboard hook
    Private Sub StopKeyboardHook()
        UnhookWindowsHookEx(keyboardHookHandle)
    End Sub

    ' ...
    ' please implement this ^^
    '

    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        ' Check if the F6 key is pressed adn if the process is currently running.
        If e.KeyCode = Keys.F6 And switch = 1 Then
            Button1_Click(Nothing, Nothing)         ' Treat it like the user has just pressed the start button.
            switch = 2                              ' set the mode to running.

        ElseIf e.KeyCode = Keys.F6 And switch = 2 Then
            Button2_Click(Nothing, Nothing)         ' Treat it like the user has just pressed the stop button.
            switch = 1                              ' set the mode to stopped.
        End If
    End Sub

    ' Initial processing for forms.
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Resetting all the textboxes and comboboxes.
        TextBox1.Text = "100"
        ComboBox1.SelectedIndex = 0
        ComboBox2.SelectedIndex = 0

        ' Start the keyboard hook
        StartKeyboardHook()

        Me.KeyPreview = True        ' Enable the form to listen for key presses. [Used for hot keys]
    End Sub

    ' Clean up when the form is closed
    Private Sub Form1_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        ' Stop the keyboard hook
        StopKeyboardHook()
    End Sub
End Class
