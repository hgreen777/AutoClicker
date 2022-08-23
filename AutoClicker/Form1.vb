Imports System.Runtime.InteropServices
Public Class Form1
    '143 x 255
    Public Declare Function SetCursorPos Lib "User32.dll" (ByVal x As Integer, ByVal y As Integer) As Long
    Public Declare Auto Function GetCursorPos Lib "User32.dll" (ByRef p As Point) As Long

    Private Declare Sub mouse_event Lib "user32" (ByVal dwflags As Long, ByVal dx As Long, ByVal cbuttons As Long, ByVal dy As Long, ByVal dwExtraInfo As Long)
    Private Const mouseclickup = 4
    Private Const mouseclickdown = 2
    Private Const mouserightdown = 8
    Private Const mouserightup = 10

    Dim running As Boolean = False

    Dim milli As Integer
    Dim sec As Integer
    Dim mins As Integer


    Dim test As Integer


    Dim clickCount As Integer = 0
    Dim clickAmount As Integer = 1

    Dim delay As Integer
    Dim selectedItem As String
    Dim selectedItem2 As String


    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        delayTimer.Enabled = False

        If RadioButton4.Checked = True Then
            Dim x As Integer
            Dim y As Integer

            Dim test1 As String = TextBox6.Text
            Dim test2 As String = TextBox7.Text

            Try
                x = CInt(test1)
                y = CInt(test2)

                SetCursorPos(x, y)

            Catch ex As Exception
                MsgBox(ex.Message)

            End Try

        End If




        If selectedItem = "Left" Then

            If selectedItem2 = "Double" Then
                mouse_event(mouseclickdown, 0, 0, 0, 0)
                mouse_event(mouseclickup, 0, 0, 0, 0)
                mouse_event(mouseclickdown, 0, 0, 0, 0)
                mouse_event(mouseclickup, 0, 0, 0, 0)
            ElseIf selectedItem2 = "Single" Then

                mouse_event(mouseclickdown, 0, 0, 0, 0)
                mouse_event(mouseclickup, 0, 0, 0, 0)
            End If

        ElseIf selectedItem = "Right" Then
            If selectedItem2 = "Double" Then
                mouse_event(mouserightdown, 0, 0, 0, 0)
                mouse_event(mouserightup, 0, 0, 0, 0)
                mouse_event(mouserightdown, 0, 0, 0, 0)
                mouse_event(mouserightup, 0, 0, 0, 0)
            ElseIf selectedItem2 = "Single" Then
                mouse_event(mouserightdown, 0, 0, 0, 0)
                mouse_event(mouserightup, 0, 0, 0, 0)
            End If

        End If

        clickCount += 1
        If RadioButton1.Checked = True Then
            If clickAmount = clickCount Then
                running = False

                Button2.Enabled = False
                Button1.Enabled = True
                Button1.BackColor = Color.WhiteSmoke
                Button2.BackColor = Color.DarkGray

                Timer1.Enabled = False

            End If
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        If running = False Then

            delayTimer.Enabled = True
            running = True
            Button1.Enabled = False
            Button2.Enabled = True
            Button1.BackColor = Color.DarkGray
            Button2.BackColor = Color.WhiteSmoke
        End If


    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        Dim text As String = TextBox1.Text
        Try
            milli = CInt(text)
            Interval()
        Catch ex As Exception
            If TextBox1.Text = "" Then

            Else
                MsgBox(ex.Message)
            End If

        End Try



    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If running = True Then
            Timer1.Enabled = False
            running = False
            Button2.Enabled = False
            Button1.Enabled = True
            Button1.BackColor = Color.WhiteSmoke
            Button2.BackColor = Color.DarkGray

        End If

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TextBox1.Text = "100"
        ComboBox1.SelectedIndex = 0
        ComboBox2.SelectedIndex = 0
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        Dim text As String = TextBox2.Text
        Try
            sec = CInt(text)
            Interval()
        Catch ex As Exception
            If TextBox2.Text = "" Then

            Else
                MsgBox(ex.Message)
            End If

        End Try
    End Sub

    Private Sub Interval()
        Dim final As Integer
        final = milli + (sec * 1000) + (mins * 60 * 1000)

        Timer1.Interval = final
    End Sub

    Private Sub TextBox3_TextChanged(sender As Object, e As EventArgs) Handles TextBox3.TextChanged
        Dim text As String = TextBox3.Text
        Try
            mins = CInt(text)
            Interval()
        Catch ex As Exception
            If TextBox3.Text = "" Then

            Else
                MsgBox(ex.Message)
            End If

        End Try
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        test += 1
        Dim text As String = CInt(test)
        Label5.Text = text
    End Sub


    Private Sub TextBox4_TextChanged(sender As Object, e As EventArgs) Handles TextBox4.TextChanged
        Dim text As String = TextBox4.Text
        Try
            clickAmount = CInt(text)

        Catch ex As Exception

            MsgBox(ex.Message)
            MsgBox("Please enter a number")


        End Try
    End Sub

    Private Sub TextBox5_TextChanged(sender As Object, e As EventArgs) Handles TextBox5.TextChanged
        Dim text As String = TextBox5.Text
        Try
            delay = CInt(text)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        delayTimer.Interval = (delay * 1000) + 1
    End Sub

    Private Sub delayTimer_Tick(sender As Object, e As EventArgs) Handles delayTimer.Tick
        Timer1.Enabled = True
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        selectedItem = ComboBox1.Items(ComboBox1.SelectedIndex)
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        selectedItem2 = ComboBox2.Items(ComboBox2.SelectedIndex)
    End Sub


End Class
