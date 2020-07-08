Imports System.Xml
Imports GKsrv
Imports MSXML2

Public Class GKSRVApp

    'Private m_GKSrv As GKsrv.cSrv
    Private m_import As String
    Private m_export As String
    Private m_ini As String
    Private m_product As String
    Private m_client As String
    Private m_licence As String
    Private m_GKSrv As GKsrv.cSrv
    Private m_Importclass As GKsrv.IImport
    Private result As String
    Private result1 As String
    Public Function ProcessFile(ImportFile As String, OutputFile As String, ProductFile As String, ClientFile As String, LicenseKey As String)
        Try
            If InitFiles(ImportFile, OutputFile, ProductFile, ClientFile, LicenseKey) = False Then Exit Function

            If InitDLL() = False Then
                MsgBox("Es ist ein Fehler bei der Initialisierung der Bibliothek aufgetreten", vbCritical + vbOKOnly)
                Exit Function
            End If

            'Datei einlesen
            Import()

            'Datei exportieren
            Export()

            Exit Function
        Catch ex As Exception
            MsgBox("Bitte installieren Sie den GAEB-Konverter", vbCritical + vbOKOnly)
        End Try
    End Function

    Private Function InitFiles(strImportFile As String, strExportFile As String, strProductFile As String, strClientFile As String, strLicenseKey As String) As Boolean
        'import-Datei muss existieren
        'export-Datei muss angegeben werden
        Try


            m_import = strImportFile
            m_export = strExportFile
            ' m_ini = GetValueFromName(ActiveWorkbook, NAME_INI)

            m_product = strProductFile
            m_client = strClientFile
            m_licence = strLicenseKey

            InitFiles = True

            Exit Function

        Catch ex As Exception
            InitFiles = False
        End Try


    End Function

    Private Function InitDLL() As Boolean
        Dim lRet As Long
        Try
            'm_GKSrv = New GKsrv.cSrv
            m_GKSrv = New cSrv()
            m_GKSrv.LogFile = "D:\\gksrv.log"
            lRet = m_GKSrv.Init
            If lRet <> 0 Then
                MsgBox("Fehler: " & lRet)
                InitDLL = False
                Exit Function
            End If
            m_GKSrv.BO.Licencing.Init(m_product, m_client, "", m_licence)

            InitDLL = True
            Exit Function


        Catch ex As Exception
            Throw New Exception("Bitte installieren Sie den GAEB-Konverter")
            InitDLL = False

        End Try
    End Function

    Private Sub Import()
        Try
            If m_import.Substring(m_import.Length - 3) = "tml" Then
                'Sonderformat - tml-Dokument laden
                LoadTMLDoc(m_import)
                'Preise berechnen
                'm_GKSrv.Project.TTDocument.CalcLVPrices
            Else
                Dim stFile As String
                stFile = "D:\\Settings.ini"
                'GAEB-Datei importieren
                'm_GKSrv.PrepareImport(m_import, "", stFile)
                m_GKSrv.PrepareImport(m_import)
                If m_GKSrv.import < 0 Then
                    Throw New Exception("Bitte installieren Sie den GAEB-Konverter")
                    Exit Sub
                End If
            End If


            Exit Sub

        Catch ex As Exception
            Throw New Exception("Bitte installieren Sie den GAEB-Konverter")
        End Try

    End Sub

    Private Sub LoadTMLDoc(tmlFilePath As String)
        Try


            'Template für T&T-Dokument erstellen
            m_GKSrv.CreateDocument()

            'Dokument laden
            m_GKSrv.TTDocument.load(tmlFilePath)

            'ggF. ID's erstellen
            m_GKSrv.Project.TTDocument.Structure.DistributeIDs()

            Exit Sub

        Catch ex As Exception
            Throw New Exception("Bitte installieren Sie den GAEB-Konverter")
        End Try

    End Sub

    Private Sub Export()
        Try
            'Export Einstellungen übernehmen
            Select Case m_GKSrv.PrepareExport(m_export)
                Case 18
                    m_GKSrv.Export()
                Case 2
                    m_GKSrv.Export()
                    'SetExportSettings2(lstExpG90)
                Case 4, 5, 6
                    m_GKSrv.Export()
                    ' SetExportSettings ActiveWorkbook.Worksheets("Exp_G2K")
                Case 7
                    m_GKSrv.Export()
                    '  SetExportSettings ActiveWorkbook.Worksheets("Exp_GXML")

                    result = m_GKSrv.Project.ProtocolContainer.GetProtocoll("ExportMakeStructure").ToString
                    result1 = result


            End Select

            'exportieren
            m_GKSrv.Export()
            Exit Sub
        Catch ex As Exception

            Throw New Exception("Bitte installieren Sie den GAEB-Konverter")
            'Datei laden und anzeigen
            ' WriteFile(ActiveWorkbook.Worksheets("export_file"), m_export)
            '  ActiveWorkbook.Worksheets("export_file").Activate()
        End Try
    End Sub

End Class
