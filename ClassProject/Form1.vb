
Public Class Form1

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        Me.Dispose()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim a As Integer
        Dim b As Integer
        a = CInt(pprice.Text)
        b = CInt(punit.Text)
        Dim Num As Integer
        Num = 0
        Num = DataGridView1.Rows.Add()
        DataGridView1.Rows.Item(Num).Cells(0).Value = pname.Text
        DataGridView1.Rows.Item(Num).Cells(1).Value = pprice.Text
        DataGridView1.Rows.Item(Num).Cells(2).Value = punit.Text
        DataGridView1.Rows.Item(Num).Cells(3).Value = a * b
        cleartextboxes()
        total()
    End Sub
    Private Sub total()
        Dim sum As Integer = 0
        For i As Integer = 0 To DataGridView1.Rows.Count() - 1 Step +1
            sum = sum + DataGridView1.Rows(i).Cells(3).Value
        Next

        TextBox4.Text = sum.ToString()
    End Sub
    Private Sub cleartextboxes()
        pname.Clear()
        pprice.Clear()
        punit.Clear()
        pname.Select()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    End Sub

    Private Sub TextBox4_TextChanged(sender As Object, e As EventArgs) Handles TextBox4.TextChanged

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If DataGridView1.Rows.Count = 0 Then
            Exit Sub
        End If
        PrintPreviewDialog1.WindowState = FormWindowState.Maximized
        PrintPreviewDialog1.StartPosition = FormStartPosition.CenterScreen
        PrintPreviewDialog1.PrintPreviewControl.Zoom = 1.5
        PrintPreviewDialog1.Document = PrintDocument1
        PrintPreviewDialog1.ShowDialog()
        TextBox3.Text += 1
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If DataGridView1.SelectedRows.Count = 0 Then
            Exit Sub
        End If
        DataGridView1.Rows.Remove(DataGridView1.SelectedRows(0))
        total()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        DataGridView1.Rows.Clear()
        cleartextboxes()
        total()
    End Sub

    Private Sub PrintDocument1_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage

        Dim Rows As DataGridViewRowCollection = Me.DataGridView1.Rows
        Dim UnitWidth As Integer = 22
        Dim UnitHeight As Integer = 22
        Dim LeftMargin As Integer = 0
        Dim topMargin As Integer = 0

        Dim ReciptDetailsHeight = Rows.Count * UnitHeight

        Dim font As New Font("Times", 12)
        Dim fontB As New Font("Times", 12, FontStyle.Bold)

        Dim Fontbold As New Font("Times", 12, FontStyle.Bold)
        Dim FontboldHeader As New Font("Times", 16, FontStyle.Bold)


        Dim TRecwidth As Integer = 283
        Dim Str As New StringFormat
        Str.Alignment = StringAlignment.Center
        Dim strLeft As New StringFormat
        strLeft.Alignment = StringAlignment.Near
        Dim strRight As New StringFormat
        strRight.Alignment = StringAlignment.Far

        Dim YBillStart As Integer = 4 * UnitHeight

        Dim YHeaderStrat As Integer = YBillStart + (3 * UnitHeight)
        Dim YDetailsStart As Integer = YHeaderStrat + UnitHeight

        Dim Rect As New Rectangle(LeftMargin + 250, topMargin + 0, TRecwidth, UnitHeight + 10)
        e.Graphics.DrawRectangle(Pens.White, Rect)
        Dim Rect1 As New Rectangle(LeftMargin + 250, topMargin + UnitHeight, TRecwidth, UnitHeight)
        e.Graphics.DrawRectangle(Pens.White, Rect1)
        Dim Rect2 As New Rectangle(LeftMargin + 250, topMargin + UnitHeight * 2, TRecwidth, UnitHeight * 3)
        e.Graphics.DrawRectangle(Pens.White, Rect2)
        Dim Rect3 As New Rectangle(LeftMargin + 250, topMargin + UnitHeight * 4, TRecwidth, UnitHeight)
        e.Graphics.DrawRectangle(Pens.Black, Rect3)
        Dim Rect4 As New Rectangle(LeftMargin + 250, topMargin + UnitHeight * 5, TRecwidth, UnitHeight)
        e.Graphics.DrawRectangle(Pens.Black, Rect4)



        Dim DtValue As String = Format(Now.Date, "MMM-dd-yyyy")


        e.Graphics.DrawString("GRANITE STORE", FontboldHeader, Brushes.Black, Rect, Str)
        e.Graphics.DrawString("Ngong, Embulbul", Fontbold, Brushes.Black, Rect1, Str)
        e.Graphics.DrawString("Time: " & DateTimePicker1.Value, Fontbold, Brushes.Black, Rect2, Str)
        e.Graphics.DrawString("Name: " & TextBox1.Text, font, Brushes.Black, Rect3, strLeft)
        e.Graphics.DrawString("Address: " & TextBox2.Text, font, Brushes.Black, Rect4, strLeft)



        Dim Rect01 As New Rectangle(LeftMargin + UnitWidth * 5, topMargin + YHeaderStrat, UnitWidth * 3, UnitHeight)
        Dim Rect02 As New Rectangle(LeftMargin + UnitWidth * 8, topMargin + YHeaderStrat, UnitWidth * 8, UnitHeight)
        Dim Rect03 As New Rectangle(LeftMargin + UnitWidth * 16, topMargin + YHeaderStrat, UnitWidth * 5, UnitHeight)
        Dim Rect04 As New Rectangle(LeftMargin + UnitWidth * 21, topMargin + YHeaderStrat, UnitWidth * 5, UnitHeight)
        Dim Rect041 As New Rectangle(LeftMargin + UnitWidth * 26, topMargin + YHeaderStrat, UnitWidth * 6, UnitHeight)


        e.Graphics.DrawRectangle(Pens.Black, Rect01)
        e.Graphics.DrawRectangle(Pens.Black, Rect02)
        e.Graphics.DrawRectangle(Pens.Black, Rect03)
        e.Graphics.DrawRectangle(Pens.Black, Rect04)
        e.Graphics.DrawRectangle(Pens.Black, Rect041)

        e.Graphics.DrawString("S.No.", font, Brushes.Black, Rect01, Str)
        e.Graphics.DrawString("Item Name", font, Brushes.Black, Rect02, Str)
        e.Graphics.DrawString("Quantity", font, Brushes.Black, Rect03, Str)
        e.Graphics.DrawString("Price", font, Brushes.Black, Rect04, Str)
        e.Graphics.DrawString("Amount", font, Brushes.Black, Rect041, Str)
        ''

        e.Graphics.DrawRectangle(Pens.Black, LeftMargin + UnitWidth * 5, topMargin + YDetailsStart, UnitWidth * 3, ReciptDetailsHeight)
        e.Graphics.DrawRectangle(Pens.Black, LeftMargin + UnitWidth * 8, topMargin + YDetailsStart, UnitWidth * 8, ReciptDetailsHeight)
        e.Graphics.DrawRectangle(Pens.Black, LeftMargin + UnitWidth * 16, topMargin + YDetailsStart, UnitWidth * 5, ReciptDetailsHeight)
        e.Graphics.DrawRectangle(Pens.Black, LeftMargin + UnitWidth * 21, topMargin + YDetailsStart, UnitWidth * 5, ReciptDetailsHeight)
        e.Graphics.DrawRectangle(Pens.Black, LeftMargin + UnitWidth * 26, topMargin + YDetailsStart, UnitWidth * 6, ReciptDetailsHeight)

        Dim I As Integer
        For I = 0 To Rows.Count - 1
            Dim Yloc = UnitHeight * I + YDetailsStart

            Dim Rect1g As New Rectangle(LeftMargin + UnitWidth * 5, topMargin + Yloc, UnitWidth * 3, UnitHeight)
            Dim Rect2g As New Rectangle(LeftMargin + UnitWidth * 8, topMargin + Yloc, UnitWidth * 8, UnitHeight)
            Dim Rect3g As New Rectangle(LeftMargin + UnitWidth * 16, topMargin + Yloc, UnitWidth * 5, UnitHeight)
            Dim Rect4g As New Rectangle(LeftMargin + UnitWidth * 21, topMargin + Yloc, UnitWidth * 5, UnitHeight)
            Dim Rect5g As New Rectangle(LeftMargin + UnitWidth * 26, topMargin + Yloc, UnitWidth * 6, UnitHeight)

            e.Graphics.DrawString(I + 1, font, Brushes.Black, Rect1g, Str)
            e.Graphics.DrawString(Rows(I).Cells(0).Value, font, Brushes.Black, Rect2g, strLeft)
            e.Graphics.DrawString(Rows(I).Cells(1).Value, font, Brushes.Black, Rect3g, Str)
            e.Graphics.DrawString(Rows(I).Cells(2).Value, font, Brushes.Black, Rect4g, Str)
            e.Graphics.DrawString(Rows(I).Cells(3).Value, font, Brushes.Black, Rect5g, Str)

        Next

        Dim Rect4x As New Rectangle(LeftMargin + 107, topMargin + ReciptDetailsHeight + YBillStart + 1 + (UnitHeight * 5), TRecwidth, UnitHeight)
        e.Graphics.DrawString("Number of Items:  " & Rows.Count, Fontbold, Brushes.Black, Rect4x, strLeft)


        Dim Rect5 As New Rectangle(LeftMargin + 421, topMargin + ReciptDetailsHeight + YBillStart + 1 + (UnitHeight * 5), TRecwidth, UnitHeight)
        e.Graphics.DrawRectangle(Pens.Black, Rect5)
        Dim ReciptTotal As Decimal = TextBox4.Text
        e.Graphics.DrawString("Bill Amount:  " & ReciptTotal, Fontbold, Brushes.Black, Rect5, Str)

    End Sub

End Class
