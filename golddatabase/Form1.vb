Imports System.Drawing.Drawing2D
Imports System.IO

Public Class Form1
    Dim filenames(), safefnames() As String
    Dim maxchance As Single = 0
    Dim bestcont, ligand_type As String
    Dim getscores1, getscores2 As Integer
    Dim datacheck As Boolean = True

    'POST test of chance correaltion
    Sub post_test()
        Me.Cursor = Cursors.WaitCursor
        maxchance = 0
        If IsNumeric(TextBox1.Text) And IsNumeric(TextBox2.Text) Then
            RichTextBox1.Visible = True
            RichTextBox1.AppendText("***CHANCE CORRELATION RANDOM AUCS***" & vbCrLf)

            '    'number of chance correlation
            For chance As Integer = 1 To NumericUpDown2.Value
                Dim ssq As Integer
                Dim aRandom As New Random 'random number generator
                Dim aNumber As Integer = 0 'a palce to store a random number
                Dim aRndL As List(Of Integer)
                Dim tp, fn, fp, tn As Integer

                Dim ss As Integer = grdcl.Rows.Count

                Dim maxd(ss) As Single
                Dim act(ss) As String
                Dim counter2 As Integer = 0
                Dim jj As Single = 0
                Dim incr As Single = NumericUpDown1.Value
                Dim auc, spca, spcb, sea, seb, dif As Single
                Dim sc As Integer = 1
                Dim temp1 As Single = 0
                Dim tedad As Integer = Label8.Text
                Dim arr(tedad) As Single
                auc = 0

                '        'making random numbers without repeating for chance correlation 
                aRndL = New List(Of Integer)
                Do While aRndL.Count < tedad + 1
                    aNumber = aRandom.Next(0, tedad + 1) 'pick a number from zero to  number of activity scores inclusive
                    If aRndL.IndexOf(aNumber) = -1 Then 'not picked
                        aRndL.Add(aNumber) 'add it
                    End If
                Loop

                '        'getting the scores
                For ii As Integer = 1 To tedad
                    If Not grdcl.Item(sc, ii - 1).Value = "N" Then
                        arr(ii) = grdcl.Item(sc, ii - 1).Value
                    End If
                Next

                '        'sorting the scores
                For j = LBound(arr) To UBound(arr) - 1
                    For k = LBound(arr) To UBound(arr) - 1
                        If arr(k) < arr(k + 1) Then
                            temp1 = arr(k)
                            arr(k) = arr(k + 1)
                            arr(k + 1) = temp1
                        End If
                    Next
                Next


                '        'Geting maximum of score for each ligand
                If RDmore.Checked Then   'GOLD algorithm
                    For counter2 = LBound(arr) To UBound(arr)
                        For i As Integer = 1 To ss - 1
                            jj = arr(counter2)

                            If Not grdcl.Item(sc, i).Value = "N" Then
                                maxd(i) = grdcl.Item(sc, i).Value
                                act(i) = grdcl.Item(2, aRndL(i)).Value

                                Select Case True
                                    Case jj <= maxd(i) And act(i) = "A"
                                        tp += 1
                                    Case IsNumeric(maxd(i)) And jj > maxd(i) And act(i) = "A"
                                        fn += 1
                                    Case IsNumeric(maxd(i)) And jj <= maxd(i) And act(i) = "I"
                                        fp += 1
                                    Case IsNumeric(maxd(i)) And jj > maxd(i) And act(i) = "I"
                                        tn += 1
                                End Select
                            End If
                        Next
                        seb = sea
                        sea = 0
                        sea = System.Math.Round(tp / (tp + fn), 3)
                        spcb = spca
                        spca = 0
                        spca = (1 - System.Math.Round(tn / (tn + fp), 3))
                        dif = (spca - spcb) * (sea + seb) * 0.5
                        auc = auc + dif
                        tp = 0
                        tn = 0
                        fn = 0
                        fp = 0
                        dif = 0
                    Next

                Else  'GLide Algorithm 

                    For counter2 = UBound(arr) To LBound(arr) Step -1
                        For i As Integer = 1 To ss - 1
                            jj = arr(counter2)
                            If Not grdcl.Item(sc, i).Value = "N" Then
                                maxd(i) = grdcl.Item(sc, i).Value

                                act(i) = Trim(grdcl.Item(2, aRndL(i)).Value)
                                Select Case True
                                    Case IsNumeric(maxd(i)) And jj >= maxd(i) And act(i) = "A"
                                        tp += 1
                                    Case IsNumeric(maxd(i)) And jj < maxd(i) And act(i) = "A"
                                        fn += 1
                                    Case IsNumeric(maxd(i)) And jj >= maxd(i) And act(i) = "I"
                                        fp += 1
                                    Case IsNumeric(maxd(i)) And jj < maxd(i) And act(i) = "I"
                                        tn += 1
                                End Select
                            End If
                        Next
                        seb = sea
                        sea = 0
                        sea = System.Math.Round(tp / (tp + fn), 3)
                        spcb = spca
                        spca = 0
                        spca = (1 - System.Math.Round(tn / (tn + fp), 3))
                        dif = (spca - spcb) * (sea + seb) * 0.5
                        auc = auc + dif
                        tp = 0
                        tn = 0
                        fn = 0
                        fp = 0
                        dif = 0
                    Next
                End If
                RichTextBox1.AppendText(auc & vbCrLf)
                Me.Text = ssq & " % of chance correlation finished "
                If auc >= maxchance Then
                    maxchance = auc
                End If
                ssq += 1
                auc = 0
                spca = 0
                spcb = 0
                sea = 0
                dif = 0
                seb = 0

            Next
            RichTextBox1.AppendText("maximum random AUC is:  " & maxchance)
        End If

        Me.Cursor = Cursors.Arrow
    End Sub
    'EF% J Med Chem 2006, 49, 23, 6973
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Cursor = Cursors.WaitCursor

        Call data_check()
        If datacheck = True Then



            Dim arrone(grdcl.Rows.Count - 1) As Single
            Dim arr2(grdcl.Rows.Count - 1) As String
            Me.Text = "wait"
            Dim aks As New Bitmap(PictureBox1.Width, PictureBox1.Height)

            Dim sc As Integer = 1
            Dim gr As Graphics = Graphics.FromImage(aks)
            Dim rect As Rectangle = PictureBox1.DisplayRectangle

            rect.X += 60
            rect.Y += 60
            rect.Height = rect.Height - 120
            rect.Width = rect.Width - 120
            Dim zer2, org, org2, zer1, magh As Point

            org.X = rect.X
            org.Y = rect.Y + rect.Height
            org2.Y = org.Y - (rect.Height / 3)
            magh.X = rect.X + (rect.Width)
            magh.Y = rect.Y + 2 * (rect.Height / 3)
            zer2.X = org.X
            zer2.Y = org.Y

            Dim qq As New System.Drawing.Pen(Color.Black, 2)
            Dim qq3 As New System.Drawing.Pen(Color.Black, 0.05)

            gr.DrawRectangle(qq3, rect)
            qq3.Color = Color.Gold
            qq3.DashStyle = DashStyle.Dash
            qq.DashStyle = DashStyle.Solid


            gr.DrawString("  1", New Font("calibri", 9, FontStyle.Regular), Brushes.Blue, org.X - 30, org.Y - 5 - (1 + System.Math.Log10(1)) * rect.Height / 3)
            gr.DrawString(" 10", New Font("calibri", 9, FontStyle.Regular), Brushes.Blue, org.X - 30, org.Y - 5 - (1 + System.Math.Log10(10)) * rect.Height / 3)
            gr.DrawString(" 100", New Font("calibri", 9, FontStyle.Regular), Brushes.Blue, org.X - 30, org.Y - 5 - (1 + System.Math.Log10(100)) * rect.Height / 3)

            Dim qqq As Single

            For ttt As Integer = 20 To 90 Step 10
                qq3.Color = Color.YellowGreen
                qqq = org.Y - (1 + System.Math.Log10(ttt)) * rect.Height / 3
                gr.DrawLine(qq3, org.X, qqq, magh.X, qqq)
            Next
            For tt As Integer = 2 To 9 Step 1
                qq3.Color = Color.LightBlue
                qqq = org.Y - (1 + System.Math.Log10(tt)) * rect.Height / 3
                gr.DrawLine(qq3, org.X, qqq, magh.X, qqq)
            Next
            For tttt As Single = 0.2 To 1 Step 0.1
                qq3.Color = Color.Red
                qqq = org.Y - (1 + System.Math.Log10(tttt)) * rect.Height / 3
                gr.DrawLine(qq3, org.X, qqq, magh.X, qqq)
            Next

            gr.DrawString(" 100", New Font("calibri", 9, FontStyle.Regular), Brushes.Blue, org.X + (1 * rect.Width), org.Y + 10)
            gr.DrawString("  40", New Font("calibri", 9, FontStyle.Regular), Brushes.Blue, org.X + (0.4 * rect.Width), org.Y + 10)
            gr.DrawString("  60", New Font("calibri", 9, FontStyle.Regular), Brushes.Blue, org.X + (0.6 * rect.Width), org.Y + 10)
            gr.DrawString("  80", New Font("calibri", 9, FontStyle.Regular), Brushes.Blue, org.X + (0.8 * rect.Width), org.Y + 10)
            gr.DrawString("  1", New Font("calibri", 9, FontStyle.Regular), Brushes.Blue, org.X + 0.01 * rect.Width, org.Y + 10)
            gr.DrawString("  20", New Font("calibri", 9, FontStyle.Regular), Brushes.Blue, org.X + (0.2 * rect.Width), org.Y + 10)
            RichTextBox1.Clear()
            RichTextBox1.AppendText("%TP" & vbTab & "%RDb" & vbTab & vbTab & "EF" & vbTab & "Activ" & vbTab & "Total" & vbCrLf)
            Dim spca, spcb, sea, seb As Single
            Dim deltay, deltax As Double
            RichTextBox1.Visible = True
            Dim tp As Integer
            Dim ss As Integer = grdcl.Rows.Count - 2
            Dim maxd(ss) As Single
            Dim act(ss) As String
            'Geting maximum of score for each ligand
            For ii As Integer = 0 To ss

                If Not grdcl.Item(sc, ii).Value = "N" Then
                    arrone(ii) = grdcl.Item(sc, ii).Value
                    arr2(ii) = Trim(grdcl.Item(2, ii).Value)
                End If
            Next
            Dim j, k As Integer
            Dim temp1 As Single
            Dim temp2 As String
            'sorting DOCK scores
            If RDmore.Checked Then

                For j = LBound(arrone) To UBound(arrone) - 1

                    For k = LBound(arrone) To UBound(arrone) - 1
                        If arrone(k) < arrone(k + 1) Then
                            temp1 = arrone(k)
                            temp2 = arr2(k)
                            arrone(k) = arrone(k + 1)
                            arr2(k) = arr2(k + 1)
                            arrone(k + 1) = temp1
                            arr2(k + 1) = temp2
                        End If
                    Next
                Next
            Else

                For j = LBound(arrone) To UBound(arrone) - 1

                    For k = LBound(arrone) To UBound(arrone) - 1
                        If arrone(k) > arrone(k + 1) Then
                            temp1 = arrone(k)
                            temp2 = arr2(k)
                            arrone(k) = arrone(k + 1)
                            arr2(k) = arr2(k + 1)
                            arrone(k + 1) = temp1
                            arr2(k + 1) = temp2
                        End If
                    Next
                Next
            End If
            'reading the sorted data
            Dim fff As Integer = 1
            Dim eee, wwww As Single
            Dim efmax As Single = 0
            For i As Integer = LBound(arrone) To UBound(arrone) - 1

                maxd(i) = arrone(i)
                act(i) = arr2(i)
                Select Case True
                    Case act(i) = "A"
                        tp += 1
                End Select
                seb = sea
                zer1.Y = zer2.Y
                sea = 0
                zer2.Y = 0
                eee = (tp / Label6.Text) * 100
                wwww = (fff / Label8.Text) * 100
                sea = (eee / wwww)

                'deltay = sea * (rect.Height)

                'logarithmic y scale
                If sea = 0 Then
                    deltay = 0
                Else
                    deltay = (1 + System.Math.Log10(sea)) * rect.Height / 3
                End If
                zer2.Y = org.Y - deltay
                spcb = spca
                zer1.X = zer2.X
                zer2.X = 0
                deltax = System.Math.Round((fff / Label8.Text), 3)

                'logarithmic
                'zer2.X = org.X + (System.Math.Log10(deltax * 100) * rect.Width * 0.5)
                zer2.X = org.X + (deltax * rect.Width)
                RichTextBox1.AppendText(System.Math.Round(eee, 2) & vbTab & System.Math.Round(wwww, 2) & vbTab & vbTab & System.Math.Round(sea, 2) & vbTab & tp & vbTab & fff & vbCrLf)
                gr.DrawLine(qq, zer1.X, zer1.Y, zer2.X, zer2.Y)
                If sea >= efmax Then
                    efmax = System.Math.Round(sea, 2)
                End If
                deltax = 0
                deltay = 0
                eee = 0
                wwww = 0
                sea = 0
                fff += 1
            Next
            Me.Text = "Finished"
            gr.DrawString("EF(Max) =  " & efmax, New Font("calibri", 12, FontStyle.Italic), Brushes.Blue, org.X - 40 + (rect.Width / 2), org.Y - (rect.Height + 50))
            gr.DrawString("E F", New Font("calibri", 13, FontStyle.Regular), Brushes.Black, org.X - 35, org.Y - (rect.Height + 30))

            gr.DrawString("Ranked Database(%)", New Font("calibri", 12, FontStyle.Regular), Brushes.Black, org.X + (0.25 * rect.Width), org.Y + 30)
            PictureBox1.BackColor = Color.White
            PictureBox1.Image = aks
        End If
        Me.Cursor = Cursors.Arrow

    End Sub

    'ROC analysis: based on J.Med Chem,2005,48,7,2534
    Public Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Call data_check()
        If datacheck = False Then
            Exit Sub
        End If
        Dim aks As New Bitmap(PictureBox1.Width, PictureBox1.Height)
        Dim sc As Integer = 2
        Dim gr As Graphics = Graphics.FromImage(aks)
        Dim rect As Rectangle = PictureBox1.DisplayRectangle
        rect.X += 60
        rect.Y += 60
        rect.Height = rect.Height - 120
        rect.Width = rect.Width - 120
        Dim zer2, org, zer1, magh As Point
        org.X = rect.X
        org.Y = rect.Y + rect.Height
        magh.X = rect.X + rect.Width
        magh.Y = rect.Y
        zer2.X = org.X
        zer2.Y = org.Y
        Dim qq As New System.Drawing.Pen(Color.Black)
        Dim qq1 As New System.Drawing.Pen(Color.Green, 2)
        Dim qq2 As New System.Drawing.Pen(Color.Red, 0.1)
        gr.DrawRectangle(qq2, rect)

        gr.DrawString("  0", New Font("calibri", 9, FontStyle.Regular), Brushes.Blue, org.X - 30, org.Y)
        gr.DrawString("0.25", New Font("calibri", 9, FontStyle.Regular), Brushes.Blue, org.X - 30, org.Y - (0.25 * rect.Height))
        gr.DrawString("0.5", New Font("calibri", 9, FontStyle.Regular), Brushes.Blue, org.X - 30, org.Y - (0.5 * rect.Height))
        gr.DrawString("0.75", New Font("calibri", 9, FontStyle.Regular), Brushes.Blue, org.X - 30, org.Y - (0.75 * rect.Height))
        gr.DrawString("  1", New Font("calibri", 9, FontStyle.Regular), Brushes.Blue, org.X - 30, org.Y - (1 * rect.Height))
        gr.DrawString("1", New Font("calibri", 9, FontStyle.Regular), Brushes.Blue, org.X + (rect.Width * 1), org.Y + 10)
        gr.DrawString("0.75", New Font("calibri", 9, FontStyle.Regular), Brushes.Blue, org.X + (rect.Width * 0.75), org.Y + 10)
        gr.DrawString("0.5", New Font("calibri", 9, FontStyle.Regular), Brushes.Blue, org.X + (rect.Width * 0.5), org.Y + 10)
        gr.DrawString("0.25", New Font("calibri", 9, FontStyle.Regular), Brushes.Blue, org.X + (rect.Width * 0.25), org.Y + 10)
        gr.DrawLine(qq2, org.X, org.Y, magh.X, magh.Y)
        RichTextBox1.Clear()
        RichTextBox1.AppendText("Se(y)" & vbTab & "1-Sp(x)" & vbTab & "TP" & vbTab & "TN" & vbTab & "FP" & vbTab & "FN" & vbTab & vbCrLf)
        Dim low As Single = TextBox1.Text - 1
        Dim high As Single = TextBox2.Text + 1

        Dim incr As Single = NumericUpDown1.Value
        Dim auc, spca, spcb, sea, seb, dif As Single
        Dim deltay, deltax As Double
        RichTextBox1.Visible = True
        Dim tp, fn, fp, tn As Integer

        Dim ss As Integer = grdcl.Rows.Count - 2
        Dim maxd(ss) As Single
        Dim ac(ss) As Single
        Dim act(ss) As String

        'Geting maximum of score for each ligand
        If RDmore.Checked Then

            For jj As Single = high To low Step incr * -1



                For i As Integer = 0 To ss
                    If Not grdcl.Item(2, i).Value = "N" Then
                        maxd(i) = grdcl.Item(1, i).Value
                        act(i) = Trim(grdcl.Item(2, i).Value)
                        Select Case True
                            Case IsNumeric(maxd(i)) And jj <= maxd(i) And act(i) = "A"
                                tp += 1
                            Case IsNumeric(maxd(i)) And jj > maxd(i) And act(i) = "A"
                                fn += 1
                            Case IsNumeric(maxd(i)) And jj <= maxd(i) And act(i) = "I"
                                fp += 1
                            Case IsNumeric(maxd(i)) And jj > maxd(i) And act(i) = "I"
                                tn += 1

                        End Select
                    End If
                Next

                seb = sea
                zer1.Y = zer2.Y
                sea = 0
                zer2.Y = 0
                sea = System.Math.Round(tp / (tp + fn), 3)
                deltay = sea * rect.Height
                zer2.Y = org.Y - deltay
                spcb = spca
                zer1.X = zer2.X
                spca = 0
                zer2.X = 0
                spca = (1 - System.Math.Round(tn / (tn + fp), 3))
                deltax = spca * rect.Width
                zer2.X = org.X + deltax
                dif = (spca - spcb) * (sea + seb) * 0.5
                auc = auc + dif
                RichTextBox1.AppendText(System.Math.Round(sea, 2) & vbTab & System.Math.Round(spca, 2) & vbTab & System.Math.Round(tp, 2) & vbTab & System.Math.Round(tn, 2) & vbTab & System.Math.Round(fp, 2) & vbTab & System.Math.Round(fn, 2) & vbCrLf)
                gr.DrawLine(qq1, zer1.X, zer1.Y, zer2.X, zer2.Y)
                tp = 0
                tn = 0
                fn = 0
                fp = 0
                dif = 0
                deltax = 0
                deltay = 0
            Next
        Else  'GLide case

            For jj As Single = low To high Step incr

                For i As Integer = 0 To ss
                    If Not grdcl.Item(2, i).Value = "N" Then
                        maxd(i) = grdcl.Item(1, i).Value
                        act(i) = Trim(grdcl.Item(2, i).Value)
                        Select Case True
                            Case IsNumeric(maxd(i)) And jj >= maxd(i) And act(i) = "A"
                                tp += 1
                            Case IsNumeric(maxd(i)) And jj < maxd(i) And act(i) = "A"
                                fn += 1
                            Case IsNumeric(maxd(i)) And jj >= maxd(i) And act(i) = "I"
                                fp += 1
                            Case IsNumeric(maxd(i)) And jj < maxd(i) And act(i) = "I"
                                tn += 1
                        End Select
                    End If
                Next
                seb = sea
                zer1.Y = zer2.Y
                sea = 0
                zer2.Y = 0
                sea = System.Math.Round(tp / (tp + fn), 3)
                deltay = sea * rect.Height
                zer2.Y = org.Y - deltay
                spcb = spca
                zer1.X = zer2.X
                spca = 0
                zer2.X = 0
                spca = (1 - System.Math.Round(tn / (tn + fp), 3))
                deltax = spca * rect.Width
                zer2.X = org.X + deltax
                dif = (spca - spcb) * (sea + seb) * 0.5
                auc = auc + dif
                RichTextBox1.AppendText(System.Math.Round(sea, 2) & vbTab & System.Math.Round(spca, 2) & vbTab & System.Math.Round(tp, 2) & vbTab & System.Math.Round(tn, 2) & vbTab & System.Math.Round(fp, 2) & vbTab & System.Math.Round(fn, 2) & vbCrLf)
                gr.DrawLine(qq1, zer1.X, zer1.Y, zer2.X, zer2.Y)
                tp = 0
                tn = 0
                fn = 0
                fp = 0
                dif = 0
                deltax = 0
                deltay = 0
            Next
        End If
        RichTextBox1.AppendText("Total CURVE AUC is:  " & auc & vbCrLf & vbCrLf)
        Me.Text = "Finished"
        gr.DrawString("AUC =  " & System.Math.Round(auc, 3), New Font("calibri", 11, FontStyle.Bold), Brushes.Green, org.X + 40, org.Y - (rect.Height + 30))
        gr.DrawString("(Se)", New Font("calibri", 13, FontStyle.Bold), Brushes.Black, org.X - 30, org.Y - (rect.Height + 30))
        gr.DrawString("(1 - Sp)", New Font("calibri", 13, FontStyle.Bold), Brushes.Black, org.X + (0.5 * rect.Width), org.Y + 30)

        PictureBox1.BackColor = Color.White
        PictureBox1.Image = aks

        If CheckBox1.Checked Then
            Call post_test()
            If maxchance < auc Then
                MsgBox("statistically SIGNIFICANT **>99%: No Chance correlation")
            Else
                MsgBox("Not significant")
            End If
        End If
        Me.Cursor = Cursors.Arrow
        ' End If
    End Sub

    Private Sub ROCPlotToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ROCPlotToolStripMenuItem.Click

        SaveFileDialog1.Filter = "JPEG file (*.jpg)|*.jpg"
        If Not SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.Cancel Then
            PictureBox1.Image.Save(SaveFileDialog1.FileName)

        End If
    End Sub

    Private Sub ToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem1.Click
        Call ROCPlotToolStripMenuItem_Click(Nothing, Nothing)

    End Sub

    Private Sub ROCResultToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ROCResultToolStripMenuItem.Click
        SaveFileDialog1.Filter = "Excel file (*.xls)|*.xls"

        If Not SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.Cancel Then
            Dim pat As String = SaveFileDialog1.FileName
            RichTextBox1.SaveFile(pat, RichTextBoxStreamType.PlainText)
        End If
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub OpenCSVFileToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenCSVFileToolStripMenuItem.Click
        OpenFileDialog1.Filter = "Comma delimited text file (*.csv)|*.csv"
        If Not OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.Cancel Then
            Me.Cursor = Cursors.WaitCursor
            Dim itemm As String = OpenFileDialog1.FileName
            Dim fn As Integer = FreeFile()
            FileOpen(fn, itemm, OpenMode.Input)
            Dim i As Integer = grdcl.Rows.Count
            While Not EOF(fn)
                Dim temp1 As String() = Split(LineInput(fn), ",")
                grdcl.Rows.Add()
                grdcl.Item(0, i - 1).Value = Trim(temp1(0))
                grdcl.Item(1, i - 1).Value = Trim(temp1(1))
                grdcl.Item(2, i - 1).Value = Trim(temp1(2))
                i += 1
            End While
            FileClose(fn)
            Call activecount()
            Button1.Enabled = True
            Button2.Enabled = True
            Me.Cursor = Cursors.Arrow
        End If
    End Sub

    Private Sub ReferencesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReferencesToolStripMenuItem.Click
        MsgBox("-ROC analysis based on; J.Med Chem,2005,48,7,2534" & vbCrLf & "-EF analysis based on; J. Med. Chem. 2006, 49, 6789-6801" & vbCrLf & "-SSLR based on: J. Chem. Inf. Model. 2009, 49, 444–460" & vbCrLf & "-Pearson based on: Kenney, J. F. and Keeping, E. S. (1962) Linear Regression and Correlation. Ch. 15 in Mathematics of Statistics, Pt. 1, 3rd ed. Princeton, NJ: Van Nostrand, pp. 252-285")
    End Sub

    Private Sub getscores()
        If Not OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.Cancel Then
            Dim i, j, n As Integer
            Dim rr, nn As String
            Dim fn As Integer
            Dim mmax, minn As Single
            mmax = -1000000
            minn = 100000
            Dim nu2 As Single
            filenames = OpenFileDialog1.FileNames
            safefnames = OpenFileDialog1.SafeFileNames
            i = grdcl.Rows.Count
            For Each filename As String In filenames
                grdcl.Rows.Add()
                Dim r As Integer = filenames.Count()
                fn = FreeFile()
                FileOpen(fn, filename, OpenMode.Input)
                grdcl.Item(0, i - 1).Value = safefnames(j)
                grdcl.Item(2, i - 1).Value = ligand_type
                While Not EOF(fn)
                    rr = LineInput(fn) & vbCrLf
                    If rr.Contains(bestcont) Then
                        If rr.Substring(2, 2) = " 1" Then
                            nn = Trim(rr.Substring(getscores1, getscores2))
                            grdcl.Item(1, i - 1).Value = Trim(nn)
                            If nu2 > mmax Then
                                mmax = nu2
                            ElseIf nu2 < minn Then
                                minn = nu2
                            End If
                            i += 1
                        End If
                    End If
                End While
                FileClose(fn)
                j += 1
                Me.Text = filename
            Next
            n = i - 1
            Label8.Text = n
            TextBox1.Text = minn
            TextBox2.Text = mmax
        End If
    End Sub

    Private Sub Form1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

        If (e.Control = True And e.KeyValue = Keys.S) Then
            Call ExportCSVFileToolStripMenuItem_Click(Nothing, Nothing)
        End If

        If (e.Control = True And e.KeyValue = Keys.I) Then
            Call OpenCSVFileToolStripMenuItem_Click(Nothing, Nothing)
        End If


    End Sub

    Private Sub ExportCSVFileToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExportCSVFileToolStripMenuItem.Click
        SaveFileDialog1.Filter = "Text file (*.csv)|*.csv"
        If Not SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.Cancel Then
            Dim filepath As String
            filepath = SaveFileDialog1.FileName
            Dim ww As String = ""
            Dim rows As Integer = grdcl.Rows.Count
            Dim colos As Integer = grdcl.ColumnCount
            For i As Integer = 0 To rows - 2

                For j As Integer = 0 To colos - 1
                    ww = ww & grdcl.Item(j, i).Value & ","
                Next
                File.AppendAllText(filepath, ww & vbCrLf)
                ww = ""
            Next
        End If
    End Sub

    Private Sub data_check()
        If Label6.Text = "" Then
            MsgBox("Error: Number of ligands (A) = 0 ")
            datacheck = False
            Exit Sub
        End If
        If IsNumeric(Label6.Text) Then
            If Label6.Text = 0 Then
                MsgBox("Error: Number of ligands (A) = 0 ")
                datacheck = False
                Exit Sub
            End If
        End If

        If Label6.Text = Label8.Text Then
            MsgBox("Error: Number of Decoys (I) = 0")
            datacheck = False
            Exit Sub

        Else
            datacheck = True

        End If
    End Sub

    Private Sub activecount()
        Dim max As Single = -10000
        Dim min As Single = 10000
        Dim k As Integer
        Dim num As Integer = 0
        For k = 0 To grdcl.Rows.Count - 2

            If grdcl.Item(2, k).Value = "A" Then
                num += 1
            End If
            Label6.Text = num
            Label8.Text = k + 1
            If grdcl.Item(1, k).Value > max Then
                max = grdcl.Item(1, k).Value
            End If
            If grdcl.Item(1, k).Value <= min Then
                min = grdcl.Item(1, k).Value
            End If
        Next
        TextBox1.Text = min
        TextBox2.Text = max
    End Sub

    Private Sub AboutToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem1.Click
        MsgBox("By: Amirhossein Sakhteman" & vbCrLf & "Department of Medicinal Chemistry, Faculty of Pharmacy" & vbCrLf & "Shiraz University of Medical Sciences, Shiraz, IRAN" & vbCrLf & "For more inforamtion please contact: asakhteman@sums.ac.ir")
    End Sub

    Private Sub autodock()
        OpenFileDialog1.Filter = "Autodock log file (*.dlg)|*.dlg"
        bestcont = "   1 | "
        getscores1 = 24
        getscores2 = 11
        'bestcont = "RANKING"
        'getscores1 = 20
        'getscores2 = 11
    End Sub

    Private Sub vina()
        OpenFileDialog1.Filter = "Vina text file (*.txt)|*.txt|Log file (*.log)|*.log|All files (*.*)|*.*"
        bestcont = " 1 "
        getscores1 = 6
        getscores2 = 12


    End Sub

    Private Sub LigandsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LigandsToolStripMenuItem.Click
        ligand_type = "A"
        Call vina()
        Call getscores()
        Call activecount()

    End Sub

    Private Sub DecoysToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DecoysToolStripMenuItem.Click
        ligand_type = "I"
        Call vina()
        Call getscores()
        Call activecount()

    End Sub

    Private Sub LigandsToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LigandsToolStripMenuItem1.Click
        ligand_type = "A"
        Call autodock()
        Call getscores()
        Call activecount()

    End Sub

    Private Sub DecoysToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DecoysToolStripMenuItem1.Click
        ligand_type = "I"
        Call autodock()
        Call getscores()
        Call activecount()

    End Sub

    Private Sub ResetScoresTableToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResetScoresTableToolStripMenuItem.Click
        grdcl.Rows.Clear()

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        If Label6.Text = "" Or Not Label6.Text = 0 Then
            MsgBox("The third column should be numeric")
            Exit Sub
        End If
        Dim aks As New Bitmap(PictureBox1.Width, PictureBox1.Height)
        Dim gr As Graphics = Graphics.FromImage(aks)
        Dim ret As Rectangle = PictureBox1.DisplayRectangle
        ret.X += 60
        ret.Y += 60
        Dim tedad As Integer = Label8.Text
        Dim real(tedad - 1) As Single
        Dim predicted(tedad - 1) As Single
        Dim rank(tedad) As Single
        Dim rank2(tedad) As Single
        Dim temp1, temp2, temp3, temp4 As Single

        'temp1 = 0
        'temp2 = 0
        'temp3 = 0
        Dim sc As Integer = 3
        Dim fc As Integer = 2
        'getting the scores
        For ii As Integer = 0 To tedad - 1
            real(ii) = grdcl.Item(sc - 1, ii).Value
            predicted(ii) = grdcl.Item(fc - 1, ii).Value
            'MsgBox(real(ii) & "      " & predicted(ii))
            rank2(ii) = ii + 1
            rank(ii) = ii + 1
        Next

        'For j = 0 To tedad
        '    MsgBox(predicted(j))
        'Next



        'sorting the scores based on real ic50 values
        For j = 0 To tedad
            For k = 0 To tedad - 2
                If real(k) < real(k + 1) Then
                    temp1 = real(k)
                    temp2 = predicted(k)
                    temp4 = grdcl.Item(0, k).Value
                    real(k) = real(k + 1)
                    predicted(k) = predicted(k + 1)
                    grdcl.Item(fc - 1, k).Value = predicted(k + 1)
                    grdcl.Item(sc - 1, k).Value = real(k + 1)
                    grdcl.Item(0, k).Value = grdcl.Item(0, k + 1).Value
                    real(k + 1) = temp1
                    predicted(k + 1) = temp2
                    grdcl.Item(fc - 1, k + 1).Value = temp2
                    grdcl.Item(sc - 1, k + 1).Value = temp1
                    grdcl.Item(0, k + 1).Value = temp4

                End If
            Next
        Next
  

    


        ' sorting the scores based on predicted values
        For j = 0 To tedad
            For k = 0 To tedad - 2
                If predicted(k) < predicted(k + 1) Then
                    temp1 = real(k)
                    temp2 = predicted(k)
                    temp3 = rank(k)
                    real(k) = real(k + 1)
                    predicted(k) = predicted(k + 1)
                    rank(k) = rank(k + 1)

                    real(k + 1) = temp1
                    predicted(k + 1) = temp2
                    rank(k + 1) = temp3
                End If
            Next
        Next

   

        'For j = 0 To tedad - 1
        '    MsgBox(predicted(j))
        'Next






        Dim sslr As Single = 0
        Dim sslref As Single = 0
        Dim g As Integer = 0
        RichTextBox1.Clear()
        RichTextBox1.AppendText("Weight" & vbTab & "rank" & vbTab & "SLR" & vbCrLf)
        For j = 0 To tedad - 1
            If RadioButton2.Checked = True Then
                RichTextBox1.AppendText(rank2(j) & vbTab & (tedad + 1) - rank(j) & vbTab & rank2(j) * System.Math.Log10((tedad + 1) - rank(j)) & vbCrLf)
                'MsgBox(rank(j))
                sslr += (tedad - g) * System.Math.Log10(rank(j))
                sslref += (tedad - g) * System.Math.Log10(g + 1)
                g += 1
            Else
                MsgBox("Error: The lower scores must be favored")
                RadioButton2.Checked = True
                Exit Sub
            End If
        Next
        RichTextBox1.AppendText("Calculated SSLR=" & sslr & vbCrLf & "Min possible of SSLR=" & sslref & vbCrLf)
        gr.DrawString("Sum of Sum of Log Rank is: " & sslr & vbCrLf & "Min possible of SSLR=" & sslref, New Font("calibri", 14, FontStyle.Regular), Brushes.Blue, ret.X, ret.Y)



        'post test checked
        If CheckBox1.Checked = True Then

            Dim rrr(NumericUpDown2.Value) As Single
            For f As Integer = 1 To NumericUpDown2.Value
                Dim aRndL As List(Of Integer)
                aRndL = New List(Of Integer)
                Dim aNumber As Integer = 1
                Do While aRndL.Count < tedad
                    Dim aRandom As New Random
                    aNumber = aRandom.Next(1, tedad + 1) 'pick a number from zero to  number of activity scores inclusive
                    If aRndL.IndexOf(aNumber) = -1 Then 'not picked yet
                        aRndL.Add(aNumber) 'add it
                    End If
                Loop
                For j As Integer = 0 To tedad - 1
                    RichTextBox1.AppendText(aRndL(j) & "_")
                Next
                g = 0
                For i As Integer = 1 To tedad
                    rrr(f) += (tedad - g) * System.Math.Log10(aRndL(i - 1))
                    g += 1
                Next
                RichTextBox1.AppendText("Random SSLR:" & rrr(f) & vbCrLf)
                If rrr(f) <= sslr Then
                    gr.DrawString("Not significant in " & f & "th Try", New Font("calibri", 14, FontStyle.Regular), Brushes.Red, ret.X, ret.Y + 50)
                    PictureBox1.Image = aks
                    Exit Sub
                End If

            Next
            gr.DrawString("Significant in " & NumericUpDown2.Value & " tests", New Font("calibri", 14, FontStyle.Regular), Brushes.Green, ret.X, ret.Y + 50)
        End If
        PictureBox1.Image = aks
    End Sub

    ' Kenney, J. F. and Keeping, E. S. (1962) "Linear Regression and Correlation." Ch. 15 in Mathematics of Statistics, Pt. 1, 3rd ed. Princeton, NJ: Van Nostrand, pp. 252-285
    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        If Label6.Text = "" Or Not Label6.Text = 0 Then
            MsgBox("The third column should be numeric")
            Exit Sub
        End If
        RichTextBox1.Clear()
        Dim qq As New System.Drawing.Pen(Color.Black, 0.2)
        Dim aks As New Bitmap(PictureBox1.Width, PictureBox1.Height)
        Dim gr As Graphics = Graphics.FromImage(aks)
        Dim rect As Rectangle = PictureBox1.DisplayRectangle
        rect.X += 60
        rect.Y += 60
        rect.Height = rect.Height - 120
        rect.Width = rect.Width - 120
        PictureBox1.BackColor = Color.White
        gr.DrawRectangle(qq, rect)
        Dim org As Point
        org.X = rect.X
        org.Y = rect.Y + rect.Height
        Dim minscore, maxscore, minactivity, maxactivity, sigmx, sigmy, avx, avy As Single

        minscore = grdcl.Item(1, 1).Value
        maxscore = grdcl.Item(1, 1).Value


        minactivity = grdcl.Item(2, 1).Value
        maxactivity = grdcl.Item(2, 1).Value

        Dim roco As Integer = grdcl.Rows.Count - 2

        For ozv As Integer = 0 To roco

            If grdcl.Item(1, ozv).Value < minscore Then
                minscore = grdcl.Item(1, ozv).Value
            End If
            If grdcl.Item(1, ozv).Value > maxscore Then
                maxscore = grdcl.Item(1, ozv).Value
            End If
            If grdcl.Item(2, ozv).Value < minactivity Then
                minactivity = grdcl.Item(2, ozv).Value
            End If
            If grdcl.Item(2, ozv).Value > maxactivity Then
                maxactivity = grdcl.Item(2, ozv).Value
            End If
            sigmx = sigmx + grdcl.Item(1, ozv).Value
            sigmy = sigmy + grdcl.Item(2, ozv).Value

        Next
        Dim deltay, deltax As Single
        avx = sigmx / (roco + 1)
        avy = sigmy / (roco + 1)

        deltay = System.Math.Abs(maxactivity - minactivity)
        deltax = System.Math.Abs(maxscore - minscore)

        Dim pon As Rectangle
        Dim sor, quot1, quot2, r, b, intercept As Single
        pon.Height = 4
        pon.Width = 4
        RichTextBox1.AppendText("no." & vbTab & "r" & vbTab & "b" & vbTab & "i" & vbCrLf)
        For ozv As Integer = 0 To roco
            sor = sor + ((grdcl.Item(1, ozv).Value) - avx) * ((grdcl.Item(2, ozv).Value) - avy)
            quot1 = quot1 + ((grdcl.Item(1, ozv).Value) - avx) ^ 2
            quot2 = quot2 + ((grdcl.Item(2, ozv).Value) - avy) ^ 2
            pon.X = rect.X - 2 + System.Math.Abs((grdcl.Item(1, ozv).Value) - minscore) * rect.Width / deltax
            pon.Y = rect.Y + rect.Height - 2 - (System.Math.Abs((grdcl.Item(2, ozv).Value) - minactivity) * rect.Height / deltay)
            If chktrnd.Checked = True Then
                gr.DrawString((grdcl.Item(0, ozv).Value), New Font("Calibri", 9, FontStyle.Regular), Brushes.LightSkyBlue, pon.X, pon.Y)
            End If
            gr.FillRectangle(Brushes.Red, pon)
            r = sor / (System.Math.Sqrt(quot1) * System.Math.Sqrt(quot2))
            b = sor / quot1
            intercept = avy - (b * avx)

        Next

        Dim mab, magh As Point
        mab.X = rect.X + System.Math.Abs(minscore - minscore) * rect.Width / deltax
        mab.Y = rect.Y + rect.Height - System.Math.Abs(((b * minscore) + intercept - minactivity)) * rect.Height / deltay

        magh.X = rect.X - 2 + System.Math.Abs(maxscore - minscore) * rect.Width / deltax
        magh.Y = rect.Y + rect.Height - System.Math.Abs(((b * maxscore) + intercept - minactivity)) * rect.Height / deltay


        If chktrnd.Checked = True Then
            gr.DrawLine(Pens.Green, mab, magh)
        End If
        RichTextBox1.AppendText("Pearson correlation coefficient= " & r & vbCrLf)
        RichTextBox1.AppendText("Slope= " & b & vbCrLf)
        RichTextBox1.AppendText("Intercept= " & intercept & vbCrLf)

        gr.DrawString("Y= " & System.Math.Round(b, 2) & "X" & " + " & System.Math.Round(intercept, 2) & vbCrLf & "r =  " & System.Math.Round(r, 2), New Font("calibri", 11, FontStyle.Bold), Brushes.Green, org.X - 30 + rect.Width / 2, org.Y - (rect.Height + 50))

        gr.DrawString(System.Math.Round(minactivity, 1), New Font("calibri", 9, FontStyle.Regular), Brushes.Blue, org.X - 30, org.Y - 10)
        gr.DrawString(System.Math.Round(maxactivity, 1), New Font("calibri", 9, FontStyle.Regular), Brushes.Blue, org.X - 30, org.Y - (1 * rect.Height))
        gr.DrawString(System.Math.Round((maxactivity + minactivity) / 2, 1), New Font("calibri", 9, FontStyle.Regular), Brushes.Blue, org.X - 30, org.Y - (1 * rect.Height / 2))
        gr.DrawString(System.Math.Round(minscore, 1), New Font("calibri", 9, FontStyle.Regular), Brushes.Blue, org.X, org.Y + 10)
        gr.DrawString(System.Math.Round(maxscore, 1), New Font("calibri", 9, FontStyle.Regular), Brushes.Blue, org.X + (rect.Width * 1), org.Y + 10)
        gr.DrawString(System.Math.Round((maxscore + minscore) / 2, 1), New Font("calibri", 9, FontStyle.Regular), Brushes.Blue, org.X + (rect.Width / 2 * 1), org.Y + 10)
        PictureBox1.Image = aks
    End Sub

    Private Sub chktrnd_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chktrnd.CheckedChanged
        Call Button4_Click(Nothing, Nothing)
    End Sub

    Private Sub grdcl_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles grdcl.CellClick, grdcl.CellContentClick, grdcl.CellEnter
        Call activecount()
    End Sub

End Class