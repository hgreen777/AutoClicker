﻿
Public Class Form1
    Private Declare Sub mouse_event Lib "user32" (ByVal dwflags As Long, ByVal dx As Long, ByVal cbuttons As Long, ByVal dy As Long, ByVal dwExtraInfo As Long)
    Private Const mouseclickup = 4
    Private Const mouseclickdown = 2


    Dim milli As Integer
    Dim sec As Integer
    Dim mins As Integer


    Dim test As Integer


    Dim clickCount As Integer = 0
    Dim clickAmount As Integer

    Dim delay As Integer

    Private Sub GroupBox1_Enter(sender As Object, e As EventArgs) Handles GroupBox1.Enter

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        mouse_event(mouseclickdown, 0, 0, 0, 0)
        mouse_event(mouseclickup, 0, 0, 0, 0)
        clickCount += 1
        If RadioButton1.Checked = True Then
            If clickAmount = clickCount Then
                Timer1.Enabled = False
            End If
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        delayTimer.Enabled = True


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
        Timer1.Enabled = False
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TextBox1.Text = "100"
        ComboBox1.SelectedText = "Left"
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

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged

    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton2.CheckedChanged

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
End Class
