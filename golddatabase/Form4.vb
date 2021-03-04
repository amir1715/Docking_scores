Public Class Form4

    Private Sub PrintToDefaultPrinterToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub Form4_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None
        Button1.Visible = False

        PrintForm1.Form = Me
        PrintForm1.Print()
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.FixedSingle
        Button1.Visible = True




    End Sub

    Public Sub Form4_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint

    End Sub
End Class