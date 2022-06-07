
Imports System.IO

Public Class INCITS_Resizer

    Public ReadOnly ANSI_378_EXT As String = ".ansi-fmr"
    Public ReadOnly ISO_19794_2_CF_CS_EXT As String = ".iso-fmc-cs"
    Public ReadOnly ANSI_378 As String = "ANSI_378"
    Public ReadOnly ISO_19794_2_CF_CS As String = "ISO_19794_2_CF_CS"
    Private _FileType As String = ""
    Private _MK_CONVERT_PATH As String = Application.StartupPath & "\Bin\mk_convert.exe"
    'Private _MK_CONVERT_PATH As String = "D:\EDEL\ACC\Software\Sagem\Sagem 5.3\MorphoKit_5_3\morphokit-5.3\vs80\Bin\mk_convert.exe"
    Private _ErrorMessage As String = ""
    Private _OutputFile As String = ""

    Public ReadOnly ANSI_378_FileFormat As Short = 10
    Public ReadOnly ISO_19794_2_CF_CS_FileFormat As Short = 22

    Public Property OutputFile() As String
        Get
            Return _OutputFile
        End Get
        Set(value As String)
            _OutputFile = value
        End Set
    End Property

    Public ReadOnly Property ErrorMessage() As String
        Get
            Return _ErrorMessage
        End Get
    End Property

    'Public Property MK_CONVERT_PATH() As String
    '    Get
    '        Return _MK_CONVERT_PATH
    '    End Get
    '    Set(value As String)
    '        _MK_CONVERT_PATH = value
    '    End Set
    'End Property

    Public Property FileType() As String
        Get
            Return _FileType
        End Get
        Set(value As String)
            _FileType = value
        End Set
    End Property

    Public Function ResizeIncits(ByVal inputFile As String) As Boolean
        If Convert(inputFile, ANSI_378_FileFormat, ISO_19794_2_CF_CS_FileFormat, ISO_19794_2_CF_CS_EXT) Then
            Dim _firstOutputFile As String = _OutputFile
            _OutputFile = inputFile
            If Not Convert(_firstOutputFile, ISO_19794_2_CF_CS_FileFormat, ANSI_378_FileFormat, ANSI_378_EXT, True) Then
                Return False
            Else
                Return True
            End If
        Else
            Return False
        End If
    End Function

    Public Function Convert(ByVal inputFile As String, ByVal inputFileFormat As Short, ByVal outputFileFormat As Short, ByVal outputFileExtension As String, Optional IsDeleteInputFile As Boolean = False) As Boolean
        Try
            Dim strTempFile As String = Application.StartupPath & "\tempFile" & outputFileExtension
            If File.Exists(strTempFile) Then File.Delete(strTempFile)
            'File.Copy(inputFile, strTempFile)

            If Path.GetExtension(inputFile).ToUpper = ".ANSI-FMR" And _FileType = ANSI_378 Then
                _ErrorMessage = "Please select format other than ANSI-FMR"
                Return False
            ElseIf Path.GetExtension(inputFile).ToUpper = ".ISO-FMC-CS" And _FileType = ISO_19794_2_CF_CS Then
                _ErrorMessage = "Please select format other than ISO-FMC-CS"
                Return False
            End If

            If File.Exists(_MK_CONVERT_PATH) Then
                If _OutputFile = "" Then _
                    _OutputFile = Path.GetDirectoryName(inputFile) & "\" & Path.GetFileNameWithoutExtension(inputFile) & outputFileExtension

                'Dim startInfo As New ProcessStartInfo("D:\EDEL\ACC\Software\Sagem\Sagem 5.3\MorphoKit_5_3\morphokit-5.3\vs80\Bin\mk_convert.exe")
                Dim startInfo As New ProcessStartInfo(_MK_CONVERT_PATH)
                startInfo.WindowStyle = ProcessWindowStyle.Hidden
                startInfo.Arguments = String.Format("-i ""{0}"" -f {1} -t {2} -o ""{3}""", inputFile, inputFileFormat, outputFileFormat, strTempFile)
                Process.Start(startInfo)
                System.Threading.Thread.Sleep(2000)
                Application.DoEvents()

                'converstion takes time, loop 10x with 1 second interval to wait for converted file
                For i As Short = 1 To 10
                    If File.Exists(strTempFile) Then
                        Exit For
                    Else
                        System.Threading.Thread.Sleep(1000)
                        Application.DoEvents()
                    End If
                Next

                If File.Exists(strTempFile) Then
                    If File.Exists(_OutputFile) Then File.Delete(_OutputFile)
                    If IsDeleteInputFile Then File.Delete(inputFile)
                    File.Copy(strTempFile, _OutputFile)
                    If File.Exists(strTempFile) Then File.Delete(strTempFile)
                    Return True
                Else
                    _ErrorMessage = String.Format("No converted file has been created", _MK_CONVERT_PATH)
                    Return False
                End If
            Else
                _ErrorMessage = String.Format("Unable to find '{0}'", _MK_CONVERT_PATH)
                Return False
            End If
        Catch ex As Exception
            _ErrorMessage = ex.Message
            Return False
        End Try
    End Function

End Class
