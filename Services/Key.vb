Imports System.Net.NetworkInformation
Imports Excel = Microsoft.Office.Interop.Excel
Public Class Key
    Private Sub Key_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        lblCodigo.Text = (getMacAddressLocal())
    End Sub

    Public Function GetMAC() As String
        Dim str As String
        Dim p As New Process

        'StartInfo representa el conjunto de parámetros que se van a
        p.StartInfo.UseShellExecute = False
        p.StartInfo.RedirectStandardOutput = True
        p.StartInfo.FileName = "GetMac.exe"
        p.StartInfo.Arguments = "/fo list"
        p.Start()
        'StandardOutput Obtiene una secuencia que se utiliza
        str = p.StandardOutput.ReadLine
        str = p.StandardOutput.ReadLine
        p.WaitForExit()
        Return str.Substring(23)
    End Function


    Public Function getMacAddressLocal() As String
        Dim mac As String = ""
        Dim interfaces() As NetworkInterface
        interfaces = NetworkInterface.GetAllNetworkInterfaces()
        If (interfaces.Length > 0 And Not IsDBNull(interfaces)) Then

            For Each adaptador As NetworkInterface In interfaces

                Dim direccion As PhysicalAddress = adaptador.GetPhysicalAddress()
                Dim name As String = adaptador.Name
                If (name.ToUpper Like "*LOCAL*") Then
                    mac = direccion.ToString
                    Exit For
                End If
            Next
        End If
        Return mac
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        ''Dim exapp = New IExcelDataReader
        'Dim exapp As IExcelDataReader = ExcelReaderFactory.CreateOpenXmlReader(stream)
        'Workbook wb = exapp.Workbooks.Open(@"archivo.xls")
        'Worksheet ws = wb.Worksheets[1]

        'ws.Range["B2:P15"].PrintOut()
        'exapp.Quit()



        'Dim file As String = "D:\doc.xlsx"
        'Dim excelApp = New Excel.Application()

        'Dim books = excelApp.Workbooks
        'Dim sheet = books.Open(file)
        'excelApp.Visible = True ' True will open Excel
        'sheet.PrintPreview()
        'excelApp.Visible = False '// hides excel file When user closes preview
        'workbook.Print(printerName, printOptions)







        '' If using Professional version, put your serial key below.
        ''SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY")

        '' Load Excel workbook from file's path.
        'Dim workbook As ExcelFile = ExcelFile.Load("CombinedTemplate.xlsx")

        '' Set sheets print options.
        'For Each worksheet As ExcelWorksheet In workbook.Worksheets
        '    Dim sheetPrintOptions As ExcelPrintOptions = worksheet.PrintOptions

        '    sheetPrintOptions.Portrait = False
        '    sheetPrintOptions.HorizontalCentered = True
        '    sheetPrintOptions.VerticalCentered = True

        '    sheetPrintOptions.PrintHeadings = True
        '    sheetPrintOptions.PrintGridlines = True
        'Next

        '' Create spreadsheet's print options. 
        'Dim printOptions As New PrintOptions()
        'printOptions.SelectionType = SelectionType.EntireFile

        '' Print Excel workbook to default printer (e.g. 'Microsoft Print to Pdf').
        'Dim printerName As String = Nothing
        'workbook.Print(printerName, printOptions)


    End Sub
End Class