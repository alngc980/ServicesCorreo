Imports System.Data.SqlClient
Imports System.IO

Imports System.Net.Mail
Imports System.Net
Imports System.Text.RegularExpressions
Imports Microsoft.Office.Interop
Imports EASendMail

Public Class InformeVentas
    Dim scn As New SqlConnection
    Dim dt As New DataTable
    Dim scmd As New SqlCommand
    Dim sda As New SqlDataAdapter
    Dim ds As New DataSet

    Public Conex, ruc, titulo, asunto, correoss, CorreoSalida, ClaveSalida As String

    Dim conexion As SqlConnection
    Dim rutadirecto As String
    Dim rutaarchivo As String
    Public Function conectar() As SqlConnection
        conexion = New SqlConnection(Conex)
        Return conexion
    End Function
    Sub Listar()
        dgvdia.DataSource = listar_Dias.Tables("VentasDia")
        dgvdia.Columns(0).Width = 150
        dgvdia.Columns(1).Visible = False
    End Sub
    Private Sub InformeVentas_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LeerTxt()
        Timer1.Start()
    End Sub
    Sub LeerTxt()
        ':::Creamos nuestro objeto de tipo StreamReader que nos permite leer archivos
        Dim leer As New StreamReader(Application.StartupPath & "\config.txt")
        Dim n As Integer
        n = 1
        Try
            ':::Indicamos mediante un While que mientras no sea el ultimo caracter repita el proceso
            While leer.Peek <> -1
                ':::Leemos cada linea del archivo TXT
                Dim linea As String = leer.ReadLine()
                ':::Validamos que la linea no este vacia
                If String.IsNullOrEmpty(linea) Then
                    Continue While
                End If

                Select Case n
                    Case 1
                        Conex = Trim(linea)
                    Case 2
                        ruc = Trim(linea)
                    Case 3
                        titulo = Trim(linea)
                    Case 4
                        asunto = Trim(linea)
                    Case 5
                        correoss = Trim(linea)
                    Case 6
                        CorreoSalida = Trim(linea)
                    Case 7
                        ClaveSalida = Trim(linea)
                End Select

                n = n + 1
            End While

            leer.Close()
        Catch ex As Exception
            'MsgBox("Se presento un problema al leer el archivo: " & ex.Message, MsgBoxStyle.Critical, ":::Aprendamos de Programación:::")
        End Try
    End Sub

    Public Function listar_Dias() As DataSet
        Try
            scn = conectar()
            scn.Open()
            ds.Clear()
            sda = New SqlDataAdapter("select dDia DIA, nEstado Estado from VentasDia where nEstado = 0 and dDia <> CONVERT(date, GETDATE()) order by dDia asc", scn)
            sda.Fill(ds, "VentasDia")
        Catch ex As Exception
            'MsgBox(ex.Message)
        Finally
            scn.Close()
            sda.Dispose()
            ds.Dispose()
        End Try
        Return ds
    End Function

    Function Internet() As Boolean
        If My.Computer.Network.IsAvailable Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If Internet() Then
            Listar()
            lblmensaje.Text = "Realizando el Informe..."
        Else
            lblmensaje.Text = "Verifica tu internet!"
        End If

        If dgvdia.RowCount > 0 Then
            If lblmensaje.Text = "Realizando el Informe..." Then
                Timer1.Stop()

                For Renglones As Integer = 0 To dgvdia.RowCount - 1
                    If GenVentasPlano(dgvdia.Item(0, Renglones).Value) Then

                        If File.Exists(Application.StartupPath & "\REPORT\" + ruc + "-" + Format(CDate(dgvdia.Item(0, Renglones).Value), "yyyyMMdd") + ".sql") Then
                            rutaDirecto = Application.StartupPath & "\REPORT\"
                            rutaArchivo = ruc + "-" + Format(CDate(dgvdia.Item(0, Renglones).Value), "yyyyMMdd") + ".sql"

                            If EnvioOutlook(asunto + "-" + Format(CDate(dgvdia.Item(0, Renglones).Value), "yyyy-MM-dd"),
                                                titulo + " " + Format(CDate(dgvdia.Item(0, Renglones).Value), "yyyy-MM-dd"),
                                             correoss, rutadirecto, rutaarchivo) Then
                                ActualizarEnvio(CDate(dgvdia.Item(0, Renglones).Value))
                            End If
                        Else
                            'MsgBox("NO SE HA GENERADO EL ARCHIVO!", MsgBoxStyle.Exclamation, Title:="MENSAJE")
                        End If
                    End If
                    Renglones = (dgvdia.RowCount - 1) + 10
                Next

                Listar()
                Timer1.Start()
            End If
        Else
            Application.ExitThread()
        End If
    End Sub

    Public Function GenVentasPlano(fecha As Date) As Boolean
        scn = conectar()
        scn.Open()
        Dim command0 As SqlCommand
        command0 = scn.CreateCommand()
        command0.CommandText = "rpt_Informe '" & Format(fecha, "yyyyMMdd") & "'"
        sda = New SqlDataAdapter(command0)
        Dim dt0 As New DataTable()
        sda.Fill(dt0)
        Dim textoFAC As String = ""
        Dim Salto = vbCrLf

        If dt0.Rows.Count() > 0 Then
            For i = 0 To dt0.Rows.Count() - 1 Step 1
                textoFAC = textoFAC + dt0.Rows(i)(0).ToString()
            Next
        Else
            textoFAC = textoFAC + "SELECT  'NO SE ENCONTRARON REGISTROS DE VENTAS PARA HOY'"
        End If

        'textoFAC = textoFAC + Salto + Salto + "DROP TABLE #VentasFact"

        If Not File.Exists(Application.StartupPath & "\REPORT\" + ruc + "-" + Format(CDate(fecha), "yyyyMMdd") + ".sql") Then
            'File.Delete(Application.StartupPath & "\REPORT\" + Format(CDate(fecha), "yyyyMMdd") + ".sql")
            Dim ruta As String = Application.StartupPath & "\REPORT\" + ruc + "-" + Format(CDate(fecha), "yyyyMMdd") + ".sql"
            Try
                Dim escritor As StreamWriter
                escritor = File.AppendText(ruta)
                escritor.Write(textoFAC)
                escritor.Flush()
                escritor.Close()
            Catch ex As Exception
            End Try
        End If

        Return True

    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs)
        'Envio()
        'sss()
    End Sub

    Public Sub ActualizarEnvio(ByVal Dia As Date)
        scn = conectar()
        scn.Open()
        Dim command As SqlCommand
        command = scn.CreateCommand()
        command.CommandText = "update VentasDia set nEstado = 1 where CONVERT(date,dDia) = CONVERT(date,'" + Format(Dia, "yyyyMMdd") + "')"
        command.ExecuteNonQuery()
    End Sub

    Public Function EsCorreo(ByVal email As String) As Boolean
        If email = String.Empty Then Return False
        ' Compruebo si el formato de la dirección es correcto.
        Dim re As Regex = New Regex("^[\w._%-]+@[\w.-]+\.[a-zA-Z]{2,4}$")
        Dim m As Match = re.Match(email)
        Return (m.Captures.Count <> 0)
    End Function


    Dim correos As New MailMessage()
    Dim envios As New Mail.SmtpClient()

    'Public Function enviarCorreo(mensaje As String, asunto As String, destinatario As String, ruta As String)
    '    Try
    '        correos.To.Clear()
    '        correos.Body = ""
    '        correos.Subject = ""
    '        correos.Body = mensaje
    '        correos.Subject = asunto
    '        correos.IsBodyHtml = True

    '        Dim texto As String = correoss
    '        'Split con array de delimitadores
    '        Dim delimitadores() As String = {";"}
    '        Dim vectoraux() As String
    '        vectoraux = texto.Split(delimitadores, StringSplitOptions.None)
    '        'mostrar resultado
    '        For Each item As String In vectoraux
    '            If EsCorreo(item.Trim()) Then
    '                correos.To.Add(item.Trim())
    '            End If
    '        Next

    '        correos.Attachments.Clear()
    '        If ruta.Equals("") = False Then
    '            Dim archivo As New System.Net.Mail.Attachment(ruta)
    '            correos.Attachments.Add(archivo)
    '        End If

    '        correos.From = New Mail.MailAddress(CorreoSalida)
    '        envios.Credentials = New NetworkCredential(CorreoSalida, ClaveSalida)

    '        'Datos importantes no modificables para tener acceso a las cuentas
    '        envios.Host = "smtp.gmail.com"
    '        envios.Port = 587
    '        envios.EnableSsl = True

    '        envios.Send(correos)
    '        Return True
    '    Catch ex As Exception
    '        Return False
    '    End Try

    'End Function
    Public Function EnvioOutlook(mensaje As String, asunto As String, destinatario As String, rutaDirecto As String, rutaArchivo As String) As Boolean
        Dim m_OutLook As Outlook.Application
        Try
            'Creamos un Objeto tipo Mail
            Dim objMail As Outlook.MailItem
            'Inicializamos nuestra apliación OutLook
            m_OutLook = New Outlook.Application
            'Creamos una instancia de un objeto tipo MailItem
            objMail = m_OutLook.CreateItem(Outlook.OlItemType.olMailItem)
            'Asignamos las propiedades a nuestra Instancial del objeto
            'MailItem
            objMail.To = destinatario
            objMail.Subject = asunto
            objMail.Body = mensaje

            'Si queremos enviar un archivo adjunto usamos este codigo…
            Dim sSource As String = rutaDirecto & rutaArchivo
            Dim sDisplayName As String = rutaArchivo
            Dim sBodyLen As String = objMail.Body.Length
            Dim oAttachs As Outlook.Attachments = objMail.Attachments
            Dim oAttach As Outlook.Attachment
            oAttach = oAttachs.Add(sSource, , sBodyLen + 1, sDisplayName)
            objMail.Send()
            Return True
        Catch ex As Exception
            Return False
        Finally
            m_OutLook = Nothing
        End Try


    End Function


End Class