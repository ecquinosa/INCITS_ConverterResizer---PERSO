
Imports System.IO

Public Class Form1

    Private Sub Button2_Click(sender As System.Object, e As System.EventArgs) Handles Button2.Click
        If TextBox1.Text = "" Then
            MessageBox.Show("Please enter file..")
            TextBox1.Focus()
            Exit Sub
        Else
            If New FileInfo(TextBox1.Text).Length < 1024 Then
                MessageBox.Show("File is not greater than 1024 bytes..")
                TextBox1.Focus()
                Exit Sub
            End If
        End If

        Button1.Enabled = False
        Button2.Enabled = False

        Dim ir As New INCITS_Resizer
        If ComboBox1.Text = ir.ANSI_378 Then
            ir.FileType = ir.ANSI_378
            If ir.Convert(TextBox1.Text, ir.ANSI_378_FileFormat, ir.ISO_19794_2_CF_CS_FileFormat, ir.ISO_19794_2_CF_CS_FileFormat) Then
                MessageBox.Show("Success!")
            Else
                MessageBox.Show("Failed!" & vbNewLine & vbNewLine & ir.ErrorMessage)
            End If
        ElseIf ComboBox1.Text = ir.ISO_19794_2_CF_CS Then
            If ir.ResizeIncits(TextBox1.Text) Then
                MessageBox.Show("Success!")
            Else
                MessageBox.Show("Failed!" & vbNewLine & vbNewLine & ir.ErrorMessage)
            End If
        End If

        Button1.Enabled = True
        Button2.Enabled = True
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        BrowseFile(TextBox1)
    End Sub

    Private Sub BrowseFile(ByVal txtbox As TextBox)
        Dim ofd As New OpenFileDialog
        If ofd.ShowDialog = Windows.Forms.DialogResult.OK Then
            txtbox.Text = ofd.FileName
        End If
        ofd.Dispose()
    End Sub


    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        ComboBox1.Items.Add("ANSI_378")
        ComboBox1.Items.Add("ISO_19794_2_CF_CS")
        ComboBox1.SelectedIndex = 1
    End Sub

End Class
