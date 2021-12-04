Imports System.Drawing
Imports System.Drawing.Imaging
Public Class clsTIFFtools


    Friend Shared Function zerlegeMultipageTIFF(imageDatei As String, outdirTempTIFF As String) As Boolean
        '  "C:\Users\feinen_j\Desktop\61364.tiff")
        Dim activePage As Integer
        Dim pages As Integer
        Dim fi As New IO.FileInfo(imageDatei)
        Dim outpdf As String = imageDatei.Replace(".tiff", ".pdf")
        Dim imagefile As String
        Dim pngs() As String
        Dim Image As Image = Image.FromFile(imageDatei)
        Try
            l(" MOD zerlegeMultipageTIFF anfang" & imageDatei)
            outpdf = imageDatei.Replace(".tiff", ".pdf")
            pages = Image.GetFrameCount(FrameDimension.Page)
            For i = 0 To pages - 1
                activePage = i + 1
                Image.SelectActiveFrame(FrameDimension.Page, i)
                '// Creating an ImageData object  
                imagefile = outdirTempTIFF & "\page_" + activePage.ToString() + “.png”
                IO.File.Delete(imagefile)
                Image.Save(imagefile, ImageFormat.Png)
                ReDim Preserve pngs(i)
                pngs(i) = imagefile
            Next
            Image.Dispose()
            Image = Nothing

            wrapItextSharp.defineDinA4Dina3Formate()
            wrapItextSharp.createImagePdf(pngs, outpdf, 500, 600, True, outdirTempTIFF)
            l(" MOD zerlegeMultipageTIFF ende ok")
            Return True
        Catch ex As Exception
            l("Fehler in zerlegeMultipageTIFF: " & ex.ToString())
            Return False
        End Try
    End Function
End Class
